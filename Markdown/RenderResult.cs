namespace Markdown
{
    public class RenderResult
    {
        public RenderResult(string renderedValue, int newPosition)
        {
            RenderedValue = renderedValue;
            NewPosition = newPosition;
        }

        public string RenderedValue { get; }
        public int NewPosition { get; }
    }
}