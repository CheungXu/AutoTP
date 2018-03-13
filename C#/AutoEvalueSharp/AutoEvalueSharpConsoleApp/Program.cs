using AutoEvalueSharp.Parsers;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static System.Console;

namespace AutoEvalueSharpConsoleApp
{
    class Program
    {
        private const string FileFormat = @"TSD-Signal-\d{5}-Result\.xml";
        private const string DirectoryFormat = @"TSD-Signal-Result-.*\b";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsages();
                return;
            }

            var result = ParseArgs(args);
            if (result == null) return;

            if (!CheckArgs(result)) return;

            //ResultParser parser = new ResultParser(@"E:\Test\TSD-Signal-Result-Name", @"E:\Test\TSD-Signal", @"E:\Test");
            ResultParser parser = new ResultParser(result[0], result[1], result[2]);
            parser.Run();

            WriteLine("完成。");
        }

        private static void ShowUsages()
        {
            Write(new string('-', 10));
            Write(" AutoEvalueSharp Usages ");
            WriteLine(new string('-', 10));
            WriteLine();

            WriteLine("语法：");
            WriteLine("\tAutoEvalueSharp  路径一 路径二 输出文件路径");
            WriteLine();
            WriteLine("\t路径一为标注路径，路径二为真值路径。输出文件路径为文件夹。");
            WriteLine();
        }

        private static List<string> ParseArgs(string[] args)
        {
            List<string> result = new List<string>();
            result.AddRange(args);

            if (result.Count != 3) WriteLine("参数个数错误，无法解析。");

            if (!Directory.Exists(result[0]) || !Directory.Exists(result[1]))
            {
                WriteLine("输入路径错误。");
                return null;
            }

            Directory.CreateDirectory(result[2]);

            return result;
        }

        private static bool CheckArgs(List<string> result)
        {
            if (!Regex.IsMatch(result[0], DirectoryFormat)) return false;

            var detectionFiles = Directory.GetFiles(result[0]);

            if (detectionFiles.Length != Directory.GetFiles(result[1]).Length) return false;

            Regex regex = new Regex(FileFormat);
            foreach (var file in detectionFiles)
            {
                if (!regex.IsMatch(Path.GetFileName(file))) return false;
            }

            return true;
        }
    }
}
