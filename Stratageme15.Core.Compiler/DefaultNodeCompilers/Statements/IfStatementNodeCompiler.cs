using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class IfStatementNodeCompiler : StatementNodeCompilerBase<IfStatement>
    {
        protected override void CompileStatement(TextWriter output, IfStatement node)
        {
            
            output.Write("if ");
            CompileChild(output,node.Condition);
            output.Write(" ");
            CompileChild(output,node.TrueCodeBlock);
            foreach (var elseIf in node.ElseIfs)
            {
                NewLine(output);
                output.Write("else if ");
                CompileChild(output,elseIf.Item1);
                output.Write(" ");
                CompileChild(output, elseIf.Item2);
            }
            if (node.ElseCodeBlock != null)
            {
                NewLine(output);
                output.Write("else ");
                CompileChild(output, node.ElseCodeBlock);
            }
            NewLine(output);
        }
    }
}