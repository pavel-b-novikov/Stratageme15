using System.Collections.Generic;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions
{
    public class Expression : SyntaxTreeNodeBase
    {
        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            return Empty;
        }

    }
}