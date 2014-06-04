using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class BreakStatementNodeCompiler : StatementNodeCompilerBase<BreakStatement>
    {
        protected override void CompileStatement(TextWriter output, BreakStatement node)
        {
            output.Write("break");
            if (node.BreakLabel!=null) CompileChild(output,node.BreakLabel);
        }
    }
}