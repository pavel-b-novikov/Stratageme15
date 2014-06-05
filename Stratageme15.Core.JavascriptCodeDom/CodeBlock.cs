using System;
using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom
{
    /// <summary>
    /// Represents the block ( "{ ... }" ) of Javascript code
    /// </summary>
    public class CodeBlock : SyntaxTreeNodeBase, IStatement, IRootCodeElement, IOrderedCollector
    {
        /// <summary>
        /// Statements in corresponding order
        /// </summary>
        public LinkedList<IStatement> Statements { get; set; }

        /// <summary>
        /// Strange ident expressions sometimes appearing in jQuery and othe libraryes during parsering.
        /// According to Iden expression is not statement, supposably these ident are left for compatibility check
        /// </summary>
        public IList<Tuple<IStatement,IdentExpression>> StrangeIdents { get; private set; }

        /// <summary>
        /// Constructs new code block with empty operator list
        /// </summary>
        public CodeBlock()
        {
            Statements = new LinkedList<IStatement>();
            StrangeIdents = new List<Tuple<IStatement, IdentExpression>>();
        }

        /// <summary>
        /// Adds node to operators list
        /// Collects IStatements, ParenthesisExpressions
        /// Alls collects string literal if it is "use strict" directive
        /// And some strange compatibility idents
        /// </summary>
        /// <param name="symbol"></param>
        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            CollectSymbolInner(symbol);
        }

        private void Add(SyntaxTreeNodeBase symbol,bool addFirst)
        {
            if (!addFirst) Statements.AddLast((IStatement) symbol);
            else
            {
                Statements.AddFirst((IStatement) symbol);
            }
        }

        private void CollectSymbolInner(SyntaxTreeNodeBase symbol,bool addFirst = false)
        {
            if (Is<IStatement>(symbol))
            {
                symbol.Role = "Statement";
                Add(symbol,addFirst);
                symbol.Parent = this;
                return;
            }
            if (Is<ParenthesisExpression>(symbol))
            {
                var pe = (ParenthesisExpression)symbol;
                if (Is<CallExpression>(pe.InnerExpression))
                {
                    CallStatement cstm = new CallStatement();
                    cstm.Parent = this;
                    cstm.CallExpression = (CallExpression)pe.InnerExpression;
                    pe.InnerExpression.Parent = cstm;
                    cstm.Role = "Statement";
                    Add(cstm,addFirst);
                    symbol.Parent = this;
                    return;
                }
                if (Is<IStatement>(pe.InnerExpression))
                {
                    pe.InnerExpression.Parent = this;
                    pe.InnerExpression.Role = "Statement";
                    Add(pe.InnerExpression,addFirst);
                    symbol.Parent = this;
                    return;
                }
            }

            if (Is<IdentExpression>(symbol))
            {
                symbol.Role = "Strange ident expression";
                var afterof = Statements.Count > 0 ? Statements.Last.Previous.Value : null;
                Tuple<IStatement, IdentExpression> strangeTuple = new Tuple<IStatement, IdentExpression>(afterof, (IdentExpression)symbol);
                StrangeIdents.Add(strangeTuple);
                symbol.Parent = this;
                return;
            }

            if (Is<StringLiteral>(symbol))
            {
                var sl = (StringLiteral)symbol;
                if (sl.String.Trim('\'') == "use strict")
                {
                    UseStrict us = new UseStrict();
                    us.Parent = this;
                    us.Role = "UseStrictDirective";
                    Add(us,addFirst);
                    symbol.Parent = this;
                    return;
                }
            }
            base.CollectSymbol(symbol);
        }

        protected override IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes()
        {
            foreach (var statement in Statements)
            {
                yield return (SyntaxTreeNodeBase) statement;
            }

            foreach (var strangeIdent in StrangeIdents)
            {
                yield return strangeIdent.Item2;
            }
        }

        /// <summary>
        /// Operator label
        /// </summary>
        public StatementLabel Label { get; set; }

        /// <summary>
        /// Places collected node in the beginning of operator list
        /// </summary>
        /// <param name="symbol"></param>
        public void CollectSymbolAtStart(SyntaxTreeNodeBase symbol)
        {
            CollectSymbolInner(symbol,true);
        }
    }
}