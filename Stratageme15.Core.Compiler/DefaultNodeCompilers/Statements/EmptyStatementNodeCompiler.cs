using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class EmptyStatementNodeCompiler : StatementNodeCompilerBase<EmptyStatement>
    {
        protected override void CompileStatement(TextWriter output, EmptyStatement node)
        {
           // output.Write(";");
        }
    }
}
