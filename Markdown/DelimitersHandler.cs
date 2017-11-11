using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class DelimitersHandler
    {
        public static Dictionary<string, Func<string, int, HandlerResult>> DelimeterFuncs { get; } 
            = new Dictionary<string, Func<string, int, HandlerResult>>
        {
            {"_", SingleUnderLineHandler},
            {"__", DoubleUnderlineHandler},
            {"#", GridHandler},
            {"\\", ShieldingHandler}
        };

        private static HandlerResult SingleUnderLineHandler(string markdown, int position)
        {
            throw new NotImplementedException();
        }

        private static HandlerResult DoubleUnderlineHandler(string markdown, int position)
        {
            throw new NotImplementedException();
        }

        private static HandlerResult GridHandler(string markdown, int position)
        {
            throw new NotImplementedException();
        }

        private static HandlerResult ShieldingHandler(string markdown, int position)
        {
            throw new NotImplementedException();
        }
    }
}