using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MdParser
    {
        private string[] delimiters;

        public MdParser(string[] delimiters)
        {
            this.delimiters = delimiters;
        }

        public string Parse(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}
