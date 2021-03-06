using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class CallStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public CallExpression CallExpression { get; set; }


        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<CallStatement, CallExpression>(c => c.CallExpression, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (CallExpression != null) yield return CallExpression;
        }

        public StatementLabel Label { get; set; }

        public override string ToString()
        {
            return CallExpression.ToString();
        }
    }
}