using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class UseStrictNodeCompiler : NodeCompilerBase<UseStrict>
    {
        protected override void DoCompile(TextWriter output, UseStrict node)
        {
            output.WriteLine("'use strict';");
        }
    }
}
