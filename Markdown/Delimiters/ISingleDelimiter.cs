namespace Markdown.Delimiters
{
    internal interface ISingleDelimiter : IDelimiter
    {
        bool IsEndOfVisibleArea(char chr);
    }
}