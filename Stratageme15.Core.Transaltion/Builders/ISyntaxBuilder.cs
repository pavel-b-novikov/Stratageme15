using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Transaltion.Builders
{
    internal interface ISyntaxBuilder<TResult> where TResult : SyntaxTreeNodeBase
    {
        TResult Build();
    }
}