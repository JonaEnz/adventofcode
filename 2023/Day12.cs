namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly List<List<string>> _input;
    private Dictionary<(int, string, long), long> cache;

    public Day12()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select(s => s.Split(" ").ToList()).ToList();
    }

    public static string Unfold(string fold, int n, char c) => string.Join(c, Enumerable.Repeat(fold, n));
    public long Solve(string pattern, string sp, int foldRepeat)
    {
        pattern = Unfold(pattern, foldRepeat, '?');
        var springs = Unfold(sp, foldRepeat, ',').Split(",").Select(int.Parse).ToList();
        cache = [];
        return Count(pattern + ".", springs);
    }

    public long Count(string pattern, List<int> springs, long counter = 0)
    {
        var key = (pattern.Length, string.Concat(springs), counter);
        if (cache.TryGetValue(key, out long value))
        {
            return value;
        }
        if (pattern.Length == 0)
        {
            return (springs.Count == 0 && counter == 0) ? 1 : 0;
        }
        long res = 0;
        List<char> checks = pattern[0] == '?' ? ['.', '#'] : [pattern[0]];
        foreach (var c in checks)
        {
            res += (c, counter > 0, springs.Count > 0 && springs[0] == counter) switch
            {
                ('#', _, _) => Count(pattern[1..], springs, counter + 1),
                (_, false, _) => Count(pattern[1..], springs),
                (_, _, true) => Count(pattern[1..], springs[1..]),
                _ => 0
            };
        }
        cache[key] = res;
        return res;
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{_input.Select(line => Solve(line[0], line[1], 1)).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new($"{_input.Select(line => Solve(line[0], line[1], 5)).Sum()}");
    }
}
