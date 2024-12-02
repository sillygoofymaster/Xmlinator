using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Xml.Linq;
using System.Xml;

namespace XMLParsing
{
    internal class LINQParser : IParser
    {
        public IList<Book> Parse(string filePath)
        {
            IList<Book> result = new List<Book>();
            XDocument document;
            using var xmlReader = new XmlTextReader(filePath);
            document = XDocument.Load(xmlReader);
            var books = from book in document.Descendants("book")
                         select
            new Book
            {
                Title = book.Element("title")?.Value ?? "",
                AuthorName = book.Element("author")?.Value ?? "",
                AU_ID = book.Element("author")?.Attribute("AU_ID")?.Value ?? "",
                Format = book.Element("format")?.Value ?? "",
                Year = book.Element("year")?.Value ?? "",
            };

            foreach (var book in books)
            {
                result.Add(book);
            }

            return result;
        }
    }
}
