using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLParsing
{
    public class Book
    {
        public string Title { get; set; }
        public string Format { get; set; }
        public string Year { get; set; }
        public string AuthorName { get; set; }
        public string AU_ID { get; set; }

        public Book()
        {
            Title = "";
            Format = "";
            Year = "";
            AuthorName = "";
            AU_ID = "";
        }

        public Book(Dictionary<string, (string, string)> fields)
        {
            Title = fields.ContainsKey("title") ? fields["title"].Item1 : "";
            Format = fields.ContainsKey("format") ? fields["format"].Item1 : "";
            Year = fields.ContainsKey("year") ? fields["year"].Item1 : "";
            AuthorName = fields.ContainsKey("author") ? fields["author"].Item1 : "";
            AU_ID = fields.ContainsKey("author") ? fields["author"].Item2 : "";
        }
    }
}
