using System.IO;
using Stratageme15.Core.Compiler.Exceptions;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class ForStatementNodeCompiler : StatementNodeCompilerBase<ForStatement>
    {
        protected override void CompileStatement(TextWriter output, ForStatement node)
        {
            output.Write("for (");
            if (node.InitializationStatement!=null)
            {
                CompileChild(output,node.InitializationStatement);
                Scolon(output);
                output.Write(" ");
            }
            else if (node.InitializationExpression!=null)
            {
                CompileChild(output,node.InitializationExpression);
                Scolon(output);
                output.Write(" ");
            }
            else throw new MalformedSyntaxTreeException(node,"Initialization statement/expression");
            
            CompileChild(output,node.ComparisonExpression);
            Scolon(output);
            output.Write(" ");
            CompileChild(output,node.IncrementExpression);
            output.Write(") ");
            CompileChild(output,node.CodeBlock);
        }
    }
}