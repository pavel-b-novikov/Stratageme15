using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public abstract class StatementNodeCompilerBase<TStatement> : NodeCompilerBase<TStatement> where TStatement : SyntaxTreeNodeBase,IStatement
    {
        protected override void DoCompile(TextWriter output, TStatement node)
        {
            if (node.Label != null) output.Write("{0}:", node.Label.LabelName.Identifier);
            CompileStatement(output,node);
        }

        protected abstract void CompileStatement(TextWriter output, TStatement node);
    }
}
