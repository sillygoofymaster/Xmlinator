namespace XMLParsing
{
    public interface IParser
    {
        public IList<Book> Parse(string filePath);
    }
}
