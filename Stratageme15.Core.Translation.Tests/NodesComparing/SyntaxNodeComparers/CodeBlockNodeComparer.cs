using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    public class CodeBlockNodeComparer : SyntaxNodeComparerBase<CodeBlock>
    {
        protected override bool NodesEquals(CodeBlock actual, CodeBlock expected)
        {
            if (actual.IsEnclosed != expected.IsEnclosed) return false;
            return true;
        }
    }
}
