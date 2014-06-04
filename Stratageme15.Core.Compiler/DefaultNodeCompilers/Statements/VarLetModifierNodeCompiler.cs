using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class VarLetModifierNodeCompiler : NodeCompilerBase<VarLetModifier>
    {
        protected override void DoCompile(TextWriter output, VarLetModifier node)
        {
            if (node.IsLet) output.Write("let");
            else if (node.IsVar) output.Write("var");
        }
    }
}
