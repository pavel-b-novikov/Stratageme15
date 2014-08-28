using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class LiteralExpressionSyntaxReactor : ExpressionReactorBase<LiteralExpressionSyntax, LiteralExpression>
    {
        public override LiteralExpression TranslateNodeInner(LiteralExpressionSyntax node, TranslationContext context,
                                                             TranslationResult res)
        {
            res.Strategy = TranslationStrategy.DontTraverseChildren;
            SyntaxToken literal = node.Token;
            if (literal.CSharpKind() == SyntaxKind.StringLiteralToken)
            {
                return new StringLiteral((string) literal.Value, false);
            }

            if (literal.CSharpKind() == SyntaxKind.NumericLiteralToken)
            {
                return new NumberLiteral(decimal.Parse(literal.ValueText));
            }

            if (literal.CSharpKind() == SyntaxKind.NullKeyword)
            {
                return new NullKeywordLiteralExpression();
            }
            if (literal.CSharpKind() == SyntaxKind.TrueKeyword)
            {
                return new TrueBooleanKeywordLiteralExpression();
            }
            if (literal.CSharpKind() == SyntaxKind.FalseKeyword)
            {
                return new FalseBooleanKeywordLiteralExpression();
            }
            throw new Exception("Unrecognized literal expression");
        }
    }
}