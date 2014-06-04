namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Binary
{
    public class LogicalBinaryExpression : BinaryExpression<LogicalOperator>
    {
        public override void CollectOperator(LogicalOperator op)
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