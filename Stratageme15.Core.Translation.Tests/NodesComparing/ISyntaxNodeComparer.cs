using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing
{
    public interface ISyntaxNodeComparer
    {
        bool EqualsExceptChildren(SyntaxTreeNodeBase one, SyntaxTreeNodeBase another);
    }
}
