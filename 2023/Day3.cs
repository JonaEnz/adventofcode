namespace AdventOfCode;

public class Day3Field : Grid2D<char>
{
    public static Day3Field Parse(string input, string separator, Func<string, char> parseField)
    {
        var grid = new Day3Field();

        foreach (var line in input.Split('\n'))
        {
            grid.Grid.Add([.. line]);
        }
        grid.Default = '.';
        return grid;
    }
    public new char Get(int x, int y)
    {
        return base.Get(y, x);
    }
    private static bool IsPart(char c) => !char.IsDigit(c) && c != '.';
    public List<Tuple<int, int, int>> GearHack = []; // x,y,digit
    private int GetDigit(int x, int yStart, int yEnd)
    {
        var digit = "";
        for (int y = yStart; y <= yEnd; y++)
        {
            digit += Get(x, y).ToString();
        }
        return int.Parse(digit);
    }
    public int CheckDigit(int x, int yStart, int yEnd)
    {
        var res = 0;
        for (int y = yStart - 1; y <= yEnd + 1; y++)
        {
            if (IsPart(Get(x - 1, y)) || IsPart(Get(x + 1, y)))
            {
                if (Get(x - 1, y) == '*')
                {
                    GearHack.Add(new Tuple<int, int, int>(x - 1, y, GetDigit(x, yStart, yEnd)));
                }
                if (Get(x + 1, y) == '*')
                {
                    GearHack.Add(new Tuple<int, int, int>(x + 1, y, GetDigit(x, yStart, yEnd)));
                }
                res = GetDigit(x, yStart, yEnd);
            }
        }
        if (IsPart(Get(x, yStart - 1)) || IsPart(Get(x, yEnd + 1)))
        {
            if (Get(x, yStart - 1) == '*')
            {
                GearHack.Add(new Tuple<int, int, int>(x, yStart - 1, GetDigit(x, yStart, yEnd)));
            }
            if (Get(x, yEnd + 1) == '*')
            {
                GearHack.Add(new Tuple<int, int, int>(x, yEnd + 1, GetDigit(x, yStart, yEnd)));
            }
            res = GetDigit(x, yStart, yEnd);
        }
        return res;
    }
}

public class Day03 : BaseDay
{
    private readonly string _input;
    private Day3Field field;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
        field = Day3Field.Parse(_input, "", x => x[0]);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        for (int x = 0; x < field.Height; x++)
        {
            bool isDigit = true;
            int startY = 0;
            for (int y = 0; y <= field.Width; y++)
            {
                if (isDigit)
                {
                    if (!char.IsDigit(field.Get(x, y)) || y == field.Width)
                    {
                        //Found end of digit
                        var i = field.CheckDigit(x, startY, y - 1);
                        result += i;
                        isDigit = false;
                    }
                }
                else if (char.IsDigit(field.Get(x, y)))
                {
                    isDigit = true;
                    startY = y;
                }
            }
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var res = field.GearHack.GroupBy(x => x.Item1 + x.Item2 * 10000).Where(x => x.Count() == 2).Select(y => y.Aggregate(1, (acc, x) => acc * x.Item3)).Sum();
        return new($"{res}");
    }
}
