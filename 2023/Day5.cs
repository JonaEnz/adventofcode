namespace AdventOfCode;

using System.Security.AccessControl;
using Seed = (Int64 Start, Int64 End);

class RangeMap
{
    public Int64 Destination { get; set; }
    public Int64 MapStart { get; set; }
    public Int64 MapEnd { get; set; }
    public static RangeMap Parse(string line)
    {
        var s = line.Split(" ");
        return new RangeMap(Int64.Parse(s[0]), Int64.Parse(s[1]), Int64.Parse(s[2]));
    }
    public RangeMap(Int64 d, Int64 s, Int64 l)
    {
        Destination = d;
        MapStart = s;
        MapEnd = l + s - 1;
    }
    public List<Seed> Cut(Seed s)
    {
        if (s.End < MapStart || s.Start > MapEnd)
        {
            return [];
        }
        var seeds = new List<Seed>();
        if (s.Start < MapStart)
        {
            seeds.Add((s.Start, MapStart - 1));
        }
        if (s.End > MapEnd)
        {
            seeds.Add((MapEnd + 1, s.End));
        }
        seeds.Add((Math.Max(s.Start, MapStart), Math.Min(s.End, MapEnd)));
        // Remove any seeds that are empty
        return seeds;
    }
}

public class Day05 : BaseDay
{
    private readonly string _input;
    private IEnumerable<IEnumerable<RangeMap>> groups;

    private List<Seed> mapWithRm(IEnumerable<RangeMap> maps, IEnumerable<Seed> seeds)
    {
        var cutSeeds = new List<Seed>();
        foreach (var s in seeds)
        {
            var found = false;
            foreach (var map in maps)
            {
                var c = map.Cut(s);
                if (c.Count > 0)
                {
                    cutSeeds.AddRange(c);
                    found = true;
                }
            }
            if (!found)
            {
                cutSeeds.Add(s);
            }
        }
        var newSeeds = new List<Seed>();
        foreach (var s in cutSeeds)
        {
            var found = false;
            foreach (var map in maps)
            {
                if (s.Start >= map.MapStart && s.End <= map.MapEnd)
                {
                    newSeeds.Add((map.Destination + s.Start - map.MapStart, map.Destination + s.End - map.MapStart));
                    found = true;
                }
            }
            if (!found)
            {
                newSeeds.Add(s);
            }
        }
        return newSeeds;
    }

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath);
        groups = _input.Split("\n\n")[1..].Select(x => x.Split("\n")[1..]).Select(x => x.Select(RangeMap.Parse));
    }

    public override ValueTask<string> Solve_1()
    {
        var seeds = _input.Split("\n").First().Split(": ")[1].Split(" ").Select(x => (Start: long.Parse(x), End: (long)long.Parse(x)));

        foreach (var g in groups)
        {
            seeds = mapWithRm(g, seeds);
        }
        return new($"{seeds.Select(x => x.Start).Min()}");
    }

    public override ValueTask<string> Solve_2()
    {
        var a = _input.Split("\n").First().Split(": ")[1].Split(" ");
        var seeds = new List<Seed>();
        for (var i = 0; i < a.Length - 1; i += 2)
        {
            var s = Int64.Parse(a[i]);
            seeds.Add((Start: s, End: s - 1 + Int64.Parse(a[i + 1])));
        }

        foreach (var g in groups)
        {
            seeds = mapWithRm(g, seeds);
        }
        return new($"{seeds.Select(x => x.Start).Min()}");
    }
}
