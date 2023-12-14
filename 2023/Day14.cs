namespace AdventOfCode;

public class Day14 : BaseDay
{
    private readonly string[] _input;
    private List<List<char>> field;

    public Day14()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        field = [];
        foreach (var line in _input)
        {
            field.Add(line.ToCharArray().ToList());
        }
    }
    public static long Hash(List<List<char>> f) => f.Count == 0 ? 1 : f[0].Aggregate((long)1, (acc, c) => 3 * acc + (c == 'O' ? 2 : (c == '#' ? 2 : 0))) + 2 * Hash(f[1..]);

    public List<List<char>> Cycle(List<List<char>> f) => Enumerable.Range(0, 4).Aggregate(f, (acc, _) => Rotate90(MoveNorth(acc)));
    public List<List<char>> MoveNorth(List<List<char>> f2)
    {
        var f = f2.Select(x => x.ToList()).ToList();
        for (int x = 0; x < f[0].Count; x++)
        {
            var open = 0;
            for (int y = 0; y < f.Count; y++)
            {
                switch (f[y][x])
                {
                    case 'O':
                        if (open >= 0 && open < y)
                        {
                            f[open][x] = 'O';
                            f[y][x] = '.';
                            open++;
                            y = open;
                        }
                        else
                        {
                            open++;
                        }
                        break;
                    case '#':
                        open = y + 1;
                        break;
                    default:
                        break;
                }
            }
        }
        return f;
    }

    public static List<List<char>> Rotate90(List<List<char>> l)
    {
        var result = new List<List<char>>();
        for (int i = 0; i < l[0].Count; i++)
        {
            result.Add(new List<char>());
            for (int j = l.Count - 1; j >= 0; j--)
            {
                result[i].Add(l[j][i]);
            }
        }
        return result;


    }
    public static int Weight(List<List<char>> f) => f.AsEnumerable().Reverse().Select((x, i) => (i + 1) * x.Count(c => c == 'O')).Sum();

    public override ValueTask<string> Solve_1()
    {
        return new($"{Weight(MoveNorth(field))}");
    }

    public override ValueTask<string> Solve_2()
    {
        field = Enumerable.Range(0, 100).Aggregate(field, (acc, _) => Cycle(acc));
        return new($"{Weight(field)}");
    }
}
