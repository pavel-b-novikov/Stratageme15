using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    public class IdentExpressionNodeComparer : SyntaxNodeComparerBase<IdentExpression>
    {
        protected override bool NodesEquals(IdentExpression actual, IdentExpression expected)
        {
            return actual.Identifier == expected.Identifier;
        }
    }
}
