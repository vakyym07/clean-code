namespace Markdown.Delimiters
{
    public interface IDelimiter
    {
        string GetName();
        DelimiterType GetDelimiterType();
        bool IsCorrectDelimiter(string markdown, int position);
    }
}