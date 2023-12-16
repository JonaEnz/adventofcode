namespace AdventOfCode;

using Beam = (int x, int y, int direction);
public class Day16 : BaseDay
{
    private readonly string _input;
    private char[][] field;

    public Day16()
    {
        _input = File.ReadAllText(InputFilePath);
        field = _input.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select(l => l.ToCharArray()).ToArray();
    }

    public List<Beam> MoveOne(Beam beam)
    {
        (int, int)[] MIRROR = [(1, 2), (2, 1), (3, 4), (4, 3)];
        (int, int)[] MIRROR2 = [(1, 4), (2, 3), (3, 2), (4, 1)];
        var next = beam.direction switch
        {
            1 => (beam.x + 1, beam.y), // Right
            2 => (beam.x, beam.y + 1), // Down
            3 => (beam.x - 1, beam.y), // Left
            4 => (beam.x, beam.y - 1), // Up
            _ => (-1, -1),
        };
        if (next.Item1 < 0 || next.Item2 < 0 || next.Item1 >= field[0].Length || next.Item2 >= field.Length)
        {
            return [];
        }
        List<Beam> b = (field[next.Item2][next.Item1], beam.direction % 2) switch
        {
            ('\\', _) => [(next.Item1, next.Item2, MIRROR.First(x => x.Item1 == beam.direction).Item2)],
            ('/', _) => [(next.Item1, next.Item2, MIRROR2.First(x => x.Item1 == beam.direction).Item2)],
            ('|', 1) => [(next.Item1, next.Item2, 2), (next.Item1, next.Item2, 4)],
            ('-', 0) => [(next.Item1, next.Item2, 1), (next.Item1, next.Item2, 3)],
            _ => [(next.Item1, next.Item2, beam.direction)]
        };
        b = b.Where(next => !(next.Item1 < 0 || next.Item2 < 0 || next.Item1 >= field[0].Length || next.Item2 > field.Length)).ToList();
        return b;
    }

    public int RunBeam(Beam startBeam)
    {
        Queue<Beam> beams = [];
        HashSet<Beam> seen = [];
        beams.Enqueue(startBeam);
        while (beams.Count > 0)
        {
            foreach (var b in MoveOne(beams.Dequeue()))
            {
                if (!seen.Contains(b))
                {
                    beams.Enqueue(b);
                    seen.Add(b);
                }
            }
        }
        return seen.Select(b => (b.x, b.y)).Distinct().Count();
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{RunBeam((-1, 0, 1))}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<int> res = [];
        foreach (var x in Enumerable.Range(0, field[0].Length))
        {
            res.Add(RunBeam((x, -1, 2)));
            res.Add(RunBeam((x, field.Length, 4)));
        }
        foreach (var y in Enumerable.Range(0, field.Length))
        {
            res.Add(RunBeam((-1, y, 1)));
            res.Add(RunBeam((field[0].Length, y, 3)));
        }
        return new($"{res.Max()}");
    }
}
