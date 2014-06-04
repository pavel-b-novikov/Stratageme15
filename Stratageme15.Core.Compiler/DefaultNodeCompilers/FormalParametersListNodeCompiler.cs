using System;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers
{
   
    public class FormalParametersListNodeCompiler : NodeCompilerBase<FormalParametersList>
    {
        protected override void DoCompile(TextWriter output, FormalParametersList node)
        {
            output.Write("(");
            for (int index = 0; index < node.ArgumentNames.Count; index++)
            {
                var expression = node.ArgumentNames[index];
                CompileChild(output, expression);
                if (index < node.ArgumentNames.Count - 1) output.Write(", ");
            }
            output.Write(")");
        }
    }
}