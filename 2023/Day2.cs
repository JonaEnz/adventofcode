namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    private List<List<ValueTuple<int, int, int>>> parseGames()
    {
        var games = new List<List<ValueTuple<int, int, int>>>();
        foreach (var line in _input.Split('\n'))
        {
            var d = new List<ValueTuple<int, int, int>>();
            foreach (var draw in line.Split(": ").Last().Split("; "))
            {
                var cubes = draw.Split(", ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Split()).ToList();
                var green = cubes.Where(x => x[1] == "green").Select(x => Int32.Parse(x[0])).FirstOrDefault();
                var red = cubes.Where(x => x[1] == "red").Select(x => Int32.Parse(x[0])).FirstOrDefault();
                var blue = cubes.Where(x => x[1] == "blue").Select(x => Int32.Parse(x[0])).FirstOrDefault();
                d.Add((green, red, blue));
            }
            games.Add(d);

        }
        return games;
    }


    public override ValueTask<string> Solve_1()
    {
        var g = parseGames();
        var id = 0;
        var res = 0;
        foreach (var game in g)
        {
            id++;
            res += (game.All(x => x.Item1 <= 13 && x.Item2 <= 12 && x.Item3 <= 14)) ? id : 0;
        }
        return new($"{res}");
    }

    public override ValueTask<string> Solve_2()
    {
        var g = parseGames();
        var res = 0;
        foreach (var game in g)
        {
            var minGreen = game.Select(x => x.Item1).Max();
            var minRed = game.Select(x => x.Item2).Max();
            var minBlue = game.Select(x => x.Item3).Max();
            var power = minGreen * minRed * minBlue;
            res += power;
        }
        return new($"{res}");
    }
}

