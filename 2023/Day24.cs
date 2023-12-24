namespace AdventOfCode;

using System.Text.RegularExpressions;
using Hailstone = (double x, double y, double z, double vx, double vy, double vz);

public class Day24 : BaseDay
{
  private readonly string _input;
  List<Hailstone> hailstones = [];

  public Day24()
  {
    var reg = new Regex(@"(\d+), (\d+), (\d+) @ ([-\d]+), ([-\d]+), ([-\d]+)");
    _input = File.ReadAllText(InputFilePath);
    foreach (var line in _input.Split("\n").Where(x => x.Length > 0))
    {
      var g = reg.Match(line).Groups.Values.Skip(1).Select(x => double.Parse(x.Value)).ToList();
      hailstones.Add((g[0], g[1], g[2], g[3], g[4], g[5]));
    }
  }

  public bool IntersectXY(Hailstone a, Hailstone b, long min, long max)
  {
    double div = a.vx * b.vy - a.vy * b.vx;
    double at = (((b.x - a.x) * b.vy) - ((b.y - a.y) * b.vx)) / div;
    double bt = (((b.x - a.x) * a.vy) - ((b.y - a.y) * a.vx)) / div;
    var ix = a.x + at * a.vx;
    var iy = a.y + at * a.vy;
    return at >= 0 && bt >= 0 && ix >= min && iy >= min && ix <= max && iy <= max;
  }

  public override ValueTask<string> Solve_1()
  {
    int res = 0;
    foreach (var i in Enumerable.Range(0, hailstones.Count))
    {
      for (int j = i + 1; j < hailstones.Count; j++)
      {
        if (IntersectXY(hailstones[i], hailstones[j], 200000000000000, 400000000000000)) res++;
      }
    }

    return new($"{res}");
  }

  public override ValueTask<string> Solve_2()
  {
    return new("");
  }
}
