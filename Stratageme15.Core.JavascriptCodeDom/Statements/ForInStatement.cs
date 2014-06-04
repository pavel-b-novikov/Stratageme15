using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ForInStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public Expression IterationExpression { get; set; }

        public ForInStatementVariableDeclaration IteratorVariable { get; set; }

        public CodeBlock CodeBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<ForInStatementVariableDeclaration>(symbol))
            {
                if (IteratorVariable==null)
                {
                    IteratorVariable = (ForInStatementVariableDeclaration)symbol;
                    return;
                }
            }

            if (Is<Expression>(symbol))
            {
                if (IterationExpression==null)
                {
                    IterationExpression = (Expression)symbol;
                    return;
                }
            }

            WrapIfStatement(ref symbol);

            if (Is<CodeBlock>(symbol))
            {
                if (CodeBlock==null)
                {
                    CodeBlock = (CodeBlock) symbol;
                    return;
                }
            }
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