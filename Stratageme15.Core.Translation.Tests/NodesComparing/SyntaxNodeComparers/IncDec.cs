using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    public class PostfixIncrementDecrementExpressionNodeComparer : SyntaxNodeComparerBase<PostfixIncrementDecrementExpression>
    {
        protected override bool NodesEquals(PostfixIncrementDecrementExpression actual, PostfixIncrementDecrementExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }

    public class PrefixIncrementDecrementExpressionNodeComparer : SyntaxNodeComparerBase<PrefixIncrementDecrementExpression>
    {
        protected override bool NodesEquals(PrefixIncrementDecrementExpression actual, PrefixIncrementDecrementExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }

    public class UnaryExpressionNodeComparer : SyntaxNodeComparerBase<UnaryExpression>
    {
        protected override bool NodesEquals(UnaryExpression actual, UnaryExpression expected)
        {
            return actual.Operand == expected.Operand;
        }
    }
}
