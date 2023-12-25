namespace AdventOfCode;

public class Day25 : BaseDay
{
  private readonly string _input;
  private Dictionary<string, List<string>> components = [];

  public Day25()
  {
    _input = File.ReadAllText(InputFilePath); // !!!Input prefiltered
    foreach (var line in _input.Split("\n").Where(x => x.Length > 0))
    {
      var s = line.Split(": ");
      components[s.First()] = s.Last().Split(" ").ToList();
    }
    foreach (var c in components.Keys.ToList())
    {
      foreach (var v in components[c])
      {
        if (!components.ContainsKey(v)) components[v] = [];
        if (!components[v].Contains(c)) components[v].Add(c);
      }
    }
    // foreach (var c in components)
    // {
    //   Console.WriteLine($"{c.Key} -> {{{string.Join(",", c.Value)}}};");
    // }
  }

  public override ValueTask<string> Solve_1()
  {
    Queue<string> queue = [];
    HashSet<string> visited = [];
    queue.Enqueue(components.Keys.First());
    long i = 0;
    while (queue.Count > 0)
    {
      var q = queue.Dequeue();
      if (visited.Contains(q)) continue;
      foreach (var e in components[q])
      {
        if (!visited.Contains(e)) queue.Enqueue(e);
      }
      visited.Add(q);
      i++;
    }
    Console.WriteLine(components.Count);
    Console.WriteLine(i);
    return new($"{(long)(i + 3) * (long)(components.Count - i)}");
  }

  public override ValueTask<string> Solve_2()
  {
    return new("");
  }
}
