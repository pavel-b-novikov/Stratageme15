using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation
{
    /// <summary>
    /// Describes behavior of child nodes traversal
    /// </summary>
    public enum TranslationStrategy
    {
        /// <summary>
        /// Simple behavior - push all of children in translation stack and traverse
        /// </summary>
        TraverseChildren,

        /// <summary>
        /// Save the promise of calling OnAfterChildTraversal of current reactor
        /// </summary>
        TraverseChildrenAndNotifyMe,

        /// <summary>
        /// Ignore all child nodes and process to next stacked node
        /// </summary>
        DontTraverseChildren
    }

    /// <summary>
    /// The result of node translation 
    /// Should be returned after reactor process
    /// </summary>
    public class TranslationResult
    {
        private bool _isTranslationStrategySet;
        private TranslationStrategy _strategy;

        /// <summary>
        /// Constructs new translation result
        /// </summary>
        /// <param name="strategy">Translation strategy after node translation</param>
        public TranslationResult(TranslationStrategy strategy)
        {
            Strategy = strategy;
            _isTranslationStrategySet = false;
            SkipChildrenCount = 0;
            IsStackManuallyFormed = false;
            FallDown = false;
        }

        /// <summary>
        /// Is default translation strategy modified by reactor
        /// </summary>
        public bool IsTranslationStrategySet
        {
            get { return _isTranslationStrategySet; }
        }

        /// <summary>
        /// Translation strategy
        /// </summary>
        public TranslationStrategy Strategy
        {
            get { return _strategy; }
            set
            {
                _strategy = value;
                _isTranslationStrategySet = true;
            }
        }

        /// <summary>
        /// How many child nodes to skip after current node translation
        /// First N child nodes will be skip and all others will be pushed to stack for
        /// upcoming translation
        /// </summary>
        public int SkipChildrenCount { get; set; }

        /// <summary>
        /// Instructs translator not to manually form the translation stack
        /// It's cannot be set directly because we need to save stack count before manual nodes push
        /// Please use PrepareForManualPush method
        /// </summary>
        public bool IsStackManuallyFormed { get; private set; }

        /// <summary>
        /// Persists stack size before calling PrepareForManualPush
        /// This number is needed to perform correct "translation promise" which
        /// will call OnAfterChildTraversal
        /// </summary>
        public int StackCountBeforeManualPush { get; internal set; }

        /// <summary>
        /// Instructs translator to procced to the next appropriate reactor for node
        /// it's cannot be set directly. Please use FallDownToNextReactor method
        /// </summary>
        internal bool FallDown { get; private set; }


        /// <summary>
        /// Instructs translator not to push child nodes to stack automatically
        /// </summary>
        /// <param name="ctx">Current translation context</param>
        public void PrepareForManualPush(TranslationContext ctx)
        {
            IsStackManuallyFormed = true;
            StackCountBeforeManualPush = ctx.TranslationStack.Count;
        }

        /// <summary>
        /// Instructs translator that next appropriate node reactor should process this node
        /// </summary>
        /// <param name="fallDown">Should or not?</param>
        public void FallDownToNextReactor(bool fallDown = true)
        {
            FallDown = fallDown;
        }
    }
}