using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    public class ComparisonBinaryExpressionNodeComparer : SyntaxNodeComparerBase<ComparisonBinaryExpression>
    {
        protected override bool NodesEquals(ComparisonBinaryExpression actual, ComparisonBinaryExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }

    public class AssignmentBinaryExpressionNodeComparer : SyntaxNodeComparerBase<AssignmentBinaryExpression>
    {
        protected override bool NodesEquals(AssignmentBinaryExpression actual, AssignmentBinaryExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }

    public class BitwiseBinaryExpressionNodeComparer : SyntaxNodeComparerBase<BitwiseBinaryExpression>
    {
        protected override bool NodesEquals(BitwiseBinaryExpression actual, BitwiseBinaryExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }

    public class LogicalBinaryExpressionNodeComparer : SyntaxNodeComparerBase<LogicalBinaryExpression>
    {
        protected override bool NodesEquals(LogicalBinaryExpression actual, LogicalBinaryExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }

    public class MathBinaryExpressionNodeComparer : SyntaxNodeComparerBase<MathBinaryExpression>
    {
        protected override bool NodesEquals(MathBinaryExpression actual, MathBinaryExpression expected)
        {
            return actual.Operator == expected.Operator;
        }
    }
}
