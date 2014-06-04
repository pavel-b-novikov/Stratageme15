namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Binary
{
    public class MathBinaryExpression : BinaryExpression<MathOperator>
    {
        public override void CollectOperator(MathOperator op)
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