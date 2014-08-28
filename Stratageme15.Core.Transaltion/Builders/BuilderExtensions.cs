using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation.Builders.Expressions;
using Stratageme15.Core.Translation.Builders.Function;

namespace Stratageme15.Core.Translation.Builders
{
    public static class BuilderExtensions
    {
        public static IdentExpression Ident(this string s)
        {
            return new IdentExpression(s);
        }

        public static FormalParametersList FormalParameters(this IEnumerable<string> parameters)
        {
            return new FormalParametersList(parameters.Select(c => c.Ident()).ToList());
        }

        public static AssignmentBinaryExpression IdentifiersAssignment(string leftIdent, string rightIdent)
        {
            var abe = new AssignmentBinaryExpression();
            abe.CollectOperator(AssignmentOperator.Set);
            abe.CollectSymbol(Ident(leftIdent));
            abe.CollectSymbol(Ident(rightIdent));
            return abe;
        }

        public static AssignmentBinaryExpression Assignment(this Expression leftPart, Expression rightPart)
        {
            var abe = new AssignmentBinaryExpression();
            abe.CollectOperator(AssignmentOperator.Set);
            abe.CollectSymbol(leftPart);
            abe.CollectSymbol(rightPart);
            return abe;
        }

        public static AssignmentBinaryExpression Assignment(this Expression leftPart, string rightPartIdent)
        {
            return Assignment(leftPart, Ident(rightPartIdent));
        }

        public static void EmptyColon(this SyntaxTreeNodeBase node)
        {
            node.CollectSymbol(new EmptyStatement());
        }

        public static AssignmentBinaryExpression Assignment(this string leftPart, string rightPartIdent)
        {
            return Assignment(Ident(leftPart), Ident(rightPartIdent));
        }

        public static MemberAccessExpressionBuilder MemberAccess(this string ident)
        {
            return new MemberAccessExpressionBuilder(ident);
        }

        public static MemberAccessExpressionBuilder MemberAccess(this Expression e)
        {
            return new MemberAccessExpressionBuilder(e);
        }

        public static TSyntaxNode Colon<TSyntaxNode>(this TSyntaxNode node) where TSyntaxNode : SyntaxTreeNodeBase
        {
            node.IsScolonNeeded = true;
            return node;
        }

        public static FunctionDefExpressionBuilder Function(this string s)
        {
            return FunctionDefExpressionBuilder.Function(s);
        }
    }
}