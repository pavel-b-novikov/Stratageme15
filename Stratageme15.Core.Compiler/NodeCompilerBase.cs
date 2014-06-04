using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Stratageme15.Core.Compiler.Exceptions;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler
{
    public abstract class NodeCompilerBase<TNode> : INodeCompiler where TNode : SyntaxTreeNodeBase
    {
        private NodeCompilersRepository _repository;
        public NodeCompilersRepository Repository
        {
            get { return _repository; }
            set
            {
                if (_repository != null) throw new Exception("Repository can be set only once");
                _repository = value;
            }
        }

        public void Compile(TextWriter output, SyntaxTreeNodeBase node)
        {
            if (!(node is TNode)) throw new InvalidNodeCompiler(typeof(TNode), node.GetType());
            DoCompile(output,(TNode) node);
        }

        protected abstract void DoCompile(TextWriter output,TNode node);

        protected void IncTab(TextWriter o)
        {
            if (!(o is IndentedTextWriter)) return;
            ((IndentedTextWriter) o).Indent++;
        }

        protected void DecTab(TextWriter o)
        {
            if (!(o is IndentedTextWriter)) return;
            ((IndentedTextWriter)o).Indent--;
        }

        protected void CompileChild<TChild>(TextWriter output, TChild child) where TChild : SyntaxTreeNodeBase
        {
            var t = child==null? typeof(TChild): child.GetType();
            Repository.Get(t).Compile(output, child);
        }

        protected string CompileChildToString<TChild>(TChild child) where TChild : SyntaxTreeNodeBase
        {
            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                var t = child.GetType();
                Repository.Get(t).Compile(sw, child);    
            }
            return sb.ToString();
        }
        protected void Scolon(TextWriter output)
        {
            output.Write(";");
            //NewLine(output);
        }

        protected void NewLine(TextWriter output)
        {
            output.WriteLine();
        }

        protected void Waydown(TextWriter output, SyntaxTreeNodeBase node)
        {
            foreach (var syntaxTreeNodeBase in node.Children)
            {
                CompileChild(output,syntaxTreeNodeBase);
            }
        }
    }
}
