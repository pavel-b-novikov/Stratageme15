using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions
{
    public class ArrayCreationExpression : PrimaryExpression
    {
        public IList<Expression> ArrayElements { get; set; }

        public ArrayCreationExpression()
        {
            ArrayElements = new List<Expression>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (symbol is Expression)
            {
                symbol.Role = "ArrayElement";
                ArrayElements.Add((Expression)symbol);
                symbol.Parent = this;
                return;
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return ArrayElements;
        }
    }
}