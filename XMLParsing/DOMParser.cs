using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLParsing
{
    internal class DOMParser: IParser
    {
        public IList<Book> Parse(string filePath)
        {
            IList<Book> result = new List<Book>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList books = xmlDoc.SelectNodes("//libraryDatabase/book");
            
            foreach(XmlNode book in books)
            {
                Dictionary<string, (string, string)> fields = new();
                XmlNodeList attributes = book.ChildNodes;
                foreach (XmlNode attribute in attributes)
                {
                    string tempAttribute = "?";
                    if (attribute.Attributes["AU_ID"] != null) tempAttribute = attribute.Attributes["AU_ID"].Value;

                    fields.Add(attribute.Name, (attribute.InnerText, tempAttribute));
                }
                Book resultBook = new Book(fields);
                result.Add(resultBook);
            }

            return result;
        }
    }
}
