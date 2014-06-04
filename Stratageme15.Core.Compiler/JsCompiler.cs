using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler
{
    public class JsCompiler
    {
        private JsProgram _program;
        private NodeCompilersRepository _nodeCompilers;

        public JsCompiler(JsProgram program, NodeCompilersRepository nodeCompilers)
        {
            _program = program;
            _nodeCompilers = nodeCompilers;
        }

        public void Compile(Stream s)
        {
            using (TextWriter tw = new StreamWriter(s))
            {
                Compile(tw);
            }
        }

        public void Compile(TextWriter tw)
        {
            foreach (var rootCodeElement in _program.CodeElements)
            {
                var compiler = _nodeCompilers.Get(rootCodeElement.GetType());
                
                compiler.Compile(tw, (SyntaxTreeNodeBase)rootCodeElement);
                if (!(rootCodeElement is FunctionDefExpression))
                {
                    tw.Write(";");
                }
                tw.WriteLine();
                tw.WriteLine();
            }
        }
    }
}
