using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XMLParsing.Book;

namespace XMLParsing
{
    public class SearchParameters
    {
        public string Title { get; set; }
        public string Format { get; set; }
        public string Year { get; set; }
        public string AuthorName { get; set; }
        public string AU_ID { get; set; }
        
        public SearchParameters()
        {
            Title = "";
            Format = "";
            Year = "";
            AuthorName = "";
            AU_ID = "";
        }

        public bool BookFits (Book book)
        {
            if (book.Title.ToLower().Contains(Title.ToLower())
                && book.Format.ToLower().Contains(Format.ToLower())
                && book.Year.Contains(Year)
                && book.AuthorName.ToLower().Contains(AuthorName.ToLower())
                && book.AU_ID.StartsWith(AU_ID)) return true;
            return false;
        }

        public void Clear()
        {
            Title = "";
            Format = "";
            Year = "";
            AuthorName = "";
            AU_ID = "";
        }
    }
}
