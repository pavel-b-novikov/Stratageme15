using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class FinallyClauseNodeCompiler : NodeCompilerBase<FinallyClause>
    {
        protected override void DoCompile(TextWriter output, FinallyClause node)
        {
            output.Write("finally ");
            CompileChild(output,node.FinallyBlock);
        }
    }
}
