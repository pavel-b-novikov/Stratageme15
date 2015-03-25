using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ForInStatement : SyntaxTreeNodeBase, IRootStatement
    {
        public Expression IterationExpression { get; set; }

        public ForInStatementVariableDeclaration IteratorVariable { get; set; }

        public CodeBlock CodeBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<ForInStatement, ForInStatementVariableDeclaration>(c => c.IteratorVariable, symbol)) return;
            if (CollectExact<ForInStatement, Expression>(c => c.IterationExpression, symbol)) return;
           
            WrapIfStatement(ref symbol);

            if (CollectExact<ForInStatement, CodeBlock>(c => c.CodeBlock, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (IteratorVariable != null) yield return IteratorVariable;
            if (IterationExpression != null) yield return IterationExpression;
            if (CodeBlock != null) yield return CodeBlock;
        }

        public StatementLabel Label { get; set; }
    }
}