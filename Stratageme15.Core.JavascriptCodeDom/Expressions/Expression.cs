using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions
{
    public class Expression : SyntaxTreeNodeBase, IStatement
    {
        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Empty;
        }

        public StatementLabel Label { get; set; }
    }
}