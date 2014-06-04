using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class VariableDefStatementNodeCompiler : StatementNodeCompilerBase<VariableDefStatement>
    {
        protected override void CompileStatement(TextWriter output, VariableDefStatement node)
        {
            output.Write("var ");
            for (int index = 0; index < node.Variables.Count; index++)
            {
                var variable = node.Variables[index];
                CompileChild(output,variable.Item1);
                if (variable.Item3!=null)
                {
                    output.Write(" ");
                    output.Write(OperatorConverter.OperatorString(variable.Item2));
                    output.Write(" ");
                    CompileChild(output,variable.Item3);
                }
                if (index<node.Variables.Count-1)
                {
                    output.Write(",");
                    if (variable.Item3 != null)
                    {
                        NewLine(output);
                    }
                }
                
            }
        }
    }
}