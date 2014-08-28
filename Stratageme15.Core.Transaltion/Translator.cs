using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.Repositories;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation
{
    public class Translator
    {
        private readonly AssemblyRepository _assemblyRepository;
        private readonly ITranslationLogger _logger;
        private readonly ReactorRepository _reactorRepository;
        private readonly Stack<SyntaxNode> _translationStack;
        private readonly Stack<ChildrenTraversalPromise> _traversalPromises;

        private ChildrenTraversalPromise _currentPromise;
        private JsProgram _root;
        private TranslationContext _translationContext;

        public Translator(ReactorRepository reactorRepository, AssemblyRepository assemblyRepository,
                          ITranslationLogger logger)
        {
            _reactorRepository = reactorRepository;
            _assemblyRepository = assemblyRepository;
            _logger = logger;
            _translationStack = new Stack<SyntaxNode>();
            _traversalPromises = new Stack<ChildrenTraversalPromise>();
        }

        public string FileNameBeingTranslated { get; set; }

        private void HandleTraversalPromises(int stackCount)
        {
            if (_currentPromise == null) return;
            if (_currentPromise.StackPosition != stackCount) return;
            _translationContext.Debug("Resolving traversal promise on position {0} from reactor {1}", stackCount,
                                      _currentPromise.Promisee.GetType());
            _currentPromise.Promisee.OnPromise(_translationContext, _currentPromise.OriginalNode);
            _currentPromise = null;
            if (_traversalPromises.Count > 0) _currentPromise = _traversalPromises.Pop();
            HandleTraversalPromises(stackCount);
        }

        private void Promise(IReactor reactor, int currentStackpos, SyntaxNode cnode)
        {
            _traversalPromises.Push(_currentPromise);
            _currentPromise = new ChildrenTraversalPromise(currentStackpos, reactor, cnode);
        }

        public JsProgram Translate(SyntaxTree synTree)
        {
            _root = new JsProgram();
            SyntaxNode root = synTree.GetRoot();

            _translationStack.Clear();
            _translationStack.Push(root);
            _traversalPromises.Clear();
            _translationContext = new TranslationContext(_reactorRepository, _assemblyRepository, _root,
                                                         _translationStack, _logger, FileNameBeingTranslated, synTree);
            _currentPromise = null;

            while (_translationStack.Count > 0)
            {
                HandleTraversalPromises(_translationStack.Count);

                SyntaxNode cnode = _translationStack.Pop();
                _translationContext.SourceNode = cnode;

                IEnumerable<IReactor> reactors = _reactorRepository.GetAllSituationOrDefault(_translationContext);

                //IReactor reactor = null;
                //if (reactors.Any()) reactor = reactors.First();

                var strategy = TranslationStrategy.TraverseChildren;
                ;
                bool manualStack = false;
                int skipChildren = 0;

                foreach (IReactor rr in reactors)
                {
                    IReactor reactor = rr;
                    TranslationResult result = reactor.OnNode(_translationContext);
                    if (result.FallDown)
                    {
                        _translationContext.Debug("Falling down from reactor {0}", rr.GetType());
                        continue;
                    }

                    if (!result.IsTranslationStrategySet)
                    {
                        string msg =
                            string.Format(
                                "Reactor {0} didnt set Translation strategy. Stopping translation process due to possible translation errors",
                                reactor);
                        _translationContext.Crit(msg);
                        return _root;
                    }
                    strategy = result.Strategy;
                    skipChildren = result.SkipChildrenCount;
                    manualStack = result.IsStackManuallyFormed;
                    if (!manualStack)
                    {
                        result.StackCountBeforeManualPush = _translationStack.Count;
                    }
                    else
                    {
                        _translationContext.Debug("Reactor {0} has manually formed further stack", rr.GetType());
                    }
                    if (strategy == TranslationStrategy.TraverseChildrenAndNotifyMe)
                    {
                        _translationContext.Debug("Promise request from reactor {0}", rr.GetType());
                        Promise(reactor, result.StackCountBeforeManualPush, cnode);
                    }

                    break;
                }

                if ((strategy == TranslationStrategy.TraverseChildrenAndNotifyMe ||
                     strategy == TranslationStrategy.TraverseChildren) && !manualStack)
                {
                    IEnumerable<SyntaxNode> children = cnode.ChildNodes().Skip(skipChildren).Reverse();
                    bool any = false;
                    foreach (SyntaxNode source in children)
                    {
                        _translationStack.Push(source);
                        any = true;
                    }
                    if (!any)
                    {
                        HandleTraversalPromises(_translationStack.Count);
                    }
                }
            }
            HandleTraversalPromises(_translationStack.Count);
            return _root;
        }
    }
}