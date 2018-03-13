using System;
using System.IO;
using System.Xml;

namespace AutoEvalueSharpLib
{
    public sealed class XmlDocProvider
    {
        /// <summary>
        /// 根据提供的 XML 文件返回 XmlDocument，XML 文件的 encoding 为 zh_CN.UTF-8
        /// </summary>
        /// <param name="filepath">提供的文件路径</param>
        /// <returns></returns>
        public static XmlDocument CreateXmlDocument(string filepath)
        {
            string xmlString = null;
            using (Stream stream = File.OpenRead(filepath))
            using (StreamReader reader = new StreamReader(stream))
            {
                var firstLine = reader.ReadLine();
                xmlString = firstLine.Remove(firstLine.IndexOf('z'), 6) + "\r\n" + reader.ReadToEnd();
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            return xmlDocument;
        }
    }
}
