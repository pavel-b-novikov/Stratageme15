using System.Collections.Generic;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class IndexerExpression : PrimaryExpression
    {
        public Expression AccessedExpression { get; set; }

        public IndexExpression Index { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Collect<IndexerExpression, Expression, Expression>(c => c.AccessedExpression, symbol)) return;
            if (Collect<IndexerExpression, IndexExpression, Expression>(c => c.Index, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            yield return AccessedExpression;
            yield return Index;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]",AccessedExpression, Index);
        }
    }
}