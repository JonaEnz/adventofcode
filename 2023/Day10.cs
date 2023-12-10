namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string _input;
    private List<List<char>> field;
    private Dictionary<(int, int), int> dist = new Dictionary<(int, int), int>();

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath);
        field = _input.Split("\n").Select(l => l.ToCharArray().ToList()).ToList();
    }

    public (int, int) SPos()
    {
        for (int x = 0; x < field.Count; x++)
        {
            for (int y = 0; y < field[0].Count; y++)
            {
                if (field[x][y] == 'S')
                {
                    return (x, y);
                }
            }
        }
        return (-1, -1);
    }
    public List<List<char>> GetBlock(char c) => c switch
    {
        '-' => [[' ', ' ', ' '], ['-', '-', '-'], [' ', ' ', ' ']],
        '|' => [[' ', '|', ' '], [' ', '|', ' '], [' ', '|', ' ']],
        'F' => [[' ', ' ', ' '], [' ', 'F', '-'], [' ', '|', ' ']],
        'J' => [[' ', '|', ' '], ['-', 'J', ' '], [' ', ' ', ' ']],
        '7' => [[' ', ' ', ' '], ['-', '7', ' '], [' ', '|', ' ']],
        'L' => [[' ', '|', ' '], [' ', 'L', '-'], [' ', ' ', ' ']],
        'S' => [[' ', '|', ' '], ['-', '+', '-'], [' ', '|', ' ']],
        _ => [[' ', ' ', ' '], [' ', '*', ' '], [' ', ' ', ' ']],
    };


    public List<List<char>> ExpandPart2()
    {
        var exp = new List<List<char>>();
        for (int x = 0; x < field.Count; x++)
        {
            var a = field[x].Select(GetBlock);
            var l1 = a.SelectMany(b => b[0]).ToList();
            var l2 = a.SelectMany(b => b[1]).ToList();
            var l3 = a.SelectMany(b => b[2]).ToList();
            exp.Add(l1);
            exp.Add(l2);
            exp.Add(l3);
        }
        return exp;
    }

    public IEnumerable<(int, int)> Connected(int x, int y)
    {
        IEnumerable<(int, int)> l = field[x][y] switch
        {
            '|' => [(x - 1, y), (x + 1, y)],
            '-' => [(x, y - 1), (x, y + 1)],
            'F' => [(x, y + 1), (x + 1, y)],
            '7' => [(x, y - 1), (x + 1, y)],
            'L' => [(x - 1, y), (x, y + 1)],
            'J' => [(x - 1, y), (x, y - 1)],
            'S' => [(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)],
            _ => []
        };
        return l.Where(t => t.Item1 >= 0 && t.Item2 >= 0 && t.Item1 < field.Count && t.Item2 < field[0].Count);
    }

    public override ValueTask<string> Solve_1()
    {
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue(SPos());
        dist[queue.Peek()] = 1;
        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            var newDist = dist[(x, y)] + 1;
            var n = Connected(x, y).Where(p => !dist.ContainsKey((p.Item1, p.Item2)));
            if (n.Count() == 0 && newDist == 2)
            {
                dist.Remove((x, y));
            }
            foreach (var p in n)
            {
                queue.Enqueue(p);
                dist[p] = newDist;
            }
        }
        return new($"{dist.Count / 2}");
    }

    public override ValueTask<string> Solve_2()
    {
        for (int x = 0; x < field.Count; x++)
        {
            for (int y = 0; y < field[0].Count; y++)
            {
                if (!dist.ContainsKey((x, y)))
                {
                    field[x][y] = '.';
                }
            }
        }
        var ext = ExpandPart2();
        var queue = new Queue<(int, int)>();
        var visited = new HashSet<(int, int)>();
        queue.Enqueue((0, 0));
        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            if (visited.Contains((x, y)))
            {
                continue;
            }
            visited.Add((x, y));
            List<(int, int)> l = [(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)];
            foreach (var n in l.Where(p => p.Item1 >= 0 && p.Item2 >= 0 && p.Item1 < ext.Count && p.Item2 < ext[0].Count))
            {
                switch (ext[n.Item1][n.Item2])
                {
                    case ' ':
                        queue.Enqueue(n);
                        break;
                    case '*':
                        ext[n.Item1][n.Item2] = 'I';
                        break;
                    default:
                        break;
                }
            }
        }

        return new($"{ext.SelectMany(x => x).Count(x => x == '*')}");
    }
}
