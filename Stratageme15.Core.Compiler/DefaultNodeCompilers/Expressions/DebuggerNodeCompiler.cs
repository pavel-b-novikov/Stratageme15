using System;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions
{

    public class DebuggerNodeCompiler : NodeCompilerBase<DebuggerStatement>
    {
        protected override void DoCompile(TextWriter output, DebuggerStatement node)
        {
            output.Write("debugger");
        }
    }
}
