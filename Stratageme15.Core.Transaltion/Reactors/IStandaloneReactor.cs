using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion.Reactors
{
    public interface IStandaloneReactor<out TInput> where TInput : SyntaxNode
    {
        SyntaxTreeNodeBase TranslateNode(SyntaxNode node, TranslationContext context);
    }
}
