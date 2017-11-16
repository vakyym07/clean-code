namespace Markdown.Delimiters
{
    public class Token
    {
        public Token(string delimiterName, int positionInString, TokenStatus status)
        {
            Name = delimiterName;
            Position = positionInString;
            Status = status;
        }

        public Token(int positionInString, TokenStatus status)
        {
            Position = positionInString;
            Status = status;
        }

        public TokenStatus Status { get; }
        public string Name { get; }
        public int Position { get; }
    }

    public enum TokenStatus
    {
        Delimiter,
        NotDelimiter
    }
}