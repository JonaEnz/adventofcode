namespace AdventOfCode;

using Point = (long x, long y);

public class Day21 : BaseDay
{
    private readonly string _input;
    private bool[][] field;
    private Point startPos;

    public Day21()
    {
        _input = File.ReadAllText(InputFilePath);
        field = _input.Split("\n").Where(x => x.Length > 0).Select(line => line.Select(x => x != '#').ToArray()).ToArray();
        var i = _input.Replace("\n", "").IndexOf('S');
        startPos = (i % field[0].Length, (int)(i / field[0].Length));

    }

    public List<Point> Neighbors(Point start)
    {
        List<Point> move = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        List<Point> next = move
            .Select(m => (m.x + start.x, m.y + start.y))
            .Where(p => p.Item1 >= 0 && p.Item1 < field[0].Length && p.Item2 >= 0 && p.Item2 < field.Length && field[p.Item2][p.Item1])
            .ToList();
        return next;
    }
    public List<Point> Neighbors2(Point start)
    {
        List<Point> move = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        List<Point> next = move
            .Select(m => (m.x + start.x, m.y + start.y))
            .Where(p => field[((p.Item2 % field.Length) + field.Length) % field.Length][((p.Item1 % field[0].Length) + field[0].Length) % field[0].Length])
            .ToList();
        return next;
    }
    public override ValueTask<string> Solve_1()
    {
        List<Point> possible = [startPos];
        foreach (var _ in Enumerable.Range(1, 64))
        {
            possible = possible.SelectMany(Neighbors).Distinct().ToList();
        }

        return new($"{possible.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<Point> possible = [startPos];
        int remainder = 26501365 % field.Length;
        long i = 1;
        int printed = 0;
        Console.Write("interpolating polynomial ");
        while (printed < 3)
        {
            possible = possible.SelectMany(Neighbors2).Distinct().ToList();
            if (i % field.Length == remainder)
            {
                Console.Write("{" + $"{i},{possible.Count}" + "},"); // Use wolfram alpha to interpolate the function and evaluate at 26501365
                printed++;
            }
            i++;
        }
        Console.WriteLine();
        return new("");
    }
}
