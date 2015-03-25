using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class TernaryStatement : Expression, IRootStatement
    {
        public Expression If { get; set; }

        public Expression ThenExpression { get; set; }
        public IStatement Then { get; set; }

        public Expression ElseExpression { get; set; }
        public IStatement Else { get; set; }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (CollectExact<TernaryStatement,Expression>(c=>c.If,symbol)) return;
            if (CollectExact<TernaryStatement, Expression>(c => c.ThenExpression, symbol)) return;
            if (CollectExact<TernaryStatement, Expression>(c => c.ElseExpression, symbol)) return;
            
            if (Is<IStatement>(symbol))
            {
                if (Then == null && ThenExpression == null)
                {
                    Then = (IStatement)symbol;
                    return;
                }

                if (Else == null && ThenExpression == null)
                {
                    Else = (IStatement)symbol;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (If != null) yield return If;
            if (Then != null) yield return (SyntaxTreeNodeBase) Then;
            if (ThenExpression != null) yield return ThenExpression;
            if (Else != null) yield return (SyntaxTreeNodeBase) Else;
            if (ElseExpression != null) yield return ElseExpression;
        }      
    }
}