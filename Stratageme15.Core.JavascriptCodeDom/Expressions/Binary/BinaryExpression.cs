using System.Collections.Generic;
using System.Reflection;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Binary
{
    public class BinaryExpression<TOperation> : Expression, IRootStatement, IBinaryExpression
    {
        private static PropertyInfo _operationProp = typeof (BinaryExpression<>).GetProperty("Operator");

        private TOperation _operation;
        protected bool WasOperationSet = false;

        public Expression LeftPart { get; set; }
        public Expression RightPart { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Collect<BinaryExpression<TOperation>, Expression, Expression>(c => c.LeftPart, symbol)) return;
            if (Collect<BinaryExpression<TOperation>, Expression, Expression>(c => c.RightPart, symbol)) return;
           
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (LeftPart != null) yield return LeftPart;
            if (RightPart != null) yield return RightPart;
        }

        public virtual TOperation Operator
        {
            get { return _operation; }
            set
            {
                WasOperationSet = true;
                _operation = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", LeftPart, Operator, RightPart);
        }

        public StatementLabel Label { get; set; }
    }
}