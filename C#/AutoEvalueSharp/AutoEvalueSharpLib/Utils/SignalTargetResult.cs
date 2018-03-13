using System;

namespace AutoEvalueSharpLib.Utils
{
    public struct SignalTargetResult
    {
        public SignalTargetResult(Judge judge, string detectType, string detectTarget, string trueTarget)
        {
            Judge = judge;
            DetectType = detectType;
            DetectTarget = detectTarget;
            TrueTarget = trueTarget;
        }

        public Judge Judge;
        public string DetectType;
        public String DetectTarget;
        public String TrueTarget;
    }

    public enum Judge
    {
        TP,
        FP,
        FN
    }
}
