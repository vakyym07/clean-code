using System.Collections.Generic;
using Markdown.Delimiters;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
	    private readonly MdParser parser;

	    public Md(Dictionary<string, IDelimiter> delimiters)
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