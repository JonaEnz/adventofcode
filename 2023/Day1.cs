namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split('\n');
        var numberLines = lines.Select(x => string.Concat(x.Where(c => System.Char.IsDigit(c))));
        return new($"{numberLines
            .Where(x => x.Count() > 0)
            .Select(l => Int32.Parse("" + l.First() + l.Last()))
            .Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        var numDict = new Dictionary<string, string>()
        {
            ["one"] = "1",
            ["two"] = "2",
            ["three"] = "3",
            ["four"] = "4",
            ["five"] = "5",
            ["six"] = "6",
            ["seven"] = "7",
            ["eight"] = "8",
            ["nine"] = "9",
        };
        var lines = _input.Split('\n');
        var newLines = new List<string>();
        foreach (var l in lines)
        {
            var nl = l;
            foreach (var c in numDict)
            {
                nl = nl.Replace(c.Key, c.Key + c.Value + c.Key);
            }
            newLines.Add(nl);
        }

        var numberLines = newLines.Select(x => string.Concat(x.Where(c => System.Char.IsDigit(c))));
        return new($"{numberLines
            .Where(x => x.Count() > 0)
            .Select(l => Int32.Parse("" + l.First() + l.Last()))
            .Sum()}");

    }
}
