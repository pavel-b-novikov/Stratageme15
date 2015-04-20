using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Reactors.Basic.Utility
{
    public static class JavascriptHelper
    {
        public static IdentExpression ToIdent(this string s)
        {
            return new IdentExpression(s);
        }

        #region Functions
        public static FunctionDefExpression CreateEmptyFunction()
        {
            FunctionDefExpression fdef = new FunctionDefExpression();
            fdef.CollectSymbol(new CodeBlock());
            fdef.CollectSymbol(new FormalParametersList());
            return fdef;
        }

        public static FunctionDefExpression CreateEmptyFunction(string name,params string[] formalParameters)
        {
            FunctionDefExpression fdef = new FunctionDefExpression();
            fdef.CollectSymbol(new CodeBlock());
            fdef.CollectSymbol(new FormalParametersList());
            fdef.CollectSymbol(name.ToIdent());
            foreach (var formalParameter in formalParameters)
            {
                fdef.Parameters.CollectSymbol(formalParameter.ToIdent());
            }
            return fdef;
        }

        public static FunctionDefExpression AppendStatement(this FunctionDefExpression function,IStatement statement)
        {
            if (function.Code==null) function.Code = new CodeBlock();
            function.Code.CollectSymbol((SyntaxTreeNodeBase) statement);
            return function;
        }

        public static FunctionDefExpression AppendParameter(this FunctionDefExpression function, string parameterName)
        {
            if (function.Parameters == null) function.Parameters = new FormalParametersList();
            function.Parameters.CollectSymbol(parameterName.ToIdent());
            return function;
        }
      
        #endregion

        public static StringLiteral Literal(this string s)
        {
            StringLiteral sl = new StringLiteral(string.Format("\"{0}\"",s));
            return sl;
        }

        public static ParenthesisExpression Parenthesize(this Expression ex)
        {
            if (ex is ParenthesisExpression) return (ParenthesisExpression) ex;
            var pe = new ParenthesisExpression();
            pe.CollectSymbol(ex);
            return pe;
        }

        public static CallExpression Call(this Expression ex,params Expression[] parameters)
        {
            var ce = new CallExpression();
            ce.CollectSymbol(ex);
            if (parameters.Length>0) ce.Parameters = new FactParameterList();
            foreach (var expression in parameters)
            {
                ce.Parameters.CollectSymbol(expression);
            }
            return ce;
        }

        public static CallExpression Call(this Expression ex, FactParameterList parameters)
        {
            var ce = new CallExpression();
            ce.CollectSymbol(ex);
            parameters.Parent = ce;
            ce.Parameters = parameters;
            return ce;
        }

        #region Variable definition
        public static VariableDefStatement Variable(this string varName,Expression initializer = null)
        {
            var varIdent = varName.ToIdent();
            VariableDefStatement vds = new VariableDefStatement();
            if (initializer==null) initializer = new NullKeywordLiteralExpression();
            vds.CollectSymbol(varIdent);
            vds.CollectSymbol(initializer);
            return vds;
        }

        public static VariableDefStatement Variable(this VariableDefStatement vds, string varName)
        {
            vds.CollectSymbol(varName.ToIdent());
            return vds;
        }

        public static VariableDefStatement Variable(this VariableDefStatement vds, IdentExpression varName)
        {
            vds.CollectSymbol(varName);
            return vds;
        }

        public static VariableDefStatement Initialize(this VariableDefStatement vds, Expression expr)
        {
            vds.CollectSymbol(expr);
            return vds;
        }
        #endregion

        public static FormalParametersList FormalParameters(this IEnumerable<string> parameters)
        {
            return new FormalParametersList(parameters.Select(c => c.ToIdent()).ToList());
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
            return Assignment(leftPart, rightPartIdent.ToIdent());
        }

        public static void EmptyColon(this SyntaxTreeNodeBase node)
        {
            node.CollectSymbol(new EmptyStatement());
        }

        public static AssignmentBinaryExpression Assignment(this string leftPart, string rightPartIdent)
        {
            return Assignment(leftPart.ToIdent(), rightPartIdent.ToIdent());
        }
      
        public static LogicalBinaryExpression Logical(this Expression ex,LogicalOperator oprtr,Expression anotherExpression)
        {
            ex = ex.Parenthesize();
            anotherExpression = anotherExpression.Parenthesize();
            LogicalBinaryExpression lbe = new LogicalBinaryExpression();
            lbe.CollectOperator(oprtr);
            lbe.CollectSymbol(ex);
            lbe.CollectSymbol(anotherExpression);
            return lbe;
        }

        public static ComparisonBinaryExpression Comparison(this Expression ex, ComparisonOperator oprtr, Expression anotherExpression)
        {
            ex = ex.Parenthesize();
            anotherExpression = anotherExpression.Parenthesize();
            ComparisonBinaryExpression lbe = new ComparisonBinaryExpression();
            lbe.CollectOperator(oprtr);
            lbe.CollectSymbol(ex);
            lbe.CollectSymbol(anotherExpression);
            return lbe;
        }

        public static TSyntaxNode Colon<TSyntaxNode>(this TSyntaxNode node) where TSyntaxNode : SyntaxTreeNodeBase
        {
            node.IsScolonNeeded = true;
            return node;
        }

        public static FieldAccessExpression Member(this Expression ex, string memberName)
        {
            FieldAccessExpression mex = new FieldAccessExpression();
            mex.CollectSymbol(ex);
            mex.CollectSymbol(memberName.ToIdent());
            return mex;
        }

        public static IndexerExpression Index(this Expression ex, int number)
        {
            return Index(ex, new NumberLiteral(number));
        }

        public static IndexerExpression Index(this Expression ex, string field)
        {
            return Index(ex, new StringLiteral(field));
        }

        public static IndexerExpression Index(this Expression ex, Expression indexer)
        {
            IndexerExpression idx = new IndexerExpression();
            idx.CollectSymbol(ex);
            IndexExpression idxr = new IndexExpression();
            idxr.CollectSymbol(indexer);
            idx.CollectSymbol(idxr);
            return idx;
        }

        public static FieldAccessExpression Prototype(this Expression ex,string prototypeMember)
        {
            return Member(ex, JavascriptElements.Prototype).Member(prototypeMember);
        }

        public static CallExpression CallMember(this Expression ex, string memberName, params Expression[] arguments)
        {
            var mex = Member(ex, memberName);
            var c = Call(mex, arguments);
            return c;
        }

        public static ReturnStatement ReturnIt(this string varName)
        {
            ReturnStatement rs = new ReturnStatement();
            rs.CollectSymbol(varName.ToIdent());
            return rs;
        }

        public static ReturnStatement ReturnIt(this Expression ex)
        {
            ReturnStatement rs = new ReturnStatement();
            rs.CollectSymbol(ex);
            return rs;
        }

        #region Prototype
        public static IStatement PrototypeMethod(this string className, string methodName,
                                                                 FunctionDefExpression method,bool isPrivate,bool isStatic)
        {
            if (!isPrivate)
            {
                if (isStatic)
                {
                    return className.ToIdent().Member(methodName).Assignment(method);
                }
                return className.ToIdent().Member(JavascriptElements.Prototype).Member(methodName).Assignment(method);
            }
            method.Name = null;
            return methodName.Variable(method);
        }

        public static IStatement PrototypeMethod(this ClassDeclarationSyntax classDecl,
                                                                 string methodName, FunctionDefExpression method, bool isPrivate, bool isStatic)
        {
            return PrototypeMethod(classDecl.Identifier.ValueText, methodName, method, isPrivate, isStatic);
        }

        public static IStatement AsMethod(this FunctionDefExpression function, string className, string methodName, bool isPrivate = false, bool isStatic = false)
        {
            return PrototypeMethod(className,methodName,function,isPrivate,isStatic);
        }
        public static IStatement AsMethod(this FunctionDefExpression function, ClassDeclarationSyntax classDecl, string methodName, bool isPrivate = false, bool isStatic = false)
        {
            return PrototypeMethod(classDecl.Identifier.ValueText, methodName, function, isPrivate, isStatic);
        }
        #endregion
    }
}
