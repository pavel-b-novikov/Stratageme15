using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion
{
    public enum TranslationStrategy
    {
        TraverseChildren,
        TraverseChildrenAndNotifyMe,
        DontTraverseChildren
    }
    public class TranslationResult
    {
        private bool _isTranslationStrategySet;
        private TranslationStrategy _strategy;

        public bool IsTranslationStrategySet
        {
            get { return _isTranslationStrategySet; }
        }

        public TranslationStrategy Strategy
        {
            get { return _strategy; }
            set { _strategy = value;
                _isTranslationStrategySet = true;
            }
        }

        public int SkipChildrenCount { get; set; }
        public bool IsStackManuallyFormed { get; private set; }
        public int StackCountBeforeManualPush { get; internal set; }
        public TranslationResult(TranslationStrategy strategy)
        {
            Strategy = strategy;
            _isTranslationStrategySet = false;
            SkipChildrenCount = 0;
            IsStackManuallyFormed = false;
        }

        public void PrepareForManualPush(TranslationContext ctx)
        {
            IsStackManuallyFormed = true;
            StackCountBeforeManualPush = ctx.TranslationStack.Count;
        }
    }
}
