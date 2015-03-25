using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class DoWhileStatement : SyntaxTreeNodeBase, IStatement, IRootStatement
    {
        public CodeBlock CodeBlock { get; set; }

        public ParenthesisExpression WhileExpression { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<DoWhileStatement, ParenthesisExpression>(c => c.WhileExpression, symbol)) return;
            WrapIfStatement(ref symbol);
            if (CollectExact<DoWhileStatement, CodeBlock>(c => c.CodeBlock, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return CodeBlock;
            yield return WhileExpression;
        }

        public StatementLabel Label { get; set; }
    }
}
