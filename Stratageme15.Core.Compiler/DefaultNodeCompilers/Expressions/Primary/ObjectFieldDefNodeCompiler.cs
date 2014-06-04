using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class ObjectFieldDefNodeCompiler  : NodeCompilerBase<ObjectFieldDef>
    {
        protected override void DoCompile(TextWriter output, ObjectFieldDef node)
        {
            CompileChild(output,(SyntaxTreeNodeBase)node.Key);
            output.Write(": ");
            CompileChild(output,node.Value);
        }
    }
}
