using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class PrefixIncrementDecrementExpression : PrimaryExpression, IRootStatement, IStatement
    {
        public Expression Callee { get; set; }

        public IndrementDecrementOperator Operator { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<Expression>(symbol))
            {
                if (Callee==null)
                {
                    Callee = (Expression) symbol;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return Callee;
        }

        public StatementLabel Label { get; set; }

        public override void CollectOperator(IndrementDecrementOperator op)
        {
            Operator = op;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}" , Operator,Callee);
        }
    }
}
