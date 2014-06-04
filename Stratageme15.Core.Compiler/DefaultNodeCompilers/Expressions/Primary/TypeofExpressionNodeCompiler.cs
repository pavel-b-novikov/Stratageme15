using System.Collections.Generic;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class TypeofExpressionNodeCompiler : ExpressionNodeCompilerBase<TypeofExpression>
    {
        protected override void CompileExpression(TextWriter output, TypeofExpression node)
        {
            output.Write("typeof (");
            CompileChild(output,node.Expression);
            output.Write(")");
        }
    }
}