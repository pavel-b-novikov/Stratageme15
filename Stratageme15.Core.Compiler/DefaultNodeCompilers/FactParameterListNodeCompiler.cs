using System;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers
{
    public class FactParameterListNodeCompiler : NodeCompilerBase<FactParameterList>
    {
        protected override void DoCompile(TextWriter output, FactParameterList node)
        {
            output.Write("(");
            if (node != null)
            {
                for (int index = 0; index < node.Arguments.Count; index++)
                {
                    var expression = node.Arguments[index];
                    CompileChild(output, expression);
                    if (index < node.Arguments.Count - 1) output.Write(", ");
                }
            }
            output.Write(")");
        }
    }
}