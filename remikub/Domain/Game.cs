namespace remikub.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Game
    {
        public Game(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            AvailableCards = new List<Card>();
            for (int number = 1; number <= 13; number++)
            {
                foreach (var color in Enum.GetValues(typeof(CardColor)))
                {
                    AvailableCards.Add(new Card(number, (CardColor)color!));
                    AvailableCards.Add(new Card(number, (CardColor)color));
                }
            }
        }

        public Guid Id { get; }
        public string Name { get; }
        public List<List<Card>> Board { get; } = new List<List<Card>>();
        public IDictionary<string, List<Card>> UserHands { get; } = new Dictionary<string, List<Card>>();
        private List<Card> AvailableCards { get; }

        public void RegisterUser(string user)
        {
            UserHands[user] = new List<Card>();
            for (int i = 1; i <= 14; i++)
            {
                DrawCard(user);
            }
        }

        public void DrawCard(string user)
        {
            if (!UserHands.ContainsKey(user))
            {
                throw new ArgumentException($"User {user} is unknown");
            }
            if (!AvailableCards.Any())
            {
                throw new ArgumentException($"No card available");
            }

            var card = AvailableCards[new Random().Next(0, AvailableCards.Count - 1)];
            UserHands[user].Add(card);
            AvailableCards.Remove(card);
        }
    }
}
