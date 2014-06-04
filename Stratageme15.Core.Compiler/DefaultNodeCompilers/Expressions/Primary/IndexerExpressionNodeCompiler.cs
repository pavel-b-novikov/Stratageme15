using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class IndexerExpressionNodeCompiler : NodeCompilerBase<IndexerExpression>
    {
        protected override void DoCompile(TextWriter output, IndexerExpression node)
        {
            CompileChild(output,node.AccessedExpression);
            if (node.Index == null)
            {
                output.Write("[]");
            }
            else
            {
                CompileChild(output, node.Index);
            }
        }
    }
}