namespace Markdown.DelimitersHandlers
{
    public interface IDelimiterHandler
    {
        string RenderToHtml(string markdown);
        bool IsIgnoredDelimiter(string delimiter);
    }
}