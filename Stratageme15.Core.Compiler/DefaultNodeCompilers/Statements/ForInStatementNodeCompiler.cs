using System;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class ForInStatementNodeCompiler : StatementNodeCompilerBase<ForInStatement>
    {
        protected override void CompileStatement(TextWriter output, ForInStatement node)
        {
            output.Write("for (");
            CompileChild(output,node.IteratorVariable.Modifier);
            CompileChild(output,node.IteratorVariable.Identifier);
            InOrOf(output);
            CompileChild(output,node.IterationExpression);
            CompileChild(output,node.CodeBlock);
        }

        protected void InOrOf(TextWriter output)
        {
            output.Write(" in ");
        }
    }
}