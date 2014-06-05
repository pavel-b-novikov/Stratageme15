using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Extensions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class ForStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public VariableDefStatement InitializationStatement { get; set; }

        public Expression InitializationExpression { get; set; }

        public Expression ComparisonExpression { get; set; }

        public Expression IncrementExpression { get; set; }

        public CodeBlock CodeBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<ForStatement, VariableDefStatement>(c => c.InitializationStatement, symbol)) return;
          
            if (Is<Expression>(symbol))
            {
                if (InitializationExpression == null && InitializationStatement == null)
                {
                    InitializationExpression = (Expression)symbol;
                    return;
                }

                if (ComparisonExpression == null)
                {
                    ComparisonExpression = (Expression)symbol;
                    return;
                }

                if (IncrementExpression == null)
                {
                    IncrementExpression = (Expression)symbol;
                    return;
                }

                if (Is<CallExpression>(symbol))
                {
                    symbol = symbol.WrapInCodeBlock();
                    if (CodeBlock == null)
                    {
                        CodeBlock = (CodeBlock)symbol;
                        return;
                    }
                }
            }

            WrapIfStatement(ref symbol);
            if (CollectExact<ForStatement, CodeBlock>(c => c.CodeBlock, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (InitializationStatement != null) yield return InitializationStatement;
            if (InitializationExpression != null) yield return InitializationExpression;
            if (ComparisonExpression != null) yield return ComparisonExpression;
            if (IncrementExpression != null) yield return IncrementExpression;
            if (CodeBlock != null) yield return CodeBlock;
        }

        public StatementLabel Label { get; set; }
    }
}