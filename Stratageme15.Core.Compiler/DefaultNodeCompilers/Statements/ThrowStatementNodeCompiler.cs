using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class ThrowStatementNodeCompiler : StatementNodeCompilerBase<ThrowStatement>
    {
        protected override void CompileStatement(TextWriter output, ThrowStatement node)
        {
            output.Write("throw ");
            CompileChild(output,node.ThrowExpression);
        }
    }
}
