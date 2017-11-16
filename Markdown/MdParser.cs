using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Delimiters;

namespace Markdown
{
    public class MdParser
    {
        private readonly Dictionary<string, IDelimiter>  delimiters;

        public MdParser(Dictionary<string, IDelimiter> delimiters)
        {
            this.delimiters = delimiters;
        }

        public string Parse(string markdown)
        {
            return new BaseHandlerDelimiter().RentderToHtml(markdown, 0, delimiters);
        }
    }
}
