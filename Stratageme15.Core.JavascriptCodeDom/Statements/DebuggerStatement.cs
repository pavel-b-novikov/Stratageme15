using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class DebuggerStatement : SyntaxTreeNodeBase, IStatement, IRootStatement
    {
        public override string ToString()
        {
            return "debugger";
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Empty;
        }

        public StatementLabel Label { get; set; }
    }
}
