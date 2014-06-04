using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratageme15.Core.Tools.JavascriptParser
{
    public class ParseringException : Exception
    {
        public List<ParseringIssueDescription> Issues { get; private set; }
        public NodeInfoTree TreePart { get; private set; }
        public ParseringException(List<ParseringIssueDescription> issues, NodeInfoTree treePart)
        {
            TreePart = treePart;
            Issues = issues;
        }

        public override string Message
        {
            get
            {
                return string.Join("\r\n",Issues.Select(c=>c.ToString()));
            }
        }
    }
}
