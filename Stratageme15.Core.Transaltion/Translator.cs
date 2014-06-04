using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.Repositories;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion
{
    public class Translator
    {
        private readonly ReactorRepository _reactorRepository;
        private readonly AssemblyRepository _assemblyRepository;
        private JsProgram _root;
        private readonly Stack<SyntaxNode> _translationStack;
        private readonly Stack<ChildrenTraversalPromise> _traversalPromises;
        private ChildrenTraversalPromise _currentPromise;
        private TranslationContext _translationContext;
        public Translator(ReactorRepository reactorRepository, AssemblyRepository assemblyRepository)
        {
            _reactorRepository = reactorRepository;
            _assemblyRepository = assemblyRepository;
            _translationStack = new Stack<SyntaxNode>();
            _traversalPromises = new Stack<ChildrenTraversalPromise>();
        }

        private void HandleTraversalPromises(int stackCount)
        {
            if (_currentPromise==null) return;
            if (_currentPromise.StackPosition != stackCount) return;
            _currentPromise.Promisee.OnPromise(_translationContext,_currentPromise.OriginalNode);
            _currentPromise = null;
            if (_traversalPromises.Count > 0) _currentPromise = _traversalPromises.Pop();
            HandleTraversalPromises(stackCount);
        }

        private void Promise(IReactor reactor, int currentStackpos, SyntaxNode cnode)
        {
            _traversalPromises.Push(_currentPromise);
            _currentPromise = new ChildrenTraversalPromise(currentStackpos,reactor,cnode);
        }

        public JsProgram Translate(SyntaxTree synTree)
        {
            _root = new JsProgram();
            var root = synTree.GetRoot();
            
            _translationStack.Clear();
            _translationStack.Push(root);
            _traversalPromises.Clear();
            _translationContext = new TranslationContext(_reactorRepository, _assemblyRepository, _root,_translationStack);
            _currentPromise = null;

            while (_translationStack.Count > 0)
            {
                HandleTraversalPromises(_translationStack.Count);

                var cnode = _translationStack.Pop();
                _translationContext.SourceNode = cnode;

                var reactor = _reactorRepository.GetSituationOrDefault(_translationContext);

                TranslationStrategy strategy;
                bool manualStack = false;
                int skipChildren = 0;
                if (reactor != null)
                {
                    var result = reactor.OnNode(_translationContext);
                    if (!result.IsTranslationStrategySet) throw new Exception(string.Format("Reactor {0} didnt set Translation strategy. Possible translation errors",reactor));
                    strategy = result.Strategy;
                    skipChildren = result.SkipChildrenCount;
                    manualStack = result.IsStackManuallyFormed;
                    if (!manualStack)
                    {
                        result.StackCountBeforeManualPush = _translationStack.Count;
                    }
                    if (strategy == TranslationStrategy.TraverseChildrenAndNotifyMe)
                    {
                        Promise(reactor, result.StackCountBeforeManualPush,cnode);
                    }

                }else
                {
                    strategy = TranslationStrategy.TraverseChildren;
                }

                if ((strategy == TranslationStrategy.TraverseChildrenAndNotifyMe || strategy == TranslationStrategy.TraverseChildren) && !manualStack)
                {
                    //very dirty hack
                    IEnumerable<SyntaxNode> children;
                    if (cnode is ClassDeclarationSyntax)
                    {
                        children = cnode.ChildNodes().Skip(skipChildren).OrderByDescending(c => c is ConstructorDeclarationSyntax).Reverse();
                    }
                    else
                    {
                        children = cnode.ChildNodes().Skip(skipChildren).Reverse();
                    }

                    bool any = false;
                    foreach (var source in children)
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
