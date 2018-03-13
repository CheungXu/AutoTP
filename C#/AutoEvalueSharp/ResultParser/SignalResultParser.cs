using AutoEvalueSharpLib.Evaluators;
using AutoEvalueSharpLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEvalueSharp.Parsers
{
    public sealed class ResultParser
    {
        public string DetectionDirectory { get; private set; }
        public string MarkDirectory { get; private set; }
        public string OutputDirectory { get; private set; }

        public ResultParser(string detectionDirectory, string markDirectory, string outputDirectory)
        {
            DetectionDirectory = detectionDirectory;
            MarkDirectory = markDirectory;
            OutputDirectory = outputDirectory;
        }

        public void Run()
        {
            var result = ParseFiles(DetectionDirectory, MarkDirectory);

            void CreateSignalResultDelegate()
            {
                var signalResultDir = Path.Combine(OutputDirectory, "帧图像交通信号判定明细");
                Directory.CreateDirectory(signalResultDir);
                CreateSignalResult(signalResultDir, result);
            }

            Task[] tasks = new Task[]
            {
                Task.Run(() => CreateReport(Path.Combine(OutputDirectory, "测试报告.txt"), result)),
                Task.Run(() => CreateFrameResult(Path.Combine(OutputDirectory, "帧图像结果明细.txt"), result)),
                Task.Run(() => CreateSignalResultDelegate()),
                Task.Run(() => CreateSignalCategoriesResult(Path.Combine(OutputDirectory, "交通信号分类统计.txt"), result))
            };

            Task.WaitAll(tasks);
        }

        private void CreateReport(string path, IList<Tuple<string, IList<SignalFrameResult>>> input)
        {
            using (Stream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                var result = input.AsParallel().Select(tuple =>
                {
                    long trueCount = 0;
                    long detectCount = 0;
                    long tpCount = 0;
                    foreach (var frame in tuple.Item2)
                    {
                        trueCount += frame.TrueCount;
                        detectCount += frame.DetectCount;
                        tpCount += frame.TPCount;
                    }
                    return new { ID = tuple.Item1, TrueCount = trueCount, DetectionCount = detectCount, TPCount = tpCount };
                });

                writer.WriteLine($"{"数据名", -13}  标注真值  检测交通信号数目  正检数TP  误检数FP  漏检数FN  Precision  Recall  F-measure");
                writer.WriteLine();

                long trueCountTotal = 0;
                long detectionCountTotal = 0;
                long tpCountTotal = 0;
                foreach (var line in result)
                {
                    trueCountTotal += line.TrueCount;
                    detectionCountTotal += line.DetectionCount;
                    tpCountTotal += line.TPCount;

                    var fpCount = line.DetectionCount - line.TPCount;
                    var fnCount = line.TrueCount - line.TPCount;
                    var precision = (double)line.TPCount / line.DetectionCount;
                    var recall = (double)line.TPCount / line.TrueCount;
                    var f_measure = 0.0;
                    if (precision + recall != 0.0)
                    {
                        f_measure = 2 * precision * recall / (precision + recall);
                    }
                    writer.WriteLine($"{line.ID}  {line.TrueCount, 8}  {line.DetectionCount, 16}  {line.TPCount, 8}  {fpCount, 8}  {fnCount, 8}  {precision, 9}  {recall, 6}  {f_measure, 9}");
                }

                long fpCountTotal = detectionCountTotal - tpCountTotal;
                long fnCountTotal = trueCountTotal - tpCountTotal;
                double precisionTotal = (double)tpCountTotal / detectionCountTotal;
                double recallTotal = (double)tpCountTotal / trueCountTotal;
                double f_measureTotal = 0.0;
                if (precisionTotal + recallTotal != 0.0)
                {
                    f_measureTotal = 2 * precisionTotal * recallTotal / (precisionTotal + recallTotal);
                }
                writer.WriteLine($"{"总计", -14}  {trueCountTotal, 8}  {detectionCountTotal, 16}  {tpCountTotal, 8}  {fpCountTotal, 8}  {fnCountTotal, 8}  {precisionTotal, 9}  {recallTotal, 6}  {f_measureTotal, 9}");
            }
        }

        private void CreateFrameResult(string path, IList<Tuple<string, IList<SignalFrameResult>>> input)
        {
            using (Stream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{"帧图像名",-18}  标注真值  检测交通信号数目  正检数TP  误检数FP  漏检数FN  Precision  Recall  F-measure");

                foreach (var tuple in input)
                {
                    foreach (var line in tuple.Item2)
                    {
                        var frameName = tuple.Item1 + "-" + line.ID;
                        writer.WriteLine($"{frameName}  {line.TrueCount, 8}  {line.DetectCount, 16}  {line.TPCount, 8}  {line.FPCount, 8}  {line.FNCount, 8}  {line.Precision, 9}  {line.Recall, 6}  {line.F_Measure, 9}");
                    }
                }
            }
        }

        private void CreateSignalResult(string dirPath, IList<Tuple<string, IList<SignalFrameResult>>> input)
        {
            Parallel.ForEach(input, tuple =>
            {
                var subDirPath = Path.Combine(dirPath, tuple.Item1);
                Directory.CreateDirectory(subDirPath);

                Parallel.ForEach(tuple.Item2, frame =>
                {
                    var frameName = $"{tuple.Item1}-{frame.ID}";
                    var txtPath = Path.Combine(subDirPath, frameName + ".txt");
                    using (Stream stream = File.Open(txtPath, FileMode.OpenOrCreate, FileAccess.Write))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine($"{"帧图像名",-9}  {frameName}");
                        writer.WriteLine($"{"标注真值",-9}  {"检测交通信号",16}  判定");

                        foreach (var target in frame.TargetResults.Where(target => target.Judge == Judge.TP))
                        {
                            writer.WriteLine($"{target.TrueTarget}    {target.DetectTarget,22}  正检");
                        }

                        foreach (var target in frame.TargetResults.Where(target => target.Judge == Judge.FN))
                        {
                            writer.WriteLine($"{target.TrueTarget}    {"",22}  漏检");
                        }

                        foreach (var target in frame.TargetResults.Where(target => target.Judge == Judge.FP))
                        {
                            writer.WriteLine($"{"",11}    {target.DetectTarget,22}  误检");
                        }
                    }
                });
            });
        }

        private void CreateSignalCategoriesResult(string path, IList<Tuple<string, IList<SignalFrameResult>>> input)
        {
            long warningDetectCount = 0;
            long directDetectCount = 0;
            long banDetectCount = 0;
            long roadDetectCount = 0;
            long lightDetectCount = 0;

            long warningTPCount = 0;
            long directTPCount = 0;
            long banTPCount = 0;
            long roadTPCount = 0;
            long lightTPCount = 0;

            foreach (var tuple in input)
            {
                foreach (var frame in tuple.Item2)
                {
                    foreach (var target in frame.TargetResults)
                    {
                        if (target.Judge != Judge.FN)
                        {
                            switch (target.DetectType[1])
                            {
                                case '警':
                                    ++warningDetectCount;
                                    if (target.Judge == Judge.TP) ++warningTPCount;
                                    break;
                                case '示':
                                    ++directDetectCount;
                                    if (target.Judge == Judge.TP) ++directTPCount;
                                    break;
                                case '禁':
                                    ++banDetectCount;
                                    if (target.Judge == Judge.TP) ++banTPCount;
                                    break;
                                case '路':
                                    ++roadDetectCount;
                                    if (target.Judge == Judge.TP) ++roadTPCount;
                                    break;
                                default:
                                    if (target.DetectType.EndsWith("灯\""))
                                    {
                                        ++lightDetectCount;
                                        if (target.Judge == Judge.TP) ++lightTPCount;
                                    }
                                    else
                                    {
                                        throw new FormatException($"检测到不符合格式的标志：{target.DetectTarget}");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            using (Stream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                double warningPercent = 0.0;
                double directPercent = 0.0;
                double banPercent = 0.0;
                double roadPercent = 0.0;
                double lightPercent = 0.0;

                if (warningDetectCount != 0) warningPercent = (double)warningTPCount / warningDetectCount;
                if (directDetectCount != 0) directPercent = (double)directTPCount / directDetectCount;
                if (banDetectCount != 0) banPercent = (double)banTPCount / banDetectCount;
                if (roadDetectCount != 0) roadPercent = (double)roadTPCount / roadDetectCount;
                if (lightDetectCount != 0) lightPercent = (double)lightTPCount / lightDetectCount;

                writer.WriteLine($"{"类别",-8}  标注总数  正检总数  识别率");
                writer.WriteLine($"{"警告标志",-8}{warningDetectCount,8}  {warningTPCount,8}  {warningPercent,6}");
                writer.WriteLine($"{"指示标志",-8}{directDetectCount,8}  {directTPCount,8}  {directPercent,6}");
                writer.WriteLine($"{"禁令标志",-8}{banDetectCount,8}  {banTPCount,8}  {banPercent,6}");
                writer.WriteLine($"{"指路标志",-8}{roadDetectCount,8}  {roadTPCount,8}  {roadPercent,6}");
                writer.WriteLine($"交通信号灯  {lightDetectCount,8}  {lightTPCount,8}  {lightPercent,6}");
            }
        }

        private IList<SignalFrameResult> ParseFile(string detectionPath, string markPath)
        {
            var evaluator = new SignalEvaluator();
            return evaluator.Evalue(detectionPath, markPath);
        }

        private IList<Tuple<string, IList<SignalFrameResult>>> ParseFiles(string detectionDirectory, string markDirectory)
        {
            var detectionFiles = Directory.GetFiles(detectionDirectory);
            var markFiles = Directory.GetFiles(markDirectory);

            Array.Sort(detectionFiles);
            Array.Sort(markFiles);

            var result = new List<Tuple<string, IList<SignalFrameResult>>>();
            Parallel.For(0, markFiles.Length, idx =>
            {
                var targetID = Path.GetFileName(detectionFiles[idx]).Substring(0, 16);
                result.Add(new Tuple<string, IList<SignalFrameResult>>(targetID, ParseFile(detectionFiles[idx], markFiles[idx])));
            });

            result.Sort((left, right) => string.Compare(left.Item1, right.Item1));

            return result;
        }
    }
}
