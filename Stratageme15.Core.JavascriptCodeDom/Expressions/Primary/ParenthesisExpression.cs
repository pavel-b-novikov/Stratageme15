using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class ParenthesisExpression : PrimaryExpression, IRootCodeElement
    {
        public Expression InnerExpression { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<ParenthesisExpression,Expression>(c=>InnerExpression,symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (InnerExpression != null) yield return InnerExpression;
        }
    }
}
