namespace AdventOfCode;

using HandScore = (string Hand, int Score);
public class Day07 : BaseDay
{
    private readonly string _input;
    private List<HandScore> hands;

    public Day07()
    {
        _input = File.ReadAllText(InputFilePath);
        hands = new List<HandScore>();
        foreach (var line in _input.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            var s = line.Split(" ");
            hands.Add((s[0], int.Parse(s[1])));
        }
    }

    public enum Hand
    {
        FiveOfKind = 7,
        FourOfKind = 6,
        FullHouse = 5,
        ThreeOfKind = 4,
        TwoPair = 3,
        OnePair = 2,
        None = 0
    }

    public static int CharStrength(char c, int joker) => c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => joker,
        'T' => 10,
        char ch => int.Parse(ch.ToString()),
    };

    public static int SecondOrder(string hand, int joker) => hand.Aggregate(0, (acc, c) => 15 * acc + CharStrength(c, joker));

    public static Hand CheckHand(string hand)
    {
        var sw = hand.ToCharArray().GroupBy(x => x).Select(x => x.Count()).OrderDescending().ToList();
        return sw switch
        {
        [5, ..] => Hand.FiveOfKind,
        [4, ..] => Hand.FourOfKind,
        [3, 2, ..] => Hand.FullHouse,
        [3, ..] => Hand.ThreeOfKind,
        [2, 2, ..] => Hand.TwoPair,
        [2, ..] => Hand.OnePair,
            _ => Hand.None
        };
    }

    public static Hand CheckHandPart2(string hand)
    {
        if (hand == "JJJJJ")
        {
            return Hand.FiveOfKind;
        }
        var groups = hand.ToCharArray().GroupBy(x => x).Where(x => x.Key != 'J');
        var maxCount = groups.Select(x => x.Count()).Max();
        var maxKind = groups.Where(x => x.Count() == maxCount).MaxBy(x => CharStrength(x.Key, 1)).Key;
        var hand2 = hand.Replace('J', maxKind);
        return CheckHand(hand2);
    }

    public override ValueTask<string> Solve_1()
    {
        var ranking = hands.
            GroupBy(x => CheckHand(x.Hand)).
            OrderBy(x => x.Key).
            Select(g => g.OrderBy(h => SecondOrder(h.Hand, 11))).
            SelectMany(x => x);
        return new($"{ranking.Select((h, i) => h.Score * (i + 1)).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        var ranking = hands.
            GroupBy(x => CheckHandPart2(x.Hand)).
            OrderBy(x => x.Key).
            Select(g => g.OrderBy(h => SecondOrder(h.Hand, 1))).
            SelectMany(x => x);
        return new($"{ranking.Select((h, i) => h.Score * (i + 1)).Sum()}");
    }
}
