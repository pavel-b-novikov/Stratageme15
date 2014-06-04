using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions
{
    public abstract class ExpressionNodeCompilerBase<TExpression> : NodeCompilerBase<TExpression> where TExpression : Expression
    {
        protected sealed override void DoCompile(TextWriter output, TExpression node)
        {
            CompileExpression(output,node);
        }

        protected abstract void CompileExpression(TextWriter output, TExpression node);
    }
}
