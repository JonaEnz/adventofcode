namespace AdventOfCode;

public class Day23 : BaseDay
{
  private readonly string _input;
  private char[][] field;
  Dictionary<(int, int), List<(int, int, int)>> neighbors = [];

  public Day23()
  {
    _input = File.ReadAllText(InputFilePath);
    field = _input.Split("\n").Where(l => l.Length > 0).Select(line => line.ToCharArray()).ToArray();
    List<int> dx = [-1, 1, 0, 0];
    List<int> dy = [0, 0, -1, 1];
    for (int y = 0; y < field.Length; y++)
    {
      for (int x = 0; x < field[0].Length; x++)
      {
        List<(int, int)> candidate = field[y][x] switch
        {
          '#' => [],
          '.' => Enumerable.Range(0, 4).Select(i => (x + dx[i], y + dy[i])).ToList(),
          '>' => [(x + 1, y)],
          '<' => [(x - 1, y)],
          'v' => [(x, y + 1)],
          '^' => [(x, y - 1)],
          _ => [],
        };
        neighbors[(x, y)] = candidate
          .Where(c => c.Item1 >= 0 && c.Item2 >= 0
            && c.Item1 < field[0].Length && c.Item2 < field.Length && field[c.Item2][c.Item1] != '#')
          .Select(x => (x.Item1, x.Item2, 1))
          .ToList();
      }
    }
  }

  public void CompressNeighbors()
  {
    int i = 0;
    while (neighbors.Any(x => x.Value.Count == 2))
    {
      i++;
      var n = neighbors.First(x => x.Value.Count == 2);
      var left = n.Value[0];
      var right = n.Value[1];
      var lleft = neighbors[(left.Item1, left.Item2)];
      var lright = neighbors[(right.Item1, right.Item2)];
      var length = left.Item3 + right.Item3;
      lleft.RemoveAll(x => (x.Item1, x.Item2) == n.Key);
      lright.RemoveAll(x => (x.Item1, x.Item2) == n.Key);
      lleft.Add((right.Item1, right.Item2, length));
      lright.Add((left.Item1, left.Item2, length));
      neighbors[(left.Item1, left.Item2)] = lleft;
      neighbors[(right.Item1, right.Item2)] = lright;
      neighbors.Remove(n.Key);
    }
  }

  public int Solve()
  {
    var startX = Enumerable.Range(0, field[0].Length).First(i => field[0][i] == '.');
    var queue = new Queue<(List<(int, int)>, int)>();
    queue.Enqueue(([(startX, 0)], 0));
    var record = 1;
    while (queue.Count > 0)
    {
      var (l, steps) = queue.Dequeue();
      if (steps > record) record = steps;
      var next = neighbors[l.Last()].Where(x => !l.Contains((x.Item1, x.Item2)));
      foreach (var n in next) queue.Enqueue((l.Append((n.Item1, n.Item2)).ToList(), n.Item3 + steps));
    }
    return record;
  }

  public override ValueTask<string> Solve_1()
  {
    return new($"{Solve()}");
  }


  public override ValueTask<string> Solve_2()
  {
    List<int> dx = [-1, 1, 0, 0];
    List<int> dy = [0, 0, -1, 1];
    for (int y = 0; y < field.Length; y++)
    {
      for (int x = 0; x < field[0].Length; x++)
      {
        List<(int, int)> candidate = field[y][x] switch
        {
          '#' => [],
          _ => Enumerable.Range(0, 4).Select(i => (x + dx[i], y + dy[i])).ToList(),
        };
        candidate = candidate.Where(c => c.Item1 >= 0 && c.Item2 >= 0
            && c.Item1 < field[0].Length && c.Item2 < field.Length && field[c.Item2][c.Item1] != '#').ToList();
        neighbors[(x, y)] = candidate.Select(x => (x.Item1, x.Item2, 1)).ToList();
      }
    }
    CompressNeighbors();
    return new($"{Solve()}");
  }
}
