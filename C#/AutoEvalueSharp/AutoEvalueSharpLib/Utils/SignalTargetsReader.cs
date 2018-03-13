using System;
using System.Collections.Generic;
using System.Xml;

namespace AutoEvalueSharpLib.Utils
{
    public sealed class SignalTargetReader
    {
        private XmlDocument _xmlDocument;
        private XmlNode _xmlNode;
        private bool _prepared;

        public SignalTargetReader(XmlDocument xmlDocument)
        {
            _xmlDocument = xmlDocument;
        }

        private bool IsFrameMark
        {
            get
            {
                var mark = _xmlNode.Name;
                return mark.Length == 22
                    && mark.StartsWith("Frame")
                    && mark.EndsWith("TargetNumber");
            }
        }
        
        private bool IsTargetMark
        {
            get
            {
                var mark = _xmlNode.Name;
                return mark.Length == 21 && mark.StartsWith(FrameName + "Target");
            }
        }

        private string _frameName;
        public string FrameName
        {
            get => _frameName;
        }

        private void Prepare()
        {
            _xmlNode = _xmlDocument.LastChild.FirstChild;

            if (!IsFrameMark)
            {
                throw new FormatException("First node is not frame number.");
            }

            _prepared = true;
        }

        public IList<SignalTarget> MoveAndGetTargets()
        {
            if (!_prepared)
            {
                Prepare();
            }
            else
            {
                _xmlNode = _xmlNode.NextSibling;
            }
            if (_xmlNode == null) return null;

            _frameName = _xmlNode.Name.Substring(0, 10);

            List<SignalTarget> targetList = new List<SignalTarget>();

            int targetsCount = int.Parse(_xmlNode.InnerText);
            for (int i = 0; i < targetsCount; i++)
            {
                _xmlNode = _xmlNode.NextSibling;

                if (!IsTargetMark) throw new FormatException("Didn't enough target in " + FrameName);

                targetList.Add(GetTarget());
            }

            return targetList;
        }

        private SignalTarget GetTarget()
        {
            var childNode = _xmlNode.FirstChild;
            if (!childNode.Name.Equals("Type")) throw new FormatException("Didn't fit type mark in " + FrameName);

            string type = childNode.InnerText;

            childNode = childNode.NextSibling;
            if (!childNode.Name.Equals("Position")) throw new FormatException("Didn't fit position mark in " + FrameName);

            var position = childNode.InnerText.Split(' ');
            int x = int.Parse(position[0]);
            int y = int.Parse(position[1]);
            int width = int.Parse(position[2]);
            int height = int.Parse(position[3]);

            return new SignalTarget(_xmlNode.Name.Substring(10), type, new Rect(x, y, width, height));
        }
    }
}
