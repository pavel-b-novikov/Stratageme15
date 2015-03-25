using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics.Reactors
{
    public class TacticsMemberAccessSyntaxReactor : ReactorBase<MemberAccessExpressionSyntax>,ISituationReactor
    {
        protected override void HandleNode(MemberAccessExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            ////step 1 - determine expression type
            //var t = TypeInferer.InferTypeFromExpression(node.Expression, context);
            ////step 2 - determine member type
            ///*
            // * Possible values are:
            // * event
            // * property
            // * field
            // */

            //var member = TypeInferer.GetMember(t, node.Name.Identifier.ValueText);
            //var tacticsRepo = context.GetTacticsRepository();
            //var tactics = tacticsRepo.GetAppropriateTactics(t);
            //foreach (var tc in tactics)
            //{
            //    switch (member.MemberType)
            //    {
            //        case MemberTypes.Property:
            //            tc.ImperativeInstancePropertyAccess(node, context);
            //            break;
            //    }    
            //}
            

        }

        public bool IsAcceptable(TranslationContext context)
        {
            /*since Memberaccess expression is part of more complex expressions
             * such as Invokation expression or indexer expression which should be handled
             * by another tactics methods then here we need to distinguish simple member access and
             * more complex construction being handled in another reactors
             */

            var parent = context.SourceNode.Parent;
            bool isParentInvokation = parent is InvocationExpressionSyntax;
            bool isIndexer = parent is ElementAccessExpressionSyntax;

            return !(isIndexer || isParentInvokation);
        }
    }
}
