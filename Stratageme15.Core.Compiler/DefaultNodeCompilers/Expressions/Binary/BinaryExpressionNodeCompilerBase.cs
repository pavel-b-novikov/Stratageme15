using System;
using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Binary
{
    public abstract class BinaryExpressionNodeCompilerBase<TBinary> : NodeCompilerBase<TBinary> where TBinary : SyntaxTreeNodeBase,IBinaryExpression
    {
        protected override void DoCompile(TextWriter output, TBinary node)
        {
            CompileChild(output,node.LeftPart);
            output.Write(" ");
            Operation(output,node);
            output.Write(" ");
            CompileChild(output,node.RightPart);
        }

        protected abstract void Operation(TextWriter output, TBinary node);
    }
    
    public class AssignmentBinaryExpressionNodeCompiler : BinaryExpressionNodeCompilerBase<AssignmentBinaryExpression>
    {
        protected override void Operation(TextWriter output, AssignmentBinaryExpression node)
        {
            output.Write(OperatorConverter.OperatorString(node.Operator));
        }
    }

    public class BitwiseBinaryExpressionNodeCompiler : BinaryExpressionNodeCompilerBase<BitwiseBinaryExpression>
    {
        protected override void Operation(TextWriter output, BitwiseBinaryExpression node)
        {
            output.Write(OperatorConverter.OperatorString(node.Operator));
        }
    }

    public class ComparisonBinaryExpressionNodeCompiler : BinaryExpressionNodeCompilerBase<ComparisonBinaryExpression>
    {
        protected override void Operation(TextWriter output, ComparisonBinaryExpression node)
        {
            output.Write(OperatorConverter.OperatorString(node.Operator));
        }
    }

    public class LogicalBinaryExpressionNodeCompiler : BinaryExpressionNodeCompilerBase<LogicalBinaryExpression>
    {
        protected override void Operation(TextWriter output, LogicalBinaryExpression node)
        {
            output.Write(OperatorConverter.OperatorString(node.Operator));
        }
    }

    public class MathBinaryExpressionNodeCompiler : BinaryExpressionNodeCompilerBase<MathBinaryExpression>
    {
        protected override void Operation(TextWriter output, MathBinaryExpression node)
        {
            output.Write(OperatorConverter.OperatorString(node.Operator));
        }
    }

}
