using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion.Repositories;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    /// <summary>
    /// Performes access for temporary translation data.
    /// Translation context holds current class context, translated nodes, translation stack etc.
    /// </summary>
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

        /// <summary>
        /// Current translated node.
        /// All child nodes should be placed inside current translated node via TranslatedNode.CollectSymbol method
        /// To change current translated node use Push/PopTranslated or Set/RestorTranslated
        /// </summary>
        public SyntaxTreeNodeBase TranslatedNode { get; private set; }

        private SyntaxTreeNodeBase _backup;
        /// <summary>
        /// Preserves current translated node state and sets the Translated node to passed argument
        /// </summary>
        /// <param name="node">New translated node</param>
        public void SetTranslated(SyntaxTreeNodeBase node)
        {
            _backup = TranslatedNode;
            TranslatedNode = node;
        }

        /// <summary>
        /// Restores current translated node from backup after SetTranslatedNode call
        /// </summary>
        public void RestoreTranslated()
        {
            TranslatedNode = _backup;
        }

        /// <summary>
        /// Sets the current translation node
        /// Warning! This method does not perform parent node collection. 
        /// It only sets parent node for passed argument
        /// </summary>
        /// <param name="node">New current translation node</param>
        public void PushTranslated(SyntaxTreeNodeBase node)
        {
            node.Parent = TranslatedNode;
            TranslatedNode = node;
        }


        public void PopTranslated()
        {
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