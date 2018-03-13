using AutoEvalueSharpLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoEvalueSharpLib.Evaluators
{
    public sealed class SignalEvaluator
    {
        public IList<SignalFrameResult> Evalue(string detectionPath, string markPath)
        {
            var detectionXml = XmlDocProvider.CreateXmlDocument(detectionPath);
            var markXml = XmlDocProvider.CreateXmlDocument(markPath);

            var detectionReader = new SignalTargetReader(detectionXml);
            var markReader = new SignalTargetReader(markXml);

            IList<SignalTarget> detectionTargets = detectionReader.MoveAndGetTargets();
            IList<SignalTarget> markTargets = markReader.MoveAndGetTargets();
            List<SignalFrameResult> frameResults = new List<SignalFrameResult>();
            while (detectionTargets != null || markTargets != null)
            {
                if (detectionTargets == null || markTargets == null)
                {
                    throw new FormatException("Two files are not consistent.");
                }

                if (detectionReader.FrameName != markReader.FrameName)
                {
                    throw new FormatException($"One xml is {detectionReader.FrameName}, the other is {markReader.FrameName}");
                }

                frameResults.Add(CompareTargets(markReader.FrameName.Substring(5), detectionTargets, markTargets));

                detectionTargets = detectionReader.MoveAndGetTargets();
                markTargets = markReader.MoveAndGetTargets();
            }

            return frameResults;
        }

        private SignalFrameResult CompareTargets(string frameID, IList<SignalTarget> detectionTargets, IList<SignalTarget> markTargets)
        {
            int tpCount = 0;
            List<SignalTargetResult> results = new List<SignalTargetResult>();
            for (int i = 0; i < markTargets.Count; i++)
            {
                for (int j = 0; j < detectionTargets.Count; j++)
                {
                    var markTarget = markTargets[i];
                    var detectionTarget = detectionTargets[j];

                    if (markTarget != null && detectionTarget != null
                        && markTarget.Type == detectionTarget.Type
                        && CompareRects(detectionTarget.Position, markTarget.Position))
                    {
                        results.Add(new SignalTargetResult(Judge.TP, detectionTarget.Type, detectionTarget.ID, markTarget.ID));
                        tpCount++;
                        markTargets[i] = null;
                        detectionTargets[j] = null;
                        break;
                    }
                }
            }

            foreach (var target in markTargets)
            {
                if (target != null)
                {
                    results.Add(new SignalTargetResult(Judge.FN, null, null, target.ID));
                }
            }
            foreach (var target in detectionTargets)
            {
                if (target != null)
                {
                    results.Add(new SignalTargetResult(Judge.FP, target.Type, target.ID, null));
                }
            }

            return new SignalFrameResult(frameID, markTargets.Count, detectionTargets.Count, tpCount, results);
        }

        private bool CompareRects(Rect detection, Rect mark)
        {
            var centerX = detection.X + detection.Width / 2;
            var centerY = detection.Y + detection.Height / 2;
            return centerX > mark.X && centerY > mark.Y
                && centerX < detection.X + detection.Width && centerY < detection.Y + detection.Height;
        }
    }
}
