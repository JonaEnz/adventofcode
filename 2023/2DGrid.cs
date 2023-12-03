namespace AdventOfCode;

public class Grid2D<T>()
{
    public List<List<T>> Grid { get; set; } = new List<List<T>>();
    public T Default { get; set; } = default;
    public static Grid2D<T> Parse(string input, string separator, Func<string, T> parseField, T defaultField = default)
    {
        var grid = new Grid2D<T>();

        foreach (var line in input.Split('\n'))
        {
            grid.Grid.Add((separator != "" ? line.Split(separator) : line.ToCharArray().Select(c => c.ToString())).Select(x => parseField(x)).ToList());
        }
        grid.Default = defaultField;
        return grid;
    }
    public T Get(int x, int y)
    {
        if (y >= Height || y < 0 || x >= Width || x < 0)
        {
            return Default;
        }

        return Grid[y][x];
    }
    public int Height => Grid[0].Count;
    public int Width => Grid.Count;
    public List<Tuple<int, int, T>> Neighbors(int x, int y)
    {
        var neighbors = new List<Tuple<int, int, T>>();
        for (int xOff = -1; xOff <= 1; xOff++)
        {
            for (int yOff = -1; yOff <= 1; yOff++)
            {
                if (xOff != yOff && x + xOff >= 0 && y + xOff >= 0 && x + xOff < Width && y + yOff < Height)
                {
                    neighbors.Add(Tuple.Create(x, y, Get(x, y)));
                }
            }
        }
        return neighbors;
    }
    public void Transpose()
    {
        var w = Width;
        var h = Height;
        for (int x = 0; x < w; x++)
        {
            for (int y = x; y < h; y++)
            {
                var tmp = Get(x, y);
                Grid[x][y] = Get(y, x);
                Grid[y][x] = tmp;
            }
        }
    }
}
