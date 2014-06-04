using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class CallExpression : PrimaryExpression, IRootStatement, IStatement
    {
        public Expression Callee { get; set; }

        public FactParameterList Parameters { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Collect<CallExpression, Expression, PrimaryExpression>(c => c.Callee, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return Callee;
            yield return Parameters;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Callee, Parameters);
        }

        public StatementLabel Label { get; set; }
    }
}
