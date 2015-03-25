using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class BreakStatement : PrimaryExpression
    {
        public IdentExpression BreakLabel { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<BreakStatement, IdentExpression>(c => c.BreakLabel, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (BreakLabel != null) yield return BreakLabel;
        }
    }
}