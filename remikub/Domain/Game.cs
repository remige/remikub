namespace remikub.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Game
    {
        public const int NbCardByColor = 13;

        public Game(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            AvailableCards = new List<Card>();
            for (int number = 1; number <= NbCardByColor; number++)
            {
                foreach (var color in Enum.GetValues(typeof(CardColor)))
                {
                    AvailableCards.Add(new Card(number, (CardColor)color!));
                    AvailableCards.Add(new Card(number, (CardColor)color!));
                }
            }
        }

        public Guid Id { get; }
        public string Name { get; }
        public string? CurrentUser { get; private set; }
        public string? Winner { get; private set; }
        public List<List<Card>> Board { get; private set; } = new List<List<Card>>();
        public IDictionary<string, List<Card>> UserHands { get; } = new Dictionary<string, List<Card>>();
        public List<string> Users = new List<string>(); 
        private List<Card> AvailableCards { get; }

        public void RegisterUser(string user, bool isBot = false)
        {
            Users.Add(user);
            if(isBot)
            {
                Bots.Add(user);
            }
            if (string.IsNullOrEmpty(CurrentUser))
            {
                CurrentUser = user;
            }
            UserHands[user] = new List<Card>();
            for (int i = 1; i <= 14; i++)
            {
                DrawCard(user, false);
            }
        }

        private HashSet<string> Bots = new HashSet<string>();
        public bool IsBot(string user) => Bots.Contains(user);

        public void DrawCard(string user, bool checkUserTurn = true)
        {
            Console.WriteLine("Draw card");
            if (!UserHands.ContainsKey(user))
            {
                throw new ArgumentException($"User {user} is unknown");
            }
            if (!AvailableCards.Any())
            {
                throw new ArgumentException($"No card available");
            }
            if (checkUserTurn && CurrentUser != user)
            {
                throw new ArgumentException($"Not {user} turn");
            }

            var card = AvailableCards[new Random().Next(0, AvailableCards.Count - 1)];
            UserHands[user].Add(card);
            AvailableCards.Remove(card);
        }

        public void ReorganizeHand(string user, List<Card> hand)
        {
            if (!UserHands.TryGetValue(user, out var actualHand))
            {
                throw new ArgumentException($"User {user} is unknown");
            }
            if (CurrentUser != user)
            {
                throw new ArgumentException($"Not {user} turn");
            }
            if (!IsEquivalent(actualHand, hand))
            {
                throw new RemikubException(RemikubExceptionCode.HandsAreDifferent);
            }
            UserHands[user] = hand;
        }

        public void Play(string user, List<List<Card>> board, List<Card> hand)
        {
            Console.WriteLine("Play");
            if (!UserHands.TryGetValue(user, out var actualHand))
            {
                throw new ArgumentException($"User {user} is unknown");
            }
            if (CurrentUser != user)
            {
                throw new ArgumentException($"Not {user} turn");
            }

            if (hand.Count == actualHand.Count)
            {
                throw new RemikubException(RemikubExceptionCode.PlayOrDraw);
            }

            if (!IsEquivalent(hand.Union(board.SelectMany(x => x)), actualHand.Union(Board.SelectMany(x => x))))
            {
                throw new RemikubException(RemikubExceptionCode.InvalidCardAddedOrRemoved);
            }

            var invalidCombinations = board.Where(combination => !combination.IsValidCombination())
                                            .Select((_, combinationId) => combinationId.ToString())
                                            .ToArray();
            if (invalidCombinations.Any())
            {
                throw new RemikubException(RemikubExceptionCode.InvalidCombination, invalidCombinations);
            }

            Console.WriteLine($"Update board => {board.Count} combinations");
            Board = board;
            UserHands[user] = hand;
        }

        public void EndTurn()
        {
            // Should check if has played
            if (!UserHands[CurrentUser!].Any())
            {
                Winner = CurrentUser;
            }
            else
            {
                CurrentUser = Users[(Users.IndexOf(CurrentUser!) + 1) % Users.Count];
            }
        }

        private bool IsEquivalent(IEnumerable<Card> cardsSource, IEnumerable<Card> cardsTarget)
        {
            if (cardsSource.Count() != cardsTarget.Count())
            {
                return false;
            }

            var countBySourceCard = cardsSource.GroupBy(x => new { x.Color, x.Value }).ToDictionary(x => x.Key, x => x.Count());
            var countByTargetCard = cardsSource.GroupBy(x => new { x.Color, x.Value }).ToDictionary(x => x.Key, x => x.Count());

            foreach (var (key, sourceCount) in countBySourceCard)
            {
                if (!countByTargetCard.TryGetValue(key, out var targetCount) || targetCount != sourceCount)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
