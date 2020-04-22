namespace remikub.test.Services.Smart
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FluentAssertions;
    using NUnit.Framework;
    using remikub.Domain;
    using remikub.Services.SmartPlayer;
    using static remikub.Services.SmartPlayer.SmartPlayer;

    public class SmartPlayerTest
    {
        [Test]
        public void GetAllValidCombinations_Should_Returns_Sets()
        {
            var cards = SmartPlayer.GetAllValidCombinations(new List<Card>
            {
                new Card(1, CardColor.Black),
                new Card(1, CardColor.Black),
                new Card(1, CardColor.Blue),
                new Card(1, CardColor.Red),
                new Card(1, CardColor.Orange),
                new Card(2, CardColor.Orange),
                new Card(2, CardColor.Blue),
                new Card(2, CardColor.Black),
                new Card(3, CardColor.Blue),
                new Card(4, CardColor.Blue),
                new Card(4, CardColor.Orange),
            });
            cards.Should().BeEquivalentTo(new List<List<CardValue>>
            {
                new List<CardValue> { new CardValue(1, CardColor.Black), new CardValue(1, CardColor.Blue), new CardValue(1, CardColor.Red), new CardValue(1, CardColor.Orange) },
                new List<CardValue> { new CardValue(1, CardColor.Blue), new CardValue(1, CardColor.Red), new CardValue(1, CardColor.Orange) },
                new List<CardValue> { new CardValue(1, CardColor.Black), new CardValue(1, CardColor.Red), new CardValue(1, CardColor.Orange) },
                new List<CardValue> { new CardValue(1, CardColor.Black), new CardValue(1, CardColor.Blue), new CardValue(1, CardColor.Orange) },
                new List<CardValue> { new CardValue(1, CardColor.Black), new CardValue(1, CardColor.Blue), new CardValue(1, CardColor.Red) },

                new List<CardValue> { new CardValue(2, CardColor.Orange) , new CardValue(2, CardColor.Blue) , new CardValue(2, CardColor.Black) }
            });
        }

        [Test]
        public void GetAllValidCombinations_Should_Returns_Flushes()
        {
            var cards = SmartPlayer.GetAllValidCombinations(new List<Card>
            {
                new Card(1, CardColor.Black),
                new Card(1, CardColor.Red),
                new Card(2, CardColor.Orange),
                new Card(2, CardColor.Black),
                new Card(3, CardColor.Black),
                new Card(4, CardColor.Blue),
                new Card(4, CardColor.Black),
            });
            cards.Should().BeEquivalentTo(new List<List<CardValue>>
            {
                new List<CardValue> { new CardValue(1, CardColor.Black), new CardValue(2, CardColor.Black), new CardValue(3, CardColor.Black), new CardValue(4, CardColor.Black) },
                new List<CardValue> { new CardValue(2, CardColor.Black), new CardValue(3, CardColor.Black), new CardValue(4, CardColor.Black) },
                new List<CardValue> { new CardValue(1, CardColor.Black), new CardValue(2, CardColor.Black), new CardValue(3, CardColor.Black) },
            });
        }
    }
}
