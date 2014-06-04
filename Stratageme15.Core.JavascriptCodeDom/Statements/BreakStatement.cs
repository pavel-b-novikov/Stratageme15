using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class BreakStatement : PrimaryExpression, IStatement
    {
        public IdentExpression BreakLabel { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IdentExpression>(symbol))
            {
                BreakLabel = (IdentExpression) symbol;
                return;
            }
            base.CollectSymbol(symbol);

        }

        public StatementLabel Label { get; set; }
    }
}