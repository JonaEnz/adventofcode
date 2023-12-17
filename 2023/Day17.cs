namespace AdventOfCode;

using Path = (int x, int y, int direction, int walked);

public class Day17 : BaseDay
{
    private readonly string _input;
    private int[][] field;

    public Day17()
    {
        _input = File.ReadAllText(InputFilePath);
        field = _input.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    public List<Path> Next(Path path)
    {
        List<int> dx = [1, 0, -1, 0]; //R,D,L,U
        List<int> dy = [0, 1, 0, -1];
        List<int> dirs = [(path.direction + 1) % 4, (path.direction + 3) % 4];
        if (path.walked < 3) dirs.Add(path.direction);

        return dirs
            .Select(d => (path.x + dx[d], path.y + dy[d], d, d == path.direction ? path.walked + 1 : 1))
            .Where(p => p.Item1 >= 0 && p.Item2 >= 0 && p.Item1 < field[0].Length && p.Item2 < field.Length)
            .ToList();
    }

    public List<Path> Next2(Path path)
    {
        List<int> dx = [1, 0, -1, 0]; //R,D,L,U
        List<int> dy = [0, 1, 0, -1];
        List<int> dirs = [];
        if (path.walked >= 4) dirs.AddRange([(path.direction + 1) % 4, (path.direction + 3) % 4]);
        if (path.walked < 10) dirs.Add(path.direction);

        return dirs
            .Select(d => (path.x + dx[d], path.y + dy[d], d, d == path.direction ? path.walked + 1 : 1))
            .Where(p => p.Item1 >= 0 && p.Item2 >= 0 && p.Item1 < field[0].Length && p.Item2 < field.Length)
            .ToList();
    }

    public List<(Path, int)> Solve(Func<Path, List<Path>> next)
    {

        Dictionary<Path, int> minCost = [];
        PriorityQueue<Path, int> priority = new PriorityQueue<Path, int>();
        priority.Enqueue((0, 0, 0, 0), 0);
        minCost[(0, 0, 0, 0)] = 0;
        while (priority.Count > 0)
        {
            var last = priority.Dequeue();
            foreach (var p in next(last))
            {
                if (!minCost.ContainsKey(p))
                {
                    minCost[p] = int.MaxValue;
                }
                var newCost = minCost[last] + field[p.y][p.x];
                if (newCost < minCost[p])
                {
                    minCost[p] = newCost;
                    priority.Enqueue(p, newCost);
                }
            }

        }
        return minCost.Where(x => x.Key.Item1 == field[0].Length - 1 && x.Key.Item2 == field.Length - 1).Select(x => (x.Key, x.Value)).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{Solve(Next).Select(x => x.Item2).Min()}");
    }
    public override ValueTask<string> Solve_2()
    {
        return new($"{Solve(Next2).Where(x => x.Item1.walked >= 3).Select(x => x.Item2).Min()}");
    }
}
