using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Logging;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public class IdentifierNameSyntaxReactor : ExpressionReactorBase<IdentifierNameSyntax, PrimaryExpression>,
                                               ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            //here we need to make swith for case of var is variable name, not type inference

            bool acc = (
                           typeof (Expression).IsAssignableFrom(context.TranslatedNode.GetType())
                           || typeof (FactParameterList).IsAssignableFrom(context.TranslatedNode.GetType())
                           || typeof (IndexExpression).IsAssignableFrom(context.TranslatedNode.GetType())
                           || typeof (IStatement).IsAssignableFrom(context.TranslatedNode.GetType())
                       );
            if (!acc) return false;
            if (((IdentifierNameSyntax) context.SourceNode).Identifier.ValueText == "var")
            {
                SyntaxNode prnt = context.SourceNode.Parent;
                if (prnt is VariableDeclarationSyntax) return false;
            }

            return true;
        }

        #endregion

        public override PrimaryExpression TranslateNodeInner(IdentifierNameSyntax node, TranslationContext context,
                                                             TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            if (context.TranslatedNode is NewInvokationExpression)
            {
                Type type = TypeInferer.InferTypeFromExpression(node, context);
                if (type != null)
                {
                    return type.JavascriptTypeName().Ident();
                }
            }

            string varName = node.Identifier.ValueText;
            //well, properties and this-methods are handled independently
            if (context.IsThisFieldOrPropertyVariable(varName) || context.IsMethodOfThis(varName))
            {
                if (!(node.Parent is MemberAccessExpressionSyntax))
                {
                    if (!context.IsProperty(varName))
                    {
                        context.Debug("Expanding field {0} to this.{0}", varName);
                        var ex = new FieldAccessExpression();
                        ex.Member = varName.Ident();
                        ex.Accessee = context.CurrentClassContext.CurrentFunction.GetJsClosureIdentifier();
                        return ex;
                    }
                    else
                    {
                        context.Debug("Expanding property {0} to this.get{0}()", varName);
                        var ce = new CallExpression();
                        var ex = new FieldAccessExpression();
                        ex.Member = string.Format("get{0}", varName).Ident();
                        ex.Accessee = context.CurrentClassContext.CurrentFunction.GetJsClosureIdentifier();
                        ce.Callee = ex;
                        return ce;
                    }
                }
            }
            return node.Identifier.ValueText.Ident();
        }
    }
}