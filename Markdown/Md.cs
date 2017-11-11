using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
	    private readonly MdParser parser;

	    public Md(string[] delimiters)
	    {
	        parser = new MdParser(delimiters);
	    }

		public string RenderToHtml(string markdown)
		{
		    return parser.Parse(markdown);
		}
	}

	[TestFixture]
	public class Md_ShouldRender
	{
	}
}