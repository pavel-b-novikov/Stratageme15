using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

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
                           typeof(Expression).IsAssignableFrom(context.TargetNode.GetType())
                           || typeof(FactParameterList).IsAssignableFrom(context.TargetNode.GetType())
                           || typeof(IndexExpression).IsAssignableFrom(context.TargetNode.GetType())
                           || typeof(IStatement).IsAssignableFrom(context.TargetNode.GetType())
                       );
            if (!acc) return false;
            if (((IdentifierNameSyntax)context.SourceNode).Identifier.ValueText == "var")
            {
                SyntaxNode prnt = context.SourceNode.Parent;
                if (prnt is VariableDeclarationSyntax) return false;
            }

            return true;
        }

        #endregion

        public override PrimaryExpression TranslateNodeInner(IdentifierNameSyntax node, TranslationContextWrapper context,
                                                             TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            if (context.Context.TargetNode is NewInvokationExpression)
            {
                TypeInfo typeInfo = context.Context.SemanticModel.GetTypeInfo(node);
                return typeInfo.ConvertedType.Name().ToIdent();
            }

            string varName = node.Identifier.ValueText;
            //well, properties and this-methods are handled independently
            if (context.IsThisFieldOrPropertyVariable(varName) || context.IsMethodOfThis(varName))
            {
                // here we need to check if we not trying to access "this" 2nd time
                bool needToExpand = true;
                if (node.Parent is MemberAccessExpressionSyntax)
                {
                    MemberAccessExpressionSyntax mex = node.Parent as MemberAccessExpressionSyntax;
                    if (mex.Expression != node)
                    {
                        if (mex.Expression is ThisExpressionSyntax)
                        {
                            needToExpand = false;
                        }
                    }
                }

                if (needToExpand)
                {
                    if (!context.IsProperty(varName))
                    {
                        context.Debug("Expanding field {0} to this.{0}", varName);
                        var ex = new FieldAccessExpression();
                        ex.Member = varName.ToIdent();
                        ex.Accessee = context.CurrentClassContext.CurrentFunction.GetJsClosureIdentifier();
                        return ex;
                    }
                    else
                    {
                        context.Debug("Expanding property {0} to this.get{0}()", varName);
                        var ce = new CallExpression();
                        var ex = new FieldAccessExpression();
                        ex.Member = string.Format("get{0}", varName).ToIdent();
                        ex.Accessee = context.CurrentClassContext.CurrentFunction.GetJsClosureIdentifier();
                        ce.Callee = ex;
                        return ce;
                    }
                }

            }
            return node.Identifier.ValueText.ToIdent();
        }
    }
}