using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion.Repositories;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    public class TranslationContext
    {
        public TranslationContext(ReactorRepository reactors, AssemblyRepository arep, JsProgram root,
                                  Stack<SyntaxNode> translationStack)
        {
            TranslationStack = translationStack;
            Root = root;
            TranslatedNode = root;
            Reactors = reactors;
            Usings = new List<string>();
            NamespaceUsings = new List<string>();
            _classContextsStack = new Stack<ClassTranslationContext>();
            Assemblies = arep;
        }

        #region Root services

        public AssemblyRepository Assemblies { get; private set; }
        public ReactorRepository Reactors { get; private set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; private set; }
        public List<string> NamespaceUsings { get; private set; }
        public JsProgram Root { get; private set; }
        public Stack<SyntaxNode> TranslationStack { get; private set; }
        #endregion

        public SyntaxTreeNodeBase PreviosTranslatedNode { get; private set; }
        public SyntaxTreeNodeBase TranslatedNode { get; private set; }
        public void PushTranslated(SyntaxTreeNodeBase node)
        {
            node.Parent = TranslatedNode;
            PreviosTranslatedNode = TranslatedNode;
            TranslatedNode = node;
        }

        public void PopTranslated()
        {
            PreviosTranslatedNode = TranslatedNode;
            TranslatedNode = TranslatedNode.Parent;
        }
        public SyntaxNode SourceNode { get; internal set; }

        #region Type context services
        private readonly Stack<ClassTranslationContext> _classContextsStack;
        public ClassTranslationContext CurrentClassContext
        {
            get { return _classContextsStack.Peek(); }
        }
        
        private string _currentTypeName;
        public string CurrentTypeName
        {
            get { return _currentTypeName; }
        }

        public TCtc GetCurrentClassContext<TCtc>() where TCtc : ClassTranslationContext
        {
            return (TCtc) _classContextsStack.Peek();
        }

        public void PushClass(ClassTranslationContext ctc)
        {
            _currentTypeName = ctc.Type.Name;
            if (_classContextsStack.Count > 0) ctc.Parent = _classContextsStack.Peek();
            _classContextsStack.Push(ctc);
        }

        public void PopClass()
        {
            _classContextsStack.Pop();
            _currentTypeName = null;
            if (_classContextsStack.Count > 0)
            {
                _currentTypeName = _classContextsStack.Peek().Type.Name;
            }
        }
        #endregion

        //todo logging for warnings and errors
    }
}