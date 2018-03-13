using System;
using System.Collections.Generic;
using System.Text;

namespace AutoEvalueSharpLib.Utils
{
    public sealed class SignalFrameResult
    {
        public int TrueCount { get; private set; }
        public int DetectCount { get; private set; }
        public int TPCount { get; private set; }
        public int FPCount { get => DetectCount - TPCount; }
        public int FNCount { get => TrueCount - TPCount; }
        public double Precision { get => (double)TPCount / DetectCount; }
        public double Recall { get => (double)TPCount / TrueCount; }
        public double F_Measure
        {
            get
            {
                var precision = Precision;
                var recall = Recall;
                return 2 * precision * recall / (precision + recall);
            }
        }

        public string ID { get; private set; }
        public List<SignalTargetResult> TargetResults { get; private set; }

        public SignalFrameResult(string id, int trueCount, int detectCount, int tpCount, List<SignalTargetResult> targetResults)
        {
            ID = id;
            TrueCount = trueCount;
            DetectCount = detectCount;
            TPCount = tpCount;
            TargetResults = targetResults;
        }
    }
}
