﻿namespace Nexo.Values
{
    public sealed class Number : Value
    {
        public double Value { get; init; }

        public Number(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
