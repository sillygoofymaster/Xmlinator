using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Reflection.Metadata.BlobBuilder;

using System.Reflection.PortableExecutable;

namespace XMLParsing
{
    internal class SAXParser : IParser
    {
        public IList<Book> Parse(string filePath)
        {
            Dictionary<string, (string, string)> fields = new Dictionary<string, (string, string)>();
            IList<Book> result = new List<Book>();
            var xmlReader = new XmlTextReader(filePath);
            string tempNodeName = "";
            string tempNodeValue = "";
            string tempAttribute = "";
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "libraryDatabase") || xmlReader.NodeType ==  XmlNodeType.ProcessingInstruction)
                {
                    continue;
                }
               
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "book")
                {
                    if (fields.Count > 0) result.Add(new Book(fields));
                    fields.Clear();
                }
                else if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    tempNodeName = xmlReader.Name;
                    if (xmlReader.HasAttributes)
                    {
                        xmlReader.MoveToNextAttribute();
                        tempAttribute = xmlReader.Value;
                    }
                }

                else if (xmlReader.NodeType == XmlNodeType.Text)
                {
                    tempNodeValue = xmlReader.Value;
                    fields.Add(tempNodeName, (tempNodeValue, tempAttribute));
                }
            }
            if (fields.Count > 0) result.Add(new Book(fields));
            fields.Clear();

            return result;
        }

    }
}
