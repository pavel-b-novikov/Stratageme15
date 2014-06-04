using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class LiteralExpressionSyntaxReactor : ExpressionReactorBase<LiteralExpressionSyntax,LiteralExpression>
    {
        public override LiteralExpression TranslateNodeInner(LiteralExpressionSyntax node, TranslationContext context, TranslationResult res)
        {
            res.Strategy = TranslationStrategy.DontTraverseChildren;            
            var literal = node.Token;
            if (literal.Kind == SyntaxKind.StringLiteralToken)
            {
                return new StringLiteral((string)literal.Value,false);
            }

            if (literal.Kind == SyntaxKind.NumericLiteralToken)
            {
                return new NumberLiteral(decimal.Parse(literal.ValueText));
            }

            if (literal.Kind == SyntaxKind.NullKeyword)
            {
                return new NullKeywordLiteralExpression();
            }
            if (literal.Kind == SyntaxKind.TrueKeyword)
            {
                return new TrueBooleanKeywordLiteralExpression();
            }
            if (literal.Kind==SyntaxKind.FalseKeyword)
            {
                return new FalseBooleanKeywordLiteralExpression();
            }
            throw new Exception("Unrecognized literal expression");
        }
    }
}
