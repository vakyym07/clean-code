namespace Markdown.Delimiters
{
    internal interface IDoubleDelimiter : IDelimiter
    {
        bool IsCorrectOpenedDelimiter(string markdown, int position);
        bool IsCorrectClosedDelimiter(string markdown, int position);
    }
}