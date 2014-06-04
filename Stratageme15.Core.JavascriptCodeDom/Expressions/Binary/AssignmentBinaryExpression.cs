using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Binary
{
    public class AssignmentBinaryExpression : BinaryExpression<AssignmentOperator>
    {
        public override void CollectOperator(AssignmentOperator op)
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