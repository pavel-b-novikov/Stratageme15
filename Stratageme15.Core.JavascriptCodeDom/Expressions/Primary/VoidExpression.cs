using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class VoidExpression : PrimaryExpression, IStatement
    {
        public Expression ExpressionToBeVoid { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<VoidExpression, Expression>(c => c.ExpressionToBeVoid, symbol)) return;
            
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (ExpressionToBeVoid != null) yield return ExpressionToBeVoid;
        }

        public StatementLabel Label { get; set; }
    }
}