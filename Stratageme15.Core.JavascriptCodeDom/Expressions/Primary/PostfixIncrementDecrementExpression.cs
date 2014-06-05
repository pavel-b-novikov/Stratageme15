using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class PostfixIncrementDecrementExpression : PrimaryExpression,IRootStatement, IStatement
    {
        public Expression Callee { get; set; }

        public IndrementDecrementOperator Operator { get; set; }

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
            return string.Format("{1} {0}", Operator, Callee);
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<PostfixIncrementDecrementExpression,Expression>(c=>c.Callee,symbol)) return;
            base.CollectSymbol(symbol);
        }
    }
}
