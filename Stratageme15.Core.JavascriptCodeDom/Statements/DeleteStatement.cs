using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class DeleteStatement: PrimaryExpression,IStatement
    {
        public Expression ExpressionToDelete { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<DeleteStatement,Expression>(c=>c.ExpressionToDelete,symbol)) return;

            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (ExpressionToDelete != null) yield return ExpressionToDelete;
        }

        public StatementLabel Label { get; set; }
    }
}
