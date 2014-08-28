using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics
{
    public static class SemanticHelpers
    {
        public static MemberInfo GetMember(MemberAccessExpressionSyntax node, TranslationContext ctx)
        {
            var accessedType = TypeInferer.InferTypeFromExpression(node.Expression, ctx);
            return TypeInferer.GetMember(accessedType, node.Name.Identifier.ValueText);
        }
    }
}
