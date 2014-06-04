using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class CallStatementNodeCompiler : StatementNodeCompilerBase<CallStatement>
    {
        protected override void CompileStatement(TextWriter output, CallStatement node)
        {
            CompileChild(output,node.CallExpression);
        }
    }
}