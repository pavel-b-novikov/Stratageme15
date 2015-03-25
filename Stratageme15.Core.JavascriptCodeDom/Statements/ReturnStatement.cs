using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ReturnStatement : SyntaxTreeNodeBase, IRootStatement
    {
        public Expression ReturnExpression { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<ReturnStatement, Expression>(c => c.ReturnExpression, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (ReturnExpression!=null) yield return ReturnExpression;
        }

        public override string ToString()
        {
            return string.Format("return {0}", ReturnExpression);
        }

        public StatementLabel Label { get; set; }
    }
}