using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Expressions.Primary
{
    public class NewInvokationExpression : PrimaryExpression, IStatement
    {
        public FactParameterList Parameters { get; set; }

        public FieldAccessExpression IdentifierChain { get; set; }

        public override string ToString()
        {
            return string.Format("new {0}{1}", IdentifierChain, Parameters);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            if (IdentifierChain != null) yield return IdentifierChain;
            if (Parameters != null) yield return Parameters;
        }

        private FieldAccessExpression _root;
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            
            if (Is<IdentExpression>(symbol))
            {
                if (IdentifierChain == null)
                {
                    IdentifierChain = new FieldAccessExpression();
                    _root = IdentifierChain;
                    IdentifierChain.Accessee = (Expression)symbol;
                    symbol.Parent = this;
                    return;
                }else
                {
                    if (IdentifierChain.Member==null)
                    {
                        IdentifierChain.Member = (IdentExpression)symbol;
                        symbol.Parent = this;
                        return;
                    }else
                    {
                        var nidc = new FieldAccessExpression();
                        nidc.Accessee = IdentifierChain;
                        nidc.Member = (IdentExpression) symbol;
                        IdentifierChain = nidc;
                        symbol.Parent = this;
                        return;
                    }
                }
            }
            if (Is<FieldAccessExpression>(symbol))
            {
                if (IdentifierChain == null)
                {
                    symbol.Role = "New Target";
                    IdentifierChain = (FieldAccessExpression) symbol;
                    symbol.Parent = this;
                    return;
                }
            }
            if (Is<Expression>(symbol))
            {
                if (IdentifierChain == null)
                {
                    IdentifierChain = new FieldAccessExpression();
                    _root = IdentifierChain;
                    IdentifierChain.Accessee = (Expression)symbol;
                    symbol.Parent = this;
                    return;
                }
                if (_root!=null && _root.Accessee==null)
                {
                    _root.Accessee = (Expression)symbol;
                    symbol.Parent = this;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }
    }
}