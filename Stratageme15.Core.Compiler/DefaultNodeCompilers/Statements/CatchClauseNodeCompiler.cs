using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class CatchClauseNodeCompiler : NodeCompilerBase<CatchClause>
    {
        protected override void DoCompile(TextWriter output, CatchClause node)
        {
            output.Write("catch (");
            CompileChild(output,node.Identifier);
            output.Write(") ");
            CompileChild(output,node.Handler);
        }
    }
}
