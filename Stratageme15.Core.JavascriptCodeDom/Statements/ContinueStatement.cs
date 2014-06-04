using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ContinueStatement : PrimaryExpression, IStatement
    {
        public IdentExpression ContinueLabel { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<ContinueStatement, IdentExpression>(c => c.ContinueLabel, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (ContinueLabel != null) yield return ContinueLabel;
        }

        public StatementLabel Label { get; set; }
    }
}