using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class AssignmentStatement :SyntaxTreeNodeBase, IRootStatement, IStatement
    {
        public IdentExpression Identifier { get; set; }

        public IndexerExpression Indexer { get; set; }

        public Expression AssignmentExpression { get; set; }

        public AssignmentOperator Operator { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<AssignmentStatement,IdentExpression>(c=>c.Identifier,symbol)) return;
            if (CollectExact<AssignmentStatement, IndexerExpression>(c => c.Indexer, symbol)) return;
            if (CollectExact<AssignmentStatement, Expression>(c => c.AssignmentExpression, symbol)) return;
            
            base.CollectSymbol(symbol);
        }

        public override void CollectOperator(AssignmentOperator op)
        {
            Operator = op;
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (Identifier != null) yield return Identifier;
            if (Indexer != null) yield return Indexer;
            if (AssignmentExpression != null) yield return AssignmentExpression;
        }

        public StatementLabel Label { get; set; }
    }
}