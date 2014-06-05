using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ThrowStatement : SyntaxTreeNodeBase, IStatement,IRootCodeElement
    {
        public Expression ThrowExpression { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<ThrowStatement, Expression>(c=>c.ThrowExpression,symbol)) return;
            
            if (Is<CallStatement>(symbol))
            {
                if (ThrowExpression == null)
                {
                    ThrowExpression = ((CallStatement)symbol).CallExpression;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return ThrowExpression;
        }

        public StatementLabel Label { get; set; }
    }
}
