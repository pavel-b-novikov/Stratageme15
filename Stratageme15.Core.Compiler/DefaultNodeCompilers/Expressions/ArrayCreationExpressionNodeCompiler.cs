using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions
{
    public class ArrayCreationExpressionNodeCompiler : ExpressionNodeCompilerBase<ArrayCreationExpression>
    {
        protected override void CompileExpression(TextWriter output, ArrayCreationExpression node)
        {
            output.Write("[");
            bool isFirst = true;
            foreach (var arrayElement in node.ArrayElements)
            {
                if (!isFirst)
                {
                    output.Write(", ");
                }
                isFirst = false;
                CompileChild(output,arrayElement);
                
            }
            output.Write("]");
        }
    }
}