using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class TryCatchFinallyStatementNodeCompiler : StatementNodeCompilerBase<TryCatchFinallyStatement>
    {
        protected override void CompileStatement(TextWriter output, TryCatchFinallyStatement node)
        {
            output.WriteLine("try ");
            CompileChild(output,node.TryBlock);
            NewLine(output);
            CompileChild(output,node.CatchClause);
            NewLine(output);
            CompileChild(output,node.FinallyBlock);
        }
    }
}