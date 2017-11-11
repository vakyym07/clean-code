namespace Markdown
{
    public class HandlerResult
    {
        public HandlerResult(string renderedValue, int newPosition)
        {
            RenderedValue = renderedValue;
            NewPosition = newPosition;
        }

        public string RenderedValue { get; }
        public int NewPosition { get; }
    }
}