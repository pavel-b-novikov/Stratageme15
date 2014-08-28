using System;
using System.Collections;
using System.Text;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Tools.JavascriptParser
{
    public partial class Parser
    {
        private NodeInfoTree Tree { get; set; }

        public Parser()
        {
            Tree = new NodeInfoTree(new NodeInfo(0, 0, typeof(JsProgram)));
        }

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            errors = new Errors();
            Tree = new NodeInfoTree(new NodeInfo(0, 0, typeof(JsProgram)));
        }


        #region Tree helpers

        private StatementLabel _label = null;
        public void Label()
        {
            _label = new StatementLabel(t.val);
        }

        public void Ident()
        {
            TerminalArg<IdentExpression>();
        }
        public void Clarify<T>()
        {
            Tree.Clarify<T>();
        }
        public void Terminal<T>() where T : SyntaxTreeNodeBase
        {
            Tree.Terminal<T>();
        }

        public void TerminalArg<T>() where T : SyntaxTreeNodeBase
        {
            Tree.Terminal<T>(t.val);
        }

        public void Push<T>()
        {
            Tree.Push<T>(t.line, t.col);
            if (typeof(IStatement).IsAssignableFrom(Tree.CurrentNode.NodeType))
            {
                if (_label != null)
                {
                    Tree.CurrentNode.AddTerminal(_label);
                    _label = null;
                }
            }
        }

        public void PushPop<T>()
        {
            Tree.Push<T>(t.line, t.col);
            Tree.Pop();
        }

        public void Pop()
        {
            Tree.Pop();
        }

        public void Converge<TNode>()
        {
            Tree.Converge<TNode>(t.line, t.col);
        }

        #region Operators
        public void Assignment()
        {
            Tree.Operator(OperatorConverter.GetAssignmentOperator(t.val));
        }
        public void UnaryOp()
        {
            Tree.Operator(OperatorConverter.GetUnaryOperator(t.val));
        }
        public void Math()
        {
            Tree.Operator(OperatorConverter.GetMathOperator(t.val));
        }

        public void Bitwise()
        {
            Tree.Operator(OperatorConverter.GetBitwiseOperator(t.val));
        }

        public void Comparison()
        {
            Tree.Operator(OperatorConverter.GetComparisonOperator(t.val));
        }

        public void Logical()
        {
            Tree.Operator(OperatorConverter.GetLogicalOperator(t.val));
        }

        public void IncDec()
        {
            Tree.Operator(OperatorConverter.GetIndrementDecrementOperator(t.val));
        }
        #endregion

        public void PopDrop()
        {
            Tree.PopDropIfEmpty();
        }
        #endregion

        #region LL(1) resolvers

        bool IsForin()
        {
            var pk = Peek(2);
            var pk1 = Peek(1);
            return (pk.kind == _in || pk1.kind == _in);
        }

        bool IsForof()
        {
            var pk = Peek(2);
            var pk1 = Peek(1);
            return (pk.kind == _of || pk1.kind == _of);
        }

        bool IsParenthesedSequence()
        {
            if (la.kind != _lpar) return false;
            int parCount = 0;
            int brackCount = 0;
            int n = 1;
            Token peek = Peek(n);
            do
            {
                if (peek.kind == _EOF) return false;

                if (peek.kind == _lbrack) brackCount++;
                if (peek.kind == _rbrack) brackCount--;
                if (peek.kind == _rpar) parCount--;
                if (peek.kind == _lpar) parCount++;

                if (peek.kind == _comma && brackCount == 0 && parCount==0) return true;
                n++;
                peek = Peek(n);
            } while (parCount >= 0);

            return false;

        }
        
        void EatLabel()
        {
            if (la.kind == _ident)
            {
                var tk = Peek(1);
                if (tk.kind == _colon)
                {
                    Get();
                    Label();
                    Get();
                }
            }
        }
        void NeedScolon()
        {
            if (la.kind == _scolon)
            {
                Tree.CurrentNode.IsScolonNeededAsTheEnd = true;
            }
        }

        bool IsObjectDef()
        {
            if (la.kind != _lbrace) return false;
            var tk = Peek(1);
            if (!keywordOrIdent[tk.kind]&&!nonRegexLiterals[tk.kind]) return false;
            tk = Peek(2);
            return tk.kind == _colon;
        }
       
        bool IsElseIf()
        {
            if (la.kind==_else)
            {
                var tk = Peek(1);
                return tk.kind == _if;
            }
            return false;
        }
        bool IsAssignment()
        {
            var tk = la;
            if (tk.kind==_regexL)
            {
                tk = scanner.WarrantyReturnRegexl(tk);
            }

            return assgnOps[tk.kind];
        }
        /// <summary>
        /// Eliminate redundant (in javascript) scolon
        /// </summary>
        void EatColon()
        {
            if (la.kind == _scolon) Get();
        }

        void FixUpRegexLookahead()
        {
            if (la.kind == _regexL)
            {
                la = scanner.WarrantyReturnRegexl(la);
            }
        }
        #endregion

        

        private const int maxTerminals = 160;
        private static BitArray keywordOrIdent = NewSet(new int[] { _ident, _break, _case, _catch, _continue, _debugger, _default, _delete, _do, _else, _finally, _for, _function, _if, _in, _instanceof, _new, _of, _return, _switch, _this, _throw, _try, _typeof, _var, _void, _while, _with });
        private static BitArray nonRegexLiterals = NewSet(new int[] { _numberL, _stringL, _ident, _trueW, _falseW });
        private static BitArray assgnOps = NewSet(new int[] { _assgn, _aplus, _aminus, _atimes, _adiv, _amod,_aband,_abor,_abxor,_alsh,_arsh,_arush });
        private static BitArray NewSet(int[] values)
        {
            BitArray a = new BitArray(maxTerminals);
            foreach (int x in values) a[x] = true;
            return a;
        }

    }
}
