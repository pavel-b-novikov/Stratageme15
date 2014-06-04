using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    class IndexExpressionNodeCompiler : NodeCompilerBase<IndexExpression>
    {
        protected override void DoCompile(TextWriter output, IndexExpression node)
        {
            if (node.Index==null)
            {
                output.Write("[]");
                return;
            }

            output.Write("[");
            CompileChild(output, node.Index);
            output.Write("]");
        }
    }
}
