namespace AdventOfCode;
using System.Text.RegularExpressions;

using Block = (int xl, int xu, int yl, int yu, int zl, int zu);
public class Day22 : BaseDay
{
    private readonly string _input;
    private List<Block> blocks = [];
    private List<Block> fallen = [];
    private Dictionary<Block, List<Block>> depends = [];
    private Dictionary<Block, int> above = [];
    List<List<int>> topDown = [];

    public Day22()
    {
        var reg = new Regex(@"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)");
        _input = File.ReadAllText(InputFilePath);
        foreach (var line in _input.Split("\n").Where(x => x.Length > 0))
        {
            var groups = reg.Match(line).Groups;
            var vals = groups.Values.Select(x => x.Value).Skip(1).Select(x => int.Parse(x)).ToArray();
            blocks.Add((vals[0], vals[3], vals[1], vals[4], vals[2], vals[5]));
        }
        blocks = blocks.OrderBy(x => x.zl).ToList();

        var row = Enumerable.Repeat(0, blocks.Select(x => x.yu + 1).Max()).ToList();
        foreach (var _ in Enumerable.Range(0, blocks.Select(x => x.xu + 1).Max())) topDown.Add(row.ToArray().ToList());
        fallen = blocks.Select(BlockFall).ToList();
        foreach (var f in fallen) depends[f] = DependsOn(f, fallen);
        foreach (var f in fallen) above[f] = Above(f).Count;
    }

    public int Height(Block block) => 1 + block.zu - block.zl;

    public List<(int x, int y)> BlockXYPos(Block b)
    {
        List<(int, int)> l = [];
        for (int x = b.xl; x <= b.xu; x++)
        {
            for (int y = b.yl; y <= b.yu; y++)
            {
                l.Add((x, y));
            }
        }
        return l;
    }
    public Block BlockFall(Block b)
    {
        var blockPos = BlockXYPos(b);
        var zStart = blockPos.Select(xy => topDown[xy.x][xy.y]).Max();
        foreach (var xy in BlockXYPos(b))
        {
            topDown[xy.x][xy.y] = zStart + Height(b);
        }
        return b with { zl = zStart + 1, zu = zStart + Height(b) };
    }

    public List<Block> Above(Block b)
    {
        var d = depends.Where(x => x.Value.Contains(b)).Select(x => x.Key).ToList();
        if (d.Count == 0) return [];
        var a = d.SelectMany(Above).ToList();
        a.AddRange(d);
        return a.Distinct().ToList();
    }

    public List<Block> DependsOn(Block b, List<Block> blocks)
    {
        return blocks
            .Where(x => x != b)
            .Where(o => BlockXYPos(o).Intersect(BlockXYPos(b)).Count() > 0)
            .Where(o => o.zu + 1 == b.zl)
            .ToList();
    }

    public int Falling(List<Block> falling)
    {
        var additional = depends
            .Where(x => x.Value.Count > 0 && x.Value.All(y => falling.Contains(y)))
            .Select(x => x.Key)
            .Where(x => !falling.Contains(x));
        if (additional.Count() == 0) return falling.Count - 1;
        falling.AddRange(additional);
        return Falling(falling);
    }

    public override ValueTask<string> Solve_1()
    {
        var res = fallen
            .Where(x => depends[x].Count == 1)
            .Select(x => depends[x].First())
            .Distinct().Count();
        return new($"{fallen.Count - res}");
    }

    public override ValueTask<string> Solve_2()
    {
        var res = fallen
            .Select(f => Falling([f]))
            .Sum();
        return new($"{res}");
    }
}
