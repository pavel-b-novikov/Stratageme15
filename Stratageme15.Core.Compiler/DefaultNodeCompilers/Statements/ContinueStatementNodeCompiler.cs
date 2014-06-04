using System;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class ContinueStatementNodeCompiler : NodeCompilerBase<ContinueStatement>
    {
        protected override void DoCompile(TextWriter output, ContinueStatement node)
        {
            output.Write("continue ");
            if (node.ContinueLabel != null)
            {
                CompileChild(output,node.ContinueLabel);
            }
        }
    }
}