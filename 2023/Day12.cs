namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly List<List<string>> _input;
    private Dictionary<(string, string, long), long> cache;

    public Day12()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select(s => s.Split(" ").ToList()).ToList();

    }

    public string Unfold(string fold, int n, char c) => string.Join(c, Enumerable.Repeat(fold, n));
    public long Solve(string pattern, string sp, int foldRepeat)
    {
        pattern = Unfold(pattern, foldRepeat, '?');
        var springs = Unfold(sp, foldRepeat, ',').Split(",").Select(int.Parse).ToList();
        return Count(pattern + ".", springs);
    }

    public long Count(string pattern, List<int> springs, long counter = 0)
    {
        var key = (pattern, string.Join(".", springs), counter);
        if (cache.ContainsKey(key))
        {
            return cache[key];
        }
        if (pattern.Length == 0)
        {
            return (springs.Count == 0 && counter == 0) ? 1 : 0;
        }
        long res = 0;
        List<char> checks = pattern[0] == '?' ? ['.', '#'] : [pattern[0]];
        foreach (var c in checks)
        {
            if (c == '#')
            {
                res += Count(pattern.Substring(1), springs, counter + 1);
            }
            else
            {
                if (counter > 0)
                {
                    if (springs.Count > 0 && springs[0] == counter)
                    {
                        res += Count(pattern.Substring(1), springs.Skip(1).ToList());
                    }
                }
                else
                {
                    res += Count(pattern.Substring(1), springs);

                }
            }
        }
        cache[key] = res;
        return res;
    }

    public override ValueTask<string> Solve_1()
    {
        long res = 0;
        cache = new Dictionary<(string, string, long), long>();
        foreach (var line in _input)
        {
            var springs = line[1];
            var l = line[0];
            res += Solve(l, springs, 1);
        }
        return new($"{res}");
    }

    public override ValueTask<string> Solve_2()
    {
        long res = 0;
        cache = new Dictionary<(string, string, long), long>();
        foreach (var line in _input)
        {
            var springs = line[1];
            var l = line[0];
            res += Solve(l, springs, 5);
        }
        return new($"{res}");
    }
}
