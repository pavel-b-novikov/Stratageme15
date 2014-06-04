using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class DefaultClause : SyntaxTreeNodeBase
    {
        public CodeBlock DefaultBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IStatement>(symbol))
            {
                if (DefaultBlock == null)
                {
                    DefaultBlock = new CodeBlock();
                    DefaultBlock.Parent = this;
                }
                DefaultBlock.CollectSymbol(symbol);
                return;
            }

            if (Is<CodeBlock>(symbol))
            {
                if (DefaultBlock == null)
                {
                    DefaultBlock = (CodeBlock)symbol;
                    return;
                }
            }

            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return DefaultBlock;
        }
    }
}