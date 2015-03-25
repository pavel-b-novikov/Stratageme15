using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    public class LiteralExpressionNodeComparer : SyntaxNodeComparerBase<LiteralExpression>
    {
        protected override bool NodesEquals(LiteralExpression actual, LiteralExpression expected)
        {
            return actual.Literal == expected.Literal;
        }
    }
}
