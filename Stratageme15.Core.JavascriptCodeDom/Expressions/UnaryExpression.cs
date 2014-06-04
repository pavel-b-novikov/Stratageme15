using System;
using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions
{
    public class UnaryExpression : PrimaryExpression
    {
        public UnaryOperator? FirstOperator { get; set; }

        public Expression Operand { get; set; }

        public override void CollectOperator(UnaryOperator op)
        {
            if (FirstOperator == null) FirstOperator = op;
            else base.CollectOperator(op);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return Operand;
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<UnaryExpression, Expression>(c => c.Operand, symbol))return;
            base.CollectSymbol(symbol);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}",FirstOperator,Operand);
        }
    }
}