using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class BreakStatement : PrimaryExpression, IStatement
    {
        public IdentExpression BreakLabel { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<BreakStatement, IdentExpression>(c => c.BreakLabel, symbol)) return;
            base.CollectSymbol(symbol);

        }

        public StatementLabel Label { get; set; }
    }
}