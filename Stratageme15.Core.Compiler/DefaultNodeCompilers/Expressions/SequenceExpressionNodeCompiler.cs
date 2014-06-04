using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions
{
    public class SequenceExpressionNodeCompiler : ExpressionNodeCompilerBase<SequenceExpression>
    {
        protected override void CompileExpression(TextWriter output, SequenceExpression node)
        {
            for (int index = 0; index < node.Sequence.Count; index++)
            {
                var expression = node.Sequence[index];
                CompileChild(output, expression);
                if (index < node.Sequence.Count - 1) output.Write(",");
            }
        }
    }
}
