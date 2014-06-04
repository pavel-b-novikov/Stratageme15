using System.Collections.Generic;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class IndexExpression : SyntaxTreeNodeBase
    {
        public Expression Index { get; set; }
        
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Collect<IndexExpression, Expression, Expression>(c => c.Index, symbol)) return;
            base.CollectSymbol(symbol);
        }
        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return Index;
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}
