namespace AdventOfCode;

public class Day3Field
{
    public List<List<char>> Field { get; set; }

    public Day3Field()
    {
        Field = new List<List<char>>();
    }

    public char Get(int x, int y)
    {
        if (y >= Field.Count || y < 0 || x >= Field[y].Count || x < 0)
            return '.';
        return Field[x][y];
    }
    private bool isPart(char c) => !char.IsDigit(c) && c != '.';
    public int Height => Field.Count;
    public int Width => Field[0].Count;
    public List<Tuple<int, int, int>> GearHack = new List<Tuple<int, int, int>>(); // x,y,digit
    private int getDigit(int x, int yStart, int yEnd)
    {
        var digit = "";
        for (int y = yStart; y <= yEnd; y++)
        {
            digit += Get(x, y).ToString();
        }
        return Int32.Parse(digit);
    }
    public int CheckDigit(int x, int yStart, int yEnd)
    {
        var res = 0;
        for (int y = yStart - 1; y <= yEnd + 1; y++)
        {
            if (isPart(Get(x - 1, y)) || isPart(Get(x + 1, y)))
            {
                if (Get(x - 1, y) == '*')
                {
                    GearHack.Add(new Tuple<int, int, int>(x - 1, y, getDigit(x, yStart, yEnd)));
                }
                if (Get(x + 1, y) == '*')
                {
                    GearHack.Add(new Tuple<int, int, int>(x + 1, y, getDigit(x, yStart, yEnd)));
                }
                res = getDigit(x, yStart, yEnd);
            }
        }
        if (isPart(Get(x, yStart - 1)) || isPart(Get(x, yEnd + 1)))
        {
            if (Get(x, yStart - 1) == '*')
            {
                GearHack.Add(new Tuple<int, int, int>(x, yStart - 1, getDigit(x, yStart, yEnd)));
            }
            if (Get(x, yEnd + 1) == '*')
            {
                GearHack.Add(new Tuple<int, int, int>(x, yEnd + 1, getDigit(x, yStart, yEnd)));
            }
            res = getDigit(x, yStart, yEnd);
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
        field = new Day3Field();
        foreach (var line in _input.Split("\n"))
        {
            field.Field.Add(line.ToCharArray().ToList());
        }
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
