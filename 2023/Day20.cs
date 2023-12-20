namespace AdventOfCode;
using System.Text.RegularExpressions;

using Module = (char type, bool[] state, string[] inputs, string[] output);

public class Day20 : BaseDay
{
    private readonly string[] _input;
    Dictionary<string, Module> modules = [];

    public Day20()
    {
        var reg = new Regex(@"([\&\%])?(\S+) -> (.+)");
        _input = File.ReadAllText(InputFilePath).Split("\n");
        modules = [];
        modules.Add("button", (' ', [false], [], ["broadcaster"]));
        foreach (var line in _input.Where(x => x.Length > 0))
        {
            var groups = reg.Match(line).Groups.Values.ToList();
            modules.Add(groups[2].Value, (groups[1].Value.FirstOrDefault(), [false], [], groups[3].Value.Split(", ")));
        }

        foreach (var m in modules.Keys)
        {
            foreach (var o in modules[m].output.Where(x => modules.ContainsKey(x))) modules[o] = modules[o] with { inputs = modules[o].inputs.Append(m).ToArray() };
        }
        foreach (var m in modules.Keys.Where(k => modules[k].type == '&'))
        {
            modules[m] = modules[m] with { state = Enumerable.Repeat(false, modules[m].inputs.Length).ToArray() };
        }
    }

    public (bool, bool) UpdateAndNext(string m, string from, bool pulse)
    {
        if (!modules.ContainsKey(m))
        {
            throw new Exception($"Module not found: {m}");
        }
        switch (modules[m].type)
        {
            case '%':
                if (!pulse)
                {
                    modules[m] = modules[m] with { state = [!modules[m].state[0]] };
                    return (true, modules[m].state[0]);
                }
                return (false, false);
            case '&':
                var index = Array.FindIndex(modules[m].inputs, i => i == from);
                var module = modules[m];
                module.state[index] = pulse;
                modules[m] = module;
                return (true, !module.state.All(x => x));
            default:
                return (true, pulse);
        }
    }
    public override ValueTask<string> Solve_1()
    {
        Queue<(string, string, bool)> queue = [];
        Dictionary<bool, int> pulses = [];
        pulses.Add(false, 0);
        pulses.Add(true, 0);
        foreach (int _ in Enumerable.Range(0, 1000))
        {
            queue.Enqueue(("button", "", false));
            while (queue.Count > 0)
            {
                var q = queue.Dequeue();
                if (q.Item1 == "rx") continue;
                var b = UpdateAndNext(q.Item1, q.Item2, q.Item3);
                foreach (var o in modules[q.Item1].output)
                {
                    if (b.Item1)
                    {
                        queue.Enqueue((o, q.Item1, b.Item2));
                        pulses[b.Item2]++;
                    }
                }
            }
        }
        return new($"{pulses[false] * pulses[true]}");
    }
    public static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    public static long LCM(long a, long b) => (a / GCD(a, b)) * b;

    public override ValueTask<string> Solve_2()
    {
        foreach (var m in modules.Keys)
        {
            modules[m] = modules[m] with { state = Enumerable.Repeat(false, modules[m].state.Length).ToArray() }; //Reset
        }

        Dictionary<string, long> hitRx = [];
        Queue<(string, string, bool)> queue = [];
        long i = 1;
        while (true)
        {
            queue.Enqueue(("button", "", false));
            while (queue.Count > 0)
            {
                var q = queue.Dequeue();
                if (q.Item1 == "rx")
                {
                    if (q.Item3) continue;
                    return new($"{i}");
                }
                var b = UpdateAndNext(q.Item1, q.Item2, q.Item3);
                if (modules[q.Item1].output.Contains("rx") && q.Item3 && b.Item1)
                {
                    if (!hitRx.ContainsKey(q.Item2)) hitRx[q.Item2] = i;
                    else return new($"{hitRx.Values.Aggregate((long)1, LCM)}"); // Works for my input, general case is CRT
                }
                foreach (var o in modules[q.Item1].output)
                {
                    if (b.Item1)
                    {
                        queue.Enqueue((o, q.Item1, b.Item2));
                    }
                }
            }
            i++;
        }
    }
}
