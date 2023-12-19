namespace AdventOfCode;

using System.Text.RegularExpressions;
using MatchRule = (char part, int check, string next);
using Part = (int x, int m, int a, int s);
using Range = (int xl, int xu, int ml, int mu, int al, int au, int sl, int su);

public class Day19 : BaseDay
{
    private readonly string _input;
    Dictionary<string, List<MatchRule>> allRules = [];
    List<Part> parts = [];

    public Day19()
    {
        _input = File.ReadAllText(InputFilePath);
        var reg = new Regex(@"(\S+)\{(\S+[\<\>]\d+:\S+,?)+\,(\S+)\}");
        var innerReg = new Regex(@"(\S+)([\<\>])(\d+):(\S+),?");
        foreach (var step in _input.Split("\n\n")[0].Split("\n"))
        {
            var g = reg.Match(step).Groups.Values.ToList();
            List<MatchRule> rules = [];
            foreach (var s in g[2].Value.Split(","))
            {
                var g2 = innerReg.Match(s).Groups.Values.ToList();
                rules.Add((g2[1].Value[0], int.Parse(g2[3].Value) * (g2[2].Value[0] == '<' ? -1 : 1), g2[4].Value));
            }
            rules.Add((' ', 0, g[3].Value));
            allRules.Add(g[1].Value, rules);
        }

        foreach (var part in _input.Split("\n\n")[1].Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            Part newPart = (0, 0, 0, 0);
            var p = part.TrimStart('{').TrimEnd('}').Split(",");
            foreach (var partLine in p)
            {
                var y = partLine.Split("=");
                newPart = y[0] switch
                {
                    "a" => newPart with { a = int.Parse(y[1]) },
                    "x" => newPart with { x = int.Parse(y[1]) },
                    "m" => newPart with { m = int.Parse(y[1]) },
                    "s" => newPart with { s = int.Parse(y[1]) },
                    _ => newPart,
                };
            }
            parts.Add(newPart);
        }
    }

    public string NextVal(List<MatchRule> rules, Part part)
    {
        foreach (var rule in rules)
        {
            var isMatch = rule.part switch
            {
                'a' => rule.check > 0 ? rule.check < part.a : rule.check * -1 > part.a,
                'x' => rule.check > 0 ? rule.check < part.x : rule.check * -1 > part.x,
                'm' => rule.check > 0 ? rule.check < part.m : rule.check * -1 > part.m,
                's' => rule.check > 0 ? rule.check < part.s : rule.check * -1 > part.s,
                ' ' => true,
                _ => false,
            };
            if (isMatch)
            {
                return rule.next;
            }
        }
        throw new Exception("Should not happen");
    }

    public (List<Range>, string) SplitWithRule(MatchRule rule, Range range)
    {
        if (rule.part == ' ')
        {
            return ([range], rule.next);
        }
        var r = rule.part switch
        {
            ' ' => range,
            'a' => rule.check > 0 ? range with { al = rule.check + 1 } : range with { au = -1 * rule.check - 1 },
            'x' => rule.check > 0 ? range with { xl = rule.check + 1 } : range with { xu = -1 * rule.check - 1 },
            'm' => rule.check > 0 ? range with { ml = rule.check + 1 } : range with { mu = -1 * rule.check - 1 },
            's' => rule.check > 0 ? range with { sl = rule.check + 1 } : range with { su = -1 * rule.check - 1 },
            _ => range with { al = 2, au = 1 }
        };
        var r2 = rule.part switch
        {
            'a' => rule.check > 0 ? range with { au = rule.check } : range with { al = -1 * rule.check },
            'x' => rule.check > 0 ? range with { xu = rule.check } : range with { xl = -1 * rule.check },
            'm' => rule.check > 0 ? range with { mu = rule.check } : range with { ml = -1 * rule.check },
            's' => rule.check > 0 ? range with { su = rule.check } : range with { sl = -1 * rule.check },
            _ => range with { al = 2, au = 1 }
        };
        if (r.al > r.au || r.xl > r.xu || r.ml > r.mu || r.sl > r.su)
        {
            return ([r2], "");
        }
        if (r2.al > r2.au || r2.xl > r2.xu || r2.ml > r2.mu || r2.sl > r2.su)
        {
            return ([r], rule.next);
        }
        return ([r, r2], rule.next);
    }



    public override ValueTask<string> Solve_1()
    {
        var res = 0;
        foreach (var part in parts)
        {
            var next = "in";
            while (next != "A" && next != "R")
            {
                var r = allRules[next];
                next = NextVal(allRules[next], part);
            }
            if (next == "A")
            {
                res += part.x + part.a + part.m + part.s;
            }
        }
        return new($"{res}");
    }

    public long Score(Range range)
    {
        long n = range.xu - range.xl + 1;
        n *= range.mu - range.ml + 1;
        n *= range.au - range.al + 1;
        n *= range.su - range.sl + 1;
        return n;
    }

    public override ValueTask<string> Solve_2()
    {
        Queue<(Range, string)> queue = [];
        long res = 0;
        queue.Enqueue(((1, 4000, 1, 4000, 1, 4000, 1, 4000), "in"));
        while (queue.Count > 0)
        {
            var e = queue.Dequeue();
            Console.WriteLine($"{e.Item2}->{e.Item1}");
            if (e.Item2 == "R") continue;
            if (e.Item2 == "A")
            {
                res += Score(e.Item1);
                continue;
            }
            foreach (var rule in allRules[e.Item2])
            {
                var s = SplitWithRule(rule, e.Item1);
                if (s.Item1.Count >= 1)
                {
                    queue.Enqueue((s.Item1.First(), s.Item2));
                }
                if (s.Item1.Count == 2)
                {
                    e.Item1 = s.Item1[1];
                }
            }
        }
        return new($"{res}");
    }
}
