namespace AdventOfCode;

public class Day18 : BaseDay
{
    private readonly string _input;

    public Day18()
    {
        _input = File.ReadAllText(InputFilePath);
    }
    static double ShoelaceArea(List<(long, long)> v)
    {
        var n = Enumerable.Range(0, v.Count - 1).Select(i => v[i].Item1 * v[i + 1].Item2 - v[i + 1].Item1 * v[i].Item2).Sum();
        return Math.Abs(n + v[^1].Item1 * v[0].Item2 - v[0].Item1 * v[^1].Item2);
    }

    public override ValueTask<string> Solve_1()
    {
        List<(long, long)> points = [(0, 0)];
        var lenSum = 0;
        foreach (var line in _input.Split("\n"))
        {
            var s = line.Split(" ");
            if (s.Length < 3) continue;
            var n = int.Parse(s[1]);
            points.Add(
                s[0] switch
                {
                    "D" => (points[^1].Item1 - n, points[^1].Item2),
                    "R" => (points[^1].Item1, points[^1].Item2 + n),
                    "U" => (points[^1].Item1 + n, points[^1].Item2),
                    "L" => (points[^1].Item1, points[^1].Item2 - n),
                    _ => (-1, -1),
                }
            );
            lenSum += n;
        }
        return new($"{(ShoelaceArea(points) + lenSum) / 2.0 + 1}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<(long, long)> points = [(0, 0)];
        long lenSum = 0;
        foreach (var line in _input.Split("\n"))
        {
            var s = line.Split(" ");
            if (s.Length < 3) continue;
            var h = s[2].TrimStart('(').TrimEnd(')');
            var n = long.Parse(h[1..^1], System.Globalization.NumberStyles.HexNumber);
            points.Add(
                h[^1] switch
                {
                    '1' => (points[^1].Item1 - n, points[^1].Item2),
                    '0' => (points[^1].Item1, points[^1].Item2 + n),
                    '3' => (points[^1].Item1 + n, points[^1].Item2),
                    '2' => (points[^1].Item1, points[^1].Item2 - n),
                    _ => (-1, -1),
                }
            );
            lenSum += n;
        }
        return new($"{(ShoelaceArea(points) + lenSum) / 2.0 + 1}");
    }
}
