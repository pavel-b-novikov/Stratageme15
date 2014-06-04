using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class CaseClause : SyntaxTreeNodeBase
    {
        public Expression CaseExpression { get; set; }

        public CodeBlock CodeBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Collect<CaseClause, Expression, Expression>(c => c.CaseExpression, symbol)) return;
            if (Collect<CaseClause, CodeBlock, CodeBlock>(c => c.CodeBlock, symbol)) return;

            if (Is<IStatement>(symbol))
            {
                if (CodeBlock == null)
                {
                    CodeBlock = new CodeBlock();
                    CodeBlock.Parent = this;
                }
                CodeBlock.CollectSymbol(symbol);
                return;
            }

            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (CaseExpression != null) yield return CaseExpression;
            if (CodeBlock != null) yield return CodeBlock;
        }
    }
}