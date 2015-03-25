using System.Collections.Generic;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Contexts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Stratageme15.Reactors.Basic.Utility
{
    /// <summary>
    /// Interopability for translation context
    /// </summary>
    public class TranslationContextWrapper
    {
        public void Info(string message, params object[] args)
        {
            _context.Info(message, args);
        }

        /// <summary>
        /// Perform debug message
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public void Debug(string message, params object[] args)
        {
#if DEBUG
            _context.Debug(message, args);
#endif
        }

        /// <summary>
        /// Perform translation warning
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public void Warn(string message, params object[] args)
        {
            _context.Warn(message, args);
        }

        /// <summary>
        /// Perform translation error (user is responsible)
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public void Error(string message, params object[] args)
        {
            _context.Error(message, args);
        }

        /// <summary>
        /// Perform internal translation error (translator is responsible)
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public void Crit(string message, params object[] args)
        {
            _context.Crit(message, args);
        }

        private readonly TranslationContext _context;

        public TranslationContext Context { get { return _context; } }

        /// <summary>
        /// Reference to Compilation object
        /// </summary>
        public CSharpCompilation Compilation { get { return _context.Compilation; } }

        /// <summary>
        /// Semantic model of code being translated
        /// </summary>
        public SemanticModel SemanticModel { get { return _context.SemanticModel; } }

        /// <summary>
        /// Class-scope polymorphism controller
        /// </summary>
        public Polymorphism Polymorphism { get; private set; }

        public TranslationContextWrapper(TranslationContext context)
        {
            _context = context;
            NamespaceSymbol = context.SemanticModel.Compilation.GlobalNamespace;
            Polymorphism = new Polymorphism(this);
        }

        private const string NamespaceKey = "__Namespace";
        private string _namespaceCache = null;
        /// <summary>
        /// Current namespace specified by namespace directive
        /// </summary>
        public string Namespace
        {
            get
            {
                if (_namespaceCache == null) _namespaceCache = _context.GetCustomVariable<string>(NamespaceKey);
                return _namespaceCache;
            }
            set
            {
                _namespaceCache = value;
                _context.SetCustomVariable(value, NamespaceKey);
            }
        }

        //private SourceNamespaceSymbol
        private const string UsingsKey = "__Usings";
        private List<string> _usingsCache = null;
        /// <summary>
        /// Root program usings specified outside of currant namespace
        /// </summary>
        public List<string> Usings
        {
            get
            {
                if (_usingsCache == null) _usingsCache = _context.GetOrCreateCustomVariable<List<string>>(UsingsKey);
                return _usingsCache;
            }
            private set
            {
                _usingsCache = value;
                _context.SetCustomVariable(value, UsingsKey);
            }

        }

        private const string NamespaceSymbolKey = "__NamespaceSymbol";
        public INamespaceSymbol _namespaceSymbolCache = null;
        public INamespaceSymbol NamespaceSymbol
        {
            get
            {
                if (_namespaceSymbolCache == null) _namespaceSymbolCache = _context.GetCustomVariable<INamespaceSymbol>(NamespaceSymbolKey);
                return _namespaceSymbolCache;
            }

            set
            {
                _namespaceSymbolCache = value;
                _context.SetCustomVariable(value, NamespaceSymbolKey);
            }
        }

        private const string NamespaceUsingsKey = "__NamespaceUsings";
        private List<string> _namespaceUseingsCache = null;
        /// <summary>
        /// Namespace usings specified inside of current namespace
        /// </summary>
        public List<string> NamespaceUsings
        {
            get
            {
                if (_namespaceUseingsCache == null) _namespaceUseingsCache = _context.GetOrCreateCustomVariable<List<string>>(NamespaceUsingsKey);
                return _namespaceUseingsCache;
            }
            private set
            {
                _namespaceUseingsCache = value;
                _context.SetCustomVariable(value, NamespaceUsingsKey);
            }
        }

        private const string CurrentClassContextKey = "__ClassContext";
        private ClassTranslationContext _classTranslationContextCache;
        /// <summary>
        /// Current class translation context
        /// </summary>
        public ClassTranslationContext CurrentClassContext
        {
            get
            {
                if (_classTranslationContextCache == null) _classTranslationContextCache = _context.GetCustomVariable<ClassTranslationContext>(CurrentClassContextKey);
                return _classTranslationContextCache;
            }
            set
            {
                _classTranslationContextCache = value;
                _context.SetCustomVariable(value, CurrentClassContextKey);
            }
        }

        //public string CurrentTypeName
        //{
        //    get { return _currentTypeName; }
        //}

        //public void PushClass(ClassTranslationContext ctc)
        //{
        //    _currentTypeName = ctc.Type.Name;
        //    if (_classContextsStack.Count > 0) ctc.Parent = _classContextsStack.Peek();
        //    _classContextsStack.Push(ctc);
        //}

        //public void PopClass()
        //{
        //    _classContextsStack.Pop();
        //    _currentTypeName = null;
        //    if (_classContextsStack.Count > 0)
        //    {
        //        _currentTypeName = _classContextsStack.Peek().Type.Name;
        //    }
        //}

        public ITypeSymbol LookupTypeSymbol(TypeSyntax typeSyntax)
        {
            return null;
        }
    }
}
