using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Repositories;

namespace Stratageme15.Core.Translation.TranslationContexts
{
    /// <summary>
    /// Performes access for temporary translation data.
    /// Translation context holds current class context, translated nodes, translation stack etc.
    /// </summary>
    public class TranslationContext
    {
        private readonly ITranslationLogger _logger;
        private SyntaxTreeNodeBase _backup;

        /// <summary>
        /// Constructs new translation context.
        /// Translation context exists in single instance during translation process.
        /// Its fields are modified during translation process.
        /// </summary>
        /// <param name="reactors">Node reactors repository</param>
        /// <param name="arep">Types assembly repository</param>
        /// <param name="root">Root node of target translation syntax tree</param>
        /// <param name="translationStack">Translation stack holding source syntax nodes for further translation</param>
        /// <param name="logger">Logging facility</param>
        /// <param name="fileName">Source file name. It mey be null</param>
        /// <param name="translationRoot">Root of syntax node of source program</param>
        public TranslationContext(ReactorRepository reactors, AssemblyRepository arep, JsProgram root,
                                  Stack<SyntaxNode> translationStack, ITranslationLogger logger, string fileName,
                                  SyntaxTree translationRoot)
        {
            TranslationStack = translationStack;
            _logger = logger;
            TranslationRoot = translationRoot;
            FileName = fileName;
            Root = root;
            TranslatedNode = root;
            Reactors = reactors;
            Usings = new List<string>();
            NamespaceUsings = new List<string>();
            _classContextsStack = new Stack<ClassTranslationContext>();
            Assemblies = arep;
        }

        /// <summary>
        /// The assemblies repository holding assemblies used during translation process for type lookup
        /// </summary>
        public AssemblyRepository Assemblies { get; private set; }

        /// <summary>
        /// The reactors repository holding behavioral pieces handling each source node individually
        /// </summary>
        public ReactorRepository Reactors { get; private set; }

        /// <summary>
        /// Current namespace specified by namespace directive
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Root program usings specified outside of currant namespace
        /// </summary>
        public List<string> Usings { get; private set; }

        /// <summary>
        /// Namespace usings specified inside of current namespace
        /// </summary>
        public List<string> NamespaceUsings { get; private set; }

        /// <summary>
        /// Target program syntax tree root
        /// </summary>
        public JsProgram Root { get; private set; }

        /// <summary>
        /// Translation stack holding source syntax nodes for further translation
        /// </summary>
        public Stack<SyntaxNode> TranslationStack { get; private set; }

        /// <summary>
        /// Name of source file currently being translated
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Root of syntax tree of program being translated
        /// </summary>
        public SyntaxTree TranslationRoot { get; private set; }

        /// <summary>
        /// Current translated node.
        /// All child nodes should be placed inside current translated node via TranslatedNode.CollectSymbol method
        /// To change current translated node use Push/PopTranslated or Set/RestorTranslated
        /// </summary>
        public SyntaxTreeNodeBase TranslatedNode { get; private set; }

        public SyntaxNode SourceNode { get; internal set; }

        /// <summary>
        /// The current logging facility
        /// </summary>
        public ITranslationLogger Logger
        {
            get { return _logger; }
        }

        #region Type context services

        private readonly Stack<ClassTranslationContext> _classContextsStack;


        private string _currentTypeName;

        public ClassTranslationContext CurrentClassContext
        {
            get { return _classContextsStack.Peek(); }
        }

        public string CurrentTypeName
        {
            get { return _currentTypeName; }
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

    }
}