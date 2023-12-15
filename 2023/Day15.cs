namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly string[] _input;

    public Day15()
    {
        _input = File.ReadAllText(InputFilePath).Replace("\n", "").Split(",");
    }

    public int Hash(string s) => s.Aggregate(0, (acc, c) => ((((int)c) + acc) * 17) % 256);
    public override ValueTask<string> Solve_1()
    {
        return new($"{_input.Select(Hash).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, List<(string, int)>> boxes = [];
        foreach (var s in _input)
        {
            var label = s.Split(s.Contains("=") ? "=" : "-").First();
            var hash = Hash(label);
            var fl = s.Contains("=") ? int.Parse(s.Split("=").Last()) : 0;
            if (fl == 0)
            {
                if (boxes.ContainsKey(hash))
                {
                    boxes[hash].RemoveAll(x => x.Item1 == label);
                }
            }
            else
            {
                if (!boxes.ContainsKey(hash))
                {
                    boxes[hash] = [(label, fl)];
                }
                else
                {
                    if (boxes[hash].Any(x => x.Item1 == label))
                    {
                        boxes[hash][boxes[hash].FindIndex(x => x.Item1 == label)] = (label, fl);
                    }
                    else
                    {
                        boxes[hash].Add((label, fl));
                    }
                }
            }
        }
        return new($"{boxes.Select(x => (x.Key + 1) * (x.Value.Select((y, i) => (i + 1) * y.Item2).Sum())).Sum()}");
    }
}
