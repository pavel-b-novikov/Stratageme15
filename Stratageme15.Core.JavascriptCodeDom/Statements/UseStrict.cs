using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class UseStrict : SyntaxTreeNodeBase,IStatement,IRootCodeElement
    {
        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Empty;
        }

        public override string ToString()
        {
            return "use strict;";
        }

        public StatementLabel Label { get; set; }
    }
}
