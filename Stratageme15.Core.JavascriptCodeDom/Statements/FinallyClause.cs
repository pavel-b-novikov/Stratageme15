using System.Collections.Generic;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class FinallyClause : SyntaxTreeNodeBase
    {
        public CodeBlock FinallyBlock { get; set; }
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            WrapIfStatement(ref symbol);

            if (Is<CodeBlock>(symbol))
            {
                if (FinallyBlock == null)
                {
                    FinallyBlock = (CodeBlock)symbol;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return FinallyBlock;
        }
    }
}
