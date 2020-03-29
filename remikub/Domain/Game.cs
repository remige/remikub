﻿namespace remikub.Domain
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
        public List<List<Card>> Board { get; private set; } = new List<List<Card>>();
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

        public void Play(string user, List<List<Card>> board, List<Card> hand)
        {
            // TODO : check this is the user turn

            if (!UserHands.TryGetValue(user, out var actualHand))
            {
                throw new ArgumentException($"User {user} is unknown");
            }

            if (hand.Count == actualHand.Count)
            {
                throw new RemikubException(RemikubExceptionCode.PlayOrDraw);
            }

            if (IsEquivalent(hand.Union(board.SelectMany(x => x)), actualHand.Union(Board.SelectMany(x => x))))
            {
                throw new RemikubException(RemikubExceptionCode.InvalidCardAddedOrRemoved);
            }

            var invalidCombinations = board.Where(combination => !IsValidCombination(combination))
                                            .Select((_, combinationId) => combinationId.ToString())
                                            .ToArray();
            if (invalidCombinations.Any())
            {
                throw new RemikubException(RemikubExceptionCode.InvalidCombination, invalidCombinations);
            }

            Board = board;
            UserHands[user] = hand;
        }

        private bool IsValidCombination(List<Card> combination)
        {
            if (combination.Count < 3)
            {
                return false;
            }

            return IsValidSet(combination) || IsValidFlush(combination);
        }

        private bool IsValidSet(List<Card> combination)
        {
            var colors = new HashSet<CardColor>();
            var values = new HashSet<int>();
            combination.ForEach(x =>
            {
                colors.Add(x.Color);
                values.Add(x.Value);
            });
            return colors.Count == combination.Count && values.Count == 1;
        }

        private bool IsValidFlush(List<Card> combination)
        {
            CardColor? currentColor = null;
            int? currentValue = null;
            foreach(var card in combination)
            {
                if(currentValue != null && currentValue + 1 != card.Value ||
                    currentColor != null && currentColor != card.Color)
                {
                    return false;
                }
                currentColor = card.Color;
                currentValue = card.Value;
            }
            return true;
        }

        private bool IsEquivalent(IEnumerable<Card> cardsSource, IEnumerable<Card> cardsTarget)
        {
            if (cardsSource.Count() != cardsTarget.Count() ||
                cardsSource.Any(source => cardsTarget.All(target => !target.IsEquivalent(source)))) {
                return false;
            }
            return true;
        }
    }
}