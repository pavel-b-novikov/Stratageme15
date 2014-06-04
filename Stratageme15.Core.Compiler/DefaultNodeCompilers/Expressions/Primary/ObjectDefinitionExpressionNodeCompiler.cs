using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class ObjectDefinitionExpressionNodeCompiler : ExpressionNodeCompilerBase<ObjectDefinitionExpression>
    {
        protected override void CompileExpression(TextWriter output, ObjectDefinitionExpression node)
        {
            if (node.ObjectFields.Count==0)
            {
                output.Write("{ }"); 
                return;
            }
            
            output.Write("{");
            IncTab(output);
            NewLine(output);
            for (int index = 0; index < node.ObjectFields.Count; index++)
            {
                var fld = node.ObjectFields[index];
                CompileChild(output,fld);
                if (index < node.ObjectFields.Count - 1)
                {
                    output.Write(",");
                    NewLine(output);
                }

            }
            DecTab(output);
            NewLine(output);
            output.Write("}");
        }
    }
}