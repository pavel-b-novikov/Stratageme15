using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Repositories;
using Microsoft.CodeAnalysis.CSharp;

namespace Stratageme15.Core.Translation.TranslationContexts
{
    /// <summary>
    /// Performes access for temporary translation data.
    /// Translation context holds current class context, translated nodes, translation stack etc.
    /// </summary>
    public class TranslationContext
    {
        private readonly ITranslationLogger _logger;
        private Stack<SyntaxTreeNodeBase> _backupStack = new Stack<SyntaxTreeNodeBase>();

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
            TargetNode = root;
            Reactors = reactors;
            Assemblies = arep;
            CreateSemanticModel();
        }

        private void CreateSemanticModel()
        {
            CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            CSharpCompilation comp = CSharpCompilation.Create("todo", new[] {TranslationRoot},
                                                              Assemblies.GetMetadataReferences(), compilationOptions);
            Compilation = comp;
            SemanticModel = comp.GetSemanticModel(TranslationRoot);
        }

        /// <summary>
        /// Reference to Compilation object
        /// </summary>
        public CSharpCompilation Compilation { get; private set; }

        /// <summary>
        /// Semantic model of code being translated
        /// </summary>
        public SemanticModel SemanticModel { get; private set; }

        /// <summary>
        /// The assemblies repository holding assemblies used during translation process for type lookup
        /// </summary>
        public AssemblyRepository Assemblies { get; private set; }

        /// <summary>
        /// The reactors repository holding behavioral pieces handling each source node individually
        /// </summary>
        public ReactorRepository Reactors { get; private set; }

        /// <summary>
        /// Custom translation context variables
        /// </summary>
        private readonly Dictionary<string, object> _customContextVars = new Dictionary<string, object>();

        public object this[string s]
        {
            get { return _customContextVars[s]; }
            set { _customContextVars[s] = value; }
        }

        public T GetCustomVariable<T>(string s)
        {
            if (!_customContextVars.ContainsKey(s)) return default(T);
            return (T)_customContextVars[s];
        }

        public T GetOrCreateCustomVariable<T>(string s) where T: new()
        {
            if (!IsCustomVariableDefined(s))
            {
                var e = new T();
                SetCustomVariable(e,s);
                return e;
            }
            return (T)_customContextVars[s];
        }

        public void SetCustomVariable<T>(T value,string s)
        {
            _customContextVars[s] = value;
        }

        public bool IsCustomVariableDefined(string s)
        {
            return _customContextVars.ContainsKey(s);
        }

        public void DropCustomVariable(string s)
        {
            _customContextVars.Remove(s);
        }

        public string Namespace { get; set; }
        

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
        public SyntaxTreeNodeBase TargetNode { get; private set; }

        public SyntaxNode SourceNode { get; internal set; }

        /// <summary>
        /// The current logging facility
        /// </summary>
        public ITranslationLogger Logger
        {
            get { return _logger; }
        }

        /// <summary>
        /// Preserves current translated node state and sets the Translated node to passed argument
        /// </summary>
        /// <param name="node">New translated node</param>
        public void PushContextNode(SyntaxTreeNodeBase node)
        {
            _backupStack.Push(TargetNode);
            TargetNode = node;
        }

        /// <summary>
        /// Restores current translated node from backup after SetTranslatedNode call
        /// </summary>
        public void PopContextNode()
        {
            TargetNode = _backupStack.Pop();
        }

        /// <summary>
        /// Sets the current translation node
        /// Warning! This method does not perform parent node collection. 
        /// It only sets parent node for passed argument
        /// </summary>
        /// <param name="node">New current translation node</param>
        public void PushTranslated(SyntaxTreeNodeBase node)
        {
            node.Parent = TargetNode;
            TargetNode = node;
        }


        public void PopTranslated()
        {
            TargetNode = TargetNode.Parent;
        }

    }
}