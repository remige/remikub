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

        [Test]
        public void IsSolvable()
        {
            SmartPlayer.IsSolvable(new List<Card>
            {
                new Card(1, CardColor.Black),
                new Card(1, CardColor.Red),
                new Card(2, CardColor.Orange),
                new Card(2, CardColor.Black),
                new Card(3, CardColor.Black),
                new Card(4, CardColor.Blue),
                new Card(4, CardColor.Black),
            }).Should().BeFalse();

            var list = new List<Card>();
            for(int i = 1; i <= 11; i++)
            {
                list.Add(new Card(i, CardColor.Red));
                list.Add(new Card(i, CardColor.Black));
            }
            SmartPlayer.IsSolvable(list).Should().BeTrue();

            list.Add(new Card(13, CardColor.Orange));
            SmartPlayer.IsSolvable(list).Should().BeFalse();
            list.Add(new Card(13, CardColor.Black));
            SmartPlayer.IsSolvable(list).Should().BeFalse();
            list.Add(new Card(13, CardColor.Red));
            SmartPlayer.IsSolvable(list).Should().BeTrue();


            list.Add(new Card(13, CardColor.Red));
            SmartPlayer.IsSolvable(list).Should().BeFalse();
        }


        [Test]
        public void Try()
        {
            SmartPlayer.Try(new List<Card>
            {
                new Card(1, CardColor.Black),
                new Card(1, CardColor.Red),
                new Card(2, CardColor.Orange),
                new Card(2, CardColor.Black),
                new Card(3, CardColor.Black),
                new Card(4, CardColor.Blue),
                new Card(4, CardColor.Black),
            }).Should().BeNull();
            
            var list = new List<Card>();
            for (int i = 1; i <= 11; i++)
            {
                list.Add(new Card(i, CardColor.Red));
                list.Add(new Card(i, CardColor.Black));
            }
            SmartPlayer.Try(list).Should().NotBeNull();

            list.Add(new Card(13, CardColor.Orange));
            SmartPlayer.Try(list).Should().BeNull();
            list.Add(new Card(13, CardColor.Black));
            SmartPlayer.Try(list).Should().BeNull();
            list.Add(new Card(13, CardColor.Red));
            SmartPlayer.Try(list).Should().NotBeNull();


            list.Add(new Card(13, CardColor.Red));
            SmartPlayer.Try(list).Should().BeNull();

        }
    }
}
