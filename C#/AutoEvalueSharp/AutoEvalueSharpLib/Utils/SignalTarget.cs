using System;
using System.Collections.Generic;
using System.Text;

namespace AutoEvalueSharpLib.Utils
{
    public sealed class SignalTarget
    {
        public String ID { get; }
        public string Type { get; }
        public Rect Position { get; }

        public SignalTarget(string id, string type, Rect position)
        {
            ID = id;
            Type = type;
            Position = position;
        }
    }
}
