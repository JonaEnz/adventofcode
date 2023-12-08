namespace AdventOfCode;
using System.Text.RegularExpressions;

public class Day08 : BaseDay
{
    private readonly string _input;
    private string directions;
    private Dictionary<string, Tree> dict;

    public class Tree
    {
        public string Node { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        public Tree(string l, string r, string n)
        {
            Left = l;
            Right = r;
            Node = n;
        }
    }

    public Day08()
    {
        var reg = new Regex(@"(.{3}) = \((.{3}), (.{3})\)");
        _input = File.ReadAllText(InputFilePath);
        dict = new Dictionary<string, Tree>();
        directions = _input.Split("\n").First();
        foreach (var line in _input.Split("\n")[2..].Where(s => !string.IsNullOrWhiteSpace(s)))
        {
            var match = reg.Match(line).Groups.Values.ToList();
            var name = match[1].Value;
            dict[name] = new Tree(match[2].Value, match[3].Value, name);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var init = dict["AAA"];
        var i = 0;
        while (init.Node != "ZZZ")
        {
            init = directions[i % directions.Length] == 'L' ? dict[init.Left] : dict[init.Right];
            i++;
        }
        return new($"{i}");
    }

    public static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    public static long LCM(long a, long b) => (a / GCD(a, b)) * b;
    public override ValueTask<string> Solve_2()
    {
        var inits = dict.Where(x => x.Key.EndsWith("A")).Select(x => x.Value);
        var res = new List<long>();
        foreach (var ini in inits)
        {
            var init = ini;
            var i = 0;
            while (!init.Node.EndsWith("Z"))
            {
                init = directions[i % directions.Length] == 'L' ? dict[init.Left] : dict[init.Right];
                i++;
            }
            res.Add((long)i);
        }
        return new($"{res.Aggregate(LCM)}");
    }
}
