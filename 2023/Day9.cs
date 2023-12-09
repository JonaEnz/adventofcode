namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly List<List<List<int>>> _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").Select(l => l.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToList()).Select(BuildSequences).ToList();
    }

    public int Project(List<List<int>> list, int lineIndex)
    {
        return list[lineIndex].All(x => x == 0) ? 0 : Project(list, lineIndex + 1) + list[lineIndex].Last();
    }

    public int Project2(List<List<int>> list, int lineIndex)
    {
        return list[lineIndex].All(x => x == 0) ? 0 : list[lineIndex].First() - Project2(list, lineIndex + 1);
    }

    public List<List<int>> BuildSequences(List<int> init)
    {
        var ret = new List<List<int>>();
        ret.Add(init);
        while (ret.Last().Any(x => x != 0))
        {
            ret.Add(ret.Last().Zip(ret.Last().Skip(1)).Select(x => x.Second - x.First).ToList());
        }
        return ret;
    }

    public override ValueTask<string> Solve_1()
    {
        var a = _input.Select(x => Project(x, 0)).Sum();
        return new($"{a}");
    }

    public override ValueTask<string> Solve_2()
    {
        var a = _input.Select(x => Project2(x, 0)).Sum();
        return new($"{a}");
    }
}
