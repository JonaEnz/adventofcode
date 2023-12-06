namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string _input;
    private readonly List<(int First, int Second)> races;
    private readonly Tuple<long, long> part2race;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
        var lines = _input.Split("\n").Select(l => l.Split(": ").Last()).ToList();
        var times = lines[0].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);
        var distances = lines[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);
        races = times.Zip(distances).ToList();

        var time = long.Parse(string.Concat(lines[0].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x))));
        var dist = long.Parse(string.Concat(lines[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x))));
        part2race = Tuple.Create(time, dist);

    }

    public (long, long) GetSolutionRange(long t, long d)
    {
        //(t-x)*x > dist = (-x^2 + x*t - d > 0) => abc formula
        var a = Math.Sqrt(t * t - 4 * d);
        return ((long)Math.Ceiling((-t + a) / -2), (long)Math.Floor((-t - a) / -2));
    }

    public override ValueTask<string> Solve_1()
    {
        var result = races.Select(r => GetSolutionRange(r.First, r.Second)).Select(x => x.Item2 - x.Item1 + 1).Aggregate((a, b) => a * b);
        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (t, d) = part2race;
        var (x, y) = GetSolutionRange(t, d);
        return new($"{y - x + 1}");
    }
}
