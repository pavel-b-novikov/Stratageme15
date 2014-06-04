using System;
using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Exceptions;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Statements
{
    public class VariableDefStatement : SyntaxTreeNodeBase, IRootStatement
    {
        public IList<Tuple<IdentExpression, AssignmentOperator, Expression>> Variables { get; set; }

        private IList<IdentExpression> _pendingIdents;

        public VariableDefStatement()
        {
            _pendingIdents = new List<IdentExpression>();
            Variables = new List<Tuple<IdentExpression, AssignmentOperator, Expression>>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (symbol is IdentExpression)
            {
                _pendingIdents.Add((IdentExpression) symbol);
                Variables.Add(new Tuple<IdentExpression, AssignmentOperator, Expression>((IdentExpression) symbol,AssignmentOperator.Set,null));
                return;
            }

            if (symbol is AssignmentStatement)
            {
                var a = (AssignmentStatement) symbol;
                Variables.Add(new Tuple<IdentExpression, AssignmentOperator, Expression>(a.Identifier,a.Operator,a.AssignmentExpression));
                return;
            }

            throw new UnexpectedException(symbol,this);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            foreach (var variable in Variables)
            {
                yield return variable.Item1;
                yield return variable.Item3;
            }
        }

        public StatementLabel Label { get; set; }
    }
}