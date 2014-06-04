using System.Collections.Generic;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class TypeofExpression : PrimaryExpression
    {
        public PrimaryExpression Expression { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<TypeofExpression, PrimaryExpression>(c => c.Expression, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (Expression!=null) yield return Expression;
        }
    }
}