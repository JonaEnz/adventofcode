namespace AdventOfCode;
using System.Text.RegularExpressions;

using Card = (int Id, int Intersect);

public partial class Day04 : BaseDay
{
    private readonly string _input;
    private readonly List<Card> cards;

    [GeneratedRegex(@"Card\s+(\d+):([\d\s]+)\|([\s\d]+)")]
    private static partial Regex ParseRegex();
    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
        cards = [];
        foreach (var line in _input.Split("\n").Where(x => x.Length > 0))
        {
            var m = ParseRegex().Match(line).Groups;
            var cardId = int.Parse(m[1].Value);
            var winning = m[2].Value.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);
            var have = m[3].Value.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);
            var c = (Id: cardId, Intersect: winning.Intersect(have).Count());
            cards.Add(c);
        }
    }

    public static int CardScore(Card card) => (int)Math.Pow(2, card.Intersect - 1);
    public override ValueTask<string> Solve_1()
    {
        return new($"{cards.Select(CardScore).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        var cardCount = new Dictionary<int, int>();
        foreach (var (Id, Score) in cards)
        {
            cardCount[Id] = 1;
        }
        foreach (var (Id, Intersect) in cards.Where(c => c.Intersect > 0))
        {
            foreach (var i in Enumerable.Range(Id + 1, Intersect))
            {
                cardCount[i] += cardCount[Id];
            }
        }
        return new($"{cardCount.Select(x => x.Value).Sum()}");
    }

}
