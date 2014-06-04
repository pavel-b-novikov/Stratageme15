using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Transaltion.Builders.Function
{
    public class FunctionDefExpressionBuilder : ISyntaxBuilder<FunctionDefExpression>
    {
        private string _fname;
        private LinkedList<string> _argumentNames;
        private CodeBlock _code;
        private FunctionDefExpressionBuilder(string fname)
        {
            _code = null;
            _fname = fname;
            _argumentNames = new LinkedList<string>();
        }

        private FunctionDefExpressionBuilder()
        {
            
        }

        public static FunctionDefExpressionBuilder Function()
        {
            return new FunctionDefExpressionBuilder();
        }

        public static FunctionDefExpressionBuilder Function(string name)
        {
            return new FunctionDefExpressionBuilder(name);
        }

        public FunctionDefExpressionBuilder Argument(string argName)
        {
            _argumentNames.AddLast(argName);
            return this;
        }

        public FunctionDefExpressionBuilder Code(CodeBlock code)
        {
            _code = code;
            return this;
        }

        public FunctionDefExpression Build()
        {
            FunctionDefExpression fde = new FunctionDefExpression();
            if (_fname!=null) fde.Name = _fname.Ident();
            fde.Parameters = _argumentNames.FormalParameters();
            if (_code == null)
            {
                fde.Code = new CodeBlock();
            }
            else
            {
                fde.Code = _code;
            }
            return fde;
        }
    }
}
