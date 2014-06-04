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
            if (Is<IStatement>(symbol))
            {
                symbol = symbol.WrapInCodeBlock();
            }

            if (Is<CodeBlock>(symbol))
            {
                if (TryBlock==null)
                {
                    TryBlock = (CodeBlock) symbol;
                    return;
                }
            }

            if (Is<CaseClause>(symbol))
            {
                if (CatchClause==null)
                {
                    CatchClause = (CatchClause) symbol;
                    return;
                }
            }

            if (Is<FinallyClause>(symbol))
            {
                if (FinallyBlock==null)
                {
                    FinallyBlock = (FinallyClause) symbol;
                    return;
                }
            }
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