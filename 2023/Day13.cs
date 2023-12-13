namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly string _input;
    private List<List<List<bool>>> fields = [];
    private Dictionary<int, (int, int)> dict = [];

    public Day13()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (var f in _input.Split("\n\n"))
        {
            List<List<bool>> field = [];
            foreach (var line in f.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select(l => l.Trim()))
            {
                field.Add(line.Select(c => c == '#').ToList());
            }
            fields.Add(field);
        }

    }

    public static int LineNumber(List<bool> l) => l.Count == 0 ? 1 : 2 * LineNumber(l[1..]) + (l[0] ? 1 : 0);
    public static List<List<bool>> Transpose(List<List<bool>> l)
    {
        var res = new List<List<bool>>();
        for (int i = 0; i < l[0].Count; i++)
        {
            res.Add(l.Select(x => x[i]).ToList());
        }
        return res;
    }
    public static bool IsMirror(List<int> l) => l.Count switch
    {
        0 => true,
        1 => false,
        2 => l[0] == l[1],
        _ => l[0] == l[^1] && IsMirror(l[1..^1])
    };
    public int FieldValue(List<List<bool>> field, int nr)
    {
        var (l, r) = (-1, -1);
        if (dict.ContainsKey(nr))
        {
            (l, r) = dict[nr];
        }
        var lnr = field.Select(LineNumber).ToList();
        var left = Enumerable.Range(1, lnr.Count - 1).FirstOrDefault(x => x != l && IsMirror(lnr[x..]));
        var right = Enumerable.Range(1, lnr.Count - 1).FirstOrDefault(x => x != r && IsMirror(lnr[..^x]));
        if (!dict.ContainsKey(nr))
        {
            dict[nr] = (left, right);
        }
        if (left == 0)
        {
            left = lnr.Count;
        }
        if (right == 0)
        {
            right = lnr.Count;
        }
        if (left >= lnr.Count - 1 && right >= lnr.Count - 1)
        {
            return 0;
        }
        if (left <= right)
        {
            return (int)(left + Math.Floor((decimal)(lnr.Count - left) / 2));
        }
        else
        {
            return (int)(Math.Floor((decimal)((lnr.Count - right) / 2)));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        dict = [];
        var res = 0;
        int i = 0;
        foreach (var f in fields)
        {
            var r = FieldValue(f, i) * 100 + FieldValue(Transpose(f), -i);
            res += r;
            i++;
        }
        return new($"{res}");
    }

    public override ValueTask<string> Solve_2()
    {
        int i = 0;
        var res = 0;
        foreach (var f in fields)
        {
            for (int x = 0; x < f.Count; x++)
            {
                for (int y = 0; y < f[0].Count; y++)
                {
                    f[x][y] = !f[x][y];
                    var f1 = FieldValue(f, i);
                    var f2 = FieldValue(Transpose(f), -i);
                    var r = f1 * 100 + f2;
                    if (r != 0 && (f1 == 0 || f2 == 0))
                    {
                        res += r;
                        y = f[x].Count;
                        x = f.Count - 1;
                    }
                    else
                    {
                        f[x][y] = !f[x][y];
                    }
                }
            }
            i++;
        }
        return new($"{res}");

    }
}
