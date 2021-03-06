﻿using System;

namespace remikub.Domain
{
    public class Card
    {
        public Card(int value, CardColor color)
        {
            Id = Guid.NewGuid();
            Value = value;
            Color = color;
        }
        public Guid Id { get;  }
        public int Value { get; }
        public CardColor Color { get; }
        public override string ToString() => $"{Value}_{Color}";
        public bool IsEquivalent(Card card) => Value == card.Value && Color == card.Color;
    }

    public enum CardColor
    {
        Red,
        Blue,
        Black,
        Orange
    }
}
