namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly string _input;
    private readonly List<(long, long)> galaxies;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
        galaxies = [];
        foreach (var line in _input.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select((x, i) => (x, i)))
        {
            for (int y = 0; y < line.x.Length; y++)
            {
                if (line.x[y] == '#')
                {
                    galaxies.Add(((long)line.i, (long)y));
                }
            }
        }
    }

    public long Solve(int expand)
    {
        var newGalaxies = new List<(long, long)>();
        var groupRows = galaxies.GroupBy(x => x.Item1);
        var i = 0;
        foreach (var row in Enumerable.Range(0, (int)galaxies.Select(x => x.Item1).Max() + 1))
        {
            if (!groupRows.Any(x => x.Key == row))
            {
                i += expand;
            }
            else
            {
                newGalaxies.AddRange(groupRows.First(x => x.Key == row).Select(g => (g.Item1 + i, g.Item2)));
            }
        }
        var galaxies2 = newGalaxies;
        newGalaxies = [];
        var groupCols = galaxies2.GroupBy(x => x.Item2);
        i = 0;

        foreach (var col in Enumerable.Range(0, (int)galaxies2.Select(x => x.Item2).Max() + 1))
        {
            if (!groupCols.Any(x => x.Key == col))
            {
                i += expand;
            }
            else
            {
                newGalaxies.AddRange(groupCols.First(x => x.Key == col).Select(g => (g.Item1, g.Item2 + i)));
            }

        }
        long res = 0;
        for (int x = 0; x < newGalaxies.Count - 1; x++)
        {

            for (int y = x + 1; y < newGalaxies.Count; y++)
            {
                res += Math.Abs(newGalaxies[x].Item2 - newGalaxies[y].Item2) + Math.Abs(newGalaxies[x].Item1 - newGalaxies[y].Item1);
            }
        }
        return res;

    }
    public override ValueTask<string> Solve_1()
    {
        return new($"{Solve(1)}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new($"{Solve(999999)}");
    }
}
