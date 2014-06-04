using System;
using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.JavascriptCodeDom
{
    public class CodeBlock : SyntaxTreeNodeBase, IStatement, IRootCodeElement
    {
        public IList<IStatement> Statements { get; set; }

        public IList<Tuple<IStatement,IdentExpression>> StrangeIdents { get; private set; }

        public CodeBlock()
        {
            Statements = new List<IStatement>();
            StrangeIdents = new List<Tuple<IStatement, IdentExpression>>();
        }

        public override void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (Is<IStatement>(symbol))
            {
                symbol.Role = "Statement";
                Statements.Add((IStatement) symbol);
                return;
            }
            if (Is<ParenthesisExpression>(symbol))
            {
                var pe = (ParenthesisExpression) symbol;
                if (Is<CallExpression>(pe.InnerExpression))
                {
                    CallStatement cstm = new CallStatement();
                    cstm.Parent = this;
                    cstm.CallExpression = (CallExpression) pe.InnerExpression;
                    pe.InnerExpression.Parent = cstm;
                    cstm.Role = "Statement";
                    Statements.Add(cstm);
                    return;
                }
                if (Is<IStatement>(pe.InnerExpression))
                {
                    pe.InnerExpression.Parent = this;
                    pe.InnerExpression.Role = "Statement";
                    Statements.Add((IStatement) pe.InnerExpression);
                    return;
                }
            }

            if (Is<IdentExpression>(symbol))
            {
                symbol.Role = "Strange ident expression";
                var afterof = Statements.Count > 0 ? Statements[Statements.Count - 1] : null;
                Tuple<IStatement,IdentExpression> strangeTuple = new Tuple<IStatement, IdentExpression>(afterof,(IdentExpression) symbol);
                StrangeIdents.Add(strangeTuple);
                return;
            }

            if (Is<StringLiteral>(symbol))
            {
                var sl = (StringLiteral) symbol;
                if (sl.String.Trim('\'')=="use strict")
                {
                    UseStrict us = new UseStrict();
                    us.Parent = this;
                    us.Role = "UseStrictDirective";
                    Statements.Add(us);
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

        public StatementLabel Label { get; set; }
    }
}