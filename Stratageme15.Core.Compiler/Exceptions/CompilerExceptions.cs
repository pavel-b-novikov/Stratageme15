using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Compiler.Exceptions
{
    public abstract class CompilerException : Exception
    {

    }

    public class NoSuitableNodeCompiler : CompilerException
    {
        private Type _tokenType;
        private readonly string _msg;
        public NoSuitableNodeCompiler(Type tokenType)
        {
            _tokenType = tokenType;
            _msg = string.Format("No suitable node compiler for node of type: {0}", _tokenType.Name);
        }

        public override string Message
        {
            get { return _msg; }
        }
    }

    public class InvalidNodeCompiler : CompilerException
    {
        private Type _compilerTokenType;
        private Type _triedToConvert;
        private readonly string _msg;
        public InvalidNodeCompiler(Type compilerTokenType, Type triedToConvert)
        {
            _compilerTokenType = compilerTokenType;
            _triedToConvert = triedToConvert;
            _msg = string.Format("Tried to compile {0} using compiler of type {1}", _triedToConvert.Name, _compilerTokenType.Name);
        }

        public override string Message
        {
            get { return _msg; }
        }
    }

    public class NodeCompilerRegistrationException : CompilerException
    {
        private Type _triedToRegister;
        private string _errorClause;

        public Type TriedToRegister
        {
            get { return _triedToRegister; }
        }

        private readonly string _msg;
        public NodeCompilerRegistrationException(Type triedToRegister, string errorClause)
        {
            _triedToRegister = triedToRegister;
            _errorClause = errorClause;
            _msg = string.Format("Error when registering node compiler {0}: {1}", _triedToRegister.Name,
                                     _errorClause);
        }

        public override string Message
        {
            get { return _msg; }
        }
    }

    public class MalformedSyntaxTreeException : CompilerException
    {
        public MalformedSyntaxTreeException(SyntaxTreeNodeBase malformedNode, string expected)
        {
            Expected = expected;
            MalformedNode = malformedNode;
            _msg = string.Format("Malformed {0}. {1} expected.", MalformedNode.GetType().Name, Expected);
        }

        public SyntaxTreeNodeBase MalformedNode { get; private set; }

        public string Expected { get; private set; }

        private readonly string _msg;
        public override string Message
        {
            get { return _msg; }
        }
    }
}
