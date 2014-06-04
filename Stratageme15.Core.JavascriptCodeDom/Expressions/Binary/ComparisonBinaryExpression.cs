namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Binary
{
    public class ComparisonBinaryExpression : BinaryExpression<ComparisonOperator>
    {
        public override void CollectOperator(ComparisonOperator op)
        {
            if (!WasOperationSet)
            {
                Operator = op;
                return;
            }
            base.CollectOperator(op);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", LeftPart, OperatorConverter.OperatorString(Operator), RightPart);
        }
    }
}