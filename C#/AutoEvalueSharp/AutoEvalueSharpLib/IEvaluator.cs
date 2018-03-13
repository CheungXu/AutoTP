using System;
using System.Collections.Generic;
using System.Text;

namespace AutoEvalueSharpLib
{
    public interface IEvaluator
    {
        Tuple<double, double> Evalue(string detectionPath, string markPath);
    }
}
