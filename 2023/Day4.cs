namespace AdventOfCode;
using System.Text.RegularExpressions;

using Card = (int Id, List<int> Winning, List<int> Have);

public class Day04 : BaseDay
{
    private readonly string _input;
    private readonly List<Card> cards;
    private Dictionary<int, Int64> wonCache = new();

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
        cards = new();
        var reg = new Regex(@"Card\s+(\d+):([\d\s]+)\|([\s\d]+)");
        foreach (var line in _input.Split("\n").Where(x => x.Count() > 0))
        {
            var m = reg.Match(line).Groups;
            var cardId = int.Parse(m[1].Value);
            var winning = m[2].Value.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
            var have = m[3].Value.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
            var c = (Id: cardId, Winning: winning, Have: have);
            cards.Add(c);
        }
    }

    public int cardScore(Card card)
    {
        return (int)Math.Pow(2, card.Item2.Intersect(card.Item3).Count() - 1);
    }

    public override ValueTask<string> Solve_1()
    {
        var score = cards.Select(cardScore).Sum();
        return new($"{score}");
    }

    public override ValueTask<string> Solve_2()
    {
        var cardCount = new Dictionary<int, int>();
        foreach (var c in cards)
        {
            cardCount[c.Id] = 1;
        }
        foreach (var c in cards)
        {
            var matches = c.Have.Intersect(c.Winning).Count();
            if (matches > 0)
            {
                foreach (var i in Enumerable.Range(c.Id + 1, matches).Where(x => x <= cards.Count))
                {
                    cardCount[i] += cardCount[c.Id];
                }
            }
        }
        return new($"{cardCount.Select(x => x.Value).Sum()}");
    }
}
