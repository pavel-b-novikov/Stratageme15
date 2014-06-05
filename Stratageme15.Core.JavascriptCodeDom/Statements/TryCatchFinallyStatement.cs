using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Extensions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class TryCatchFinallyStatement : SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public CodeBlock TryBlock { get; set; }

        public CatchClause CatchClause { get; set; }

        public FinallyClause FinallyBlock { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            
            WrapIfStatement(ref symbol);
            if (CollectExact<TryCatchFinallyStatement, CodeBlock>(c => c.TryBlock, symbol)) return;
            if (CollectExact<TryCatchFinallyStatement, CatchClause>(c => c.CatchClause, symbol)) return;
            if (CollectExact<TryCatchFinallyStatement, FinallyClause>(c => c.FinallyBlock, symbol)) return;
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (TryBlock != null) yield return TryBlock;
            if (CatchClause != null) yield return CatchClause;
            if (FinallyBlock != null) yield return FinallyBlock;
        }

        public StatementLabel Label { get; set; }
    }
}