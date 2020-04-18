namespace remikub.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using remikub.Domain;

    public interface IAutomaticPlayer
    {
        public void AutoPlay(Game game, string user);
    }

    public class AutomaticPlayer : IAutomaticPlayer
    {
        public void AutoPlay(Game game, string user)
        {
            // Start by playing hands only
            var flushes = ComputeFlushCombinaisons(game.UserHands[user]);
            if (flushes.Any())
            {
                game.Play(user, game.Board.Union(flushes).ToList(), game.UserHands[user].Except(flushes.SelectMany(x => x)).ToList());
            }

            var sets = ComputeSetCombinaisons(game.UserHands[user]);
            if (sets.Any())
            {
                game.Play(user, game.Board.Union(sets).ToList(), game.UserHands[user].Except(sets.SelectMany(x => x)).ToList());
            }

            var cardAdded = false;
            while (AddCardsToBoard(game, user))
            {
                cardAdded = true;
            }

            if (!flushes.Any() && !sets.Any() && !cardAdded)
            {
                game.DrawCard(user);
            }
        }

        private bool AddCardsToBoard(Game game, string user)
        {
            var playedCards = new List<Card>();
            var newBoard = game.Board.Select(x => x.Select(x => x).ToList()).ToList();
            foreach(var card in game.UserHands[user])
            {
                foreach(var combinaison in newBoard)
                {
                    var colors = combinaison.Select(x => x.Color).ToHashSet();
                    var values = combinaison.Select(x => x.Value).ToHashSet();
                    if (colors.Count == 1 && card.Color == colors.Single())
                    {
                        if(card.Value == values.Min() - 1)
                        {
                            playedCards.Add(card);
                            combinaison.Insert(0, card);
                        } else if(card.Value == values.Max() + 1)
                        {
                            playedCards.Add(card);
                            combinaison.Add(card);

                        }
                    } 
                    if(values.Count == 1 && card.Value == values.Single() && !colors.Contains(card.Color))
                    {
                        playedCards.Add(card);
                        combinaison.Add(card);
                    }
                }
            }

            var hasPlayed = playedCards.Any();
            if (hasPlayed) {
                game.Play(user, newBoard, game.UserHands[user].Except(playedCards).ToList());
            }
            return hasPlayed;

        }

        private List<List<Card>> ComputeFlushCombinaisons(List<Card> cards)
        {
            var cardsByColorAndValue = cards.GroupBy(x => x.Color)
                  .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Value)
                  .ToDictionary(x => x.Key, x => x.ToList()));

            var combinaisons = new List<List<Card>>();

            foreach (var (color, map) in cardsByColorAndValue)
            {
                var combinaison = new List<Card>();
                var currentValue = 0;
                foreach (var (value, duplicateCards) in map.OrderBy(x => x.Key))
                {
                    if (currentValue != 0 && value != currentValue + 1)
                    {
                        if (combinaison.Count >= 3)
                        {
                            combinaisons.Add(new List<Card>(combinaison));
                        }
                        combinaison.Clear();
                    }
                    else
                    {
                        combinaison.Add(duplicateCards.First());
                    }

                    currentValue = value;
                }
                if (combinaison.Count >= 3)
                {
                    combinaisons.Add(new List<Card>(combinaison));
                }
            }

            return combinaisons;
        }

        private List<List<Card>> ComputeSetCombinaisons(List<Card> cards)
        {
            var combinaisons = new List<List<Card>>();

            var cardsByValueAndColor = cards.GroupBy(x => x.Value)
                   .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Color)
                   .ToDictionary(x => x.Key, x => x));
            foreach (var (value, map) in cardsByValueAndColor)
            {
                if (map.Count >= 3)
                {
                    combinaisons.Add(map.Values.Select(x => x.First()).ToList());
                }
            }
            return combinaisons;
        }
    }
}
