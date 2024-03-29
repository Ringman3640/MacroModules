﻿using System.Drawing;

namespace MacroModules.Model.Values
{
    public class ColorValue : Value, IValueData<Color>
    {
        public override ValueDataType Type { get; protected set; } = ValueDataType.Color;

        public Color Data { get; set; } = Color.Empty;

        public ColorValue() { }

        public ColorValue(Color colorData)
        {
            Data = colorData;
        }

        public override Value Clone()
        {
            return new ColorValue(Data);
        }

        public override bool Equals(Value? other)
        {
            return base.Equals(other)
                && other is ColorValue otherColor
                && Data.Equals(otherColor.Data);
        }
    }
}
