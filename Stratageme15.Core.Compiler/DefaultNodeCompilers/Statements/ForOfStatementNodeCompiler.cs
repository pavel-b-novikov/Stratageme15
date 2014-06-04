using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class ForOfStatementNodeCompiler : StatementNodeCompilerBase<ForOfStatement>
    {
        protected override void CompileStatement(TextWriter output, ForOfStatement node)
        {
            output.Write("for (");
            CompileChild(output, node.IteratorVariable.Modifier);
            CompileChild(output, node.IteratorVariable.Identifier);
            InOrOf(output);
            CompileChild(output, node.IterationExpression);
            CompileChild(output, node.CodeBlock);
        }

        protected void InOrOf(TextWriter output)
        {
            output.Write(" of ");
        }
    }
}
