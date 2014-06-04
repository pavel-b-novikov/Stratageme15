using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Extensions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class WhileStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public ParenthesisExpression WhileCondition { get; set; }

        public CodeBlock WhileBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<ParenthesisExpression>(symbol))
            {
                if (WhileCondition==null)
                {
                    WhileCondition = (ParenthesisExpression) symbol;
                    return;
                }
            }
            if (Is<CodeBlock>(symbol))
            {
                if (WhileBlock==null)
                {
                    WhileBlock = (CodeBlock) symbol;
                    return;
                }
            }
            if (Is<IStatement>(symbol))
            {
                symbol = symbol.WrapInCodeBlock();
            }

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