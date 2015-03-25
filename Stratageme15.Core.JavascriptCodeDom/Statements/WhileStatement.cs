using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class WhileStatement : SyntaxTreeNodeBase, IRootStatement
    {
        public ParenthesisExpression WhileCondition { get; set; }

        public CodeBlock WhileBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<WhileStatement, ParenthesisExpression>(c => c.WhileCondition, symbol)) return;
            WrapIfStatement(ref symbol);
            if (CollectExact<WhileStatement, CodeBlock>(c => c.WhileBlock, symbol)) return;

            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (WhileCondition != null) yield return WhileCondition;
            if (WhileBlock != null) yield return WhileBlock;
        }

        public StatementLabel Label { get; set; }
    }
}