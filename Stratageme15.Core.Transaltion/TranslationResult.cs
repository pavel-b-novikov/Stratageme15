using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation
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

        public TranslationResult(TranslationStrategy strategy)
        {
            Strategy = strategy;
            _isTranslationStrategySet = false;
            SkipChildrenCount = 0;
            IsStackManuallyFormed = false;
            FallDown = false;
        }

        public bool IsTranslationStrategySet
        {
            get { return _isTranslationStrategySet; }
        }

        public TranslationStrategy Strategy
        {
            get { return _strategy; }
            set
            {
                _strategy = value;
                _isTranslationStrategySet = true;
            }
        }

        public int SkipChildrenCount { get; set; }
        public bool IsStackManuallyFormed { get; private set; }
        public int StackCountBeforeManualPush { get; internal set; }

        internal bool FallDown { get; set; }

        public void PrepareForManualPush(TranslationContext ctx)
        {
            IsStackManuallyFormed = true;
            StackCountBeforeManualPush = ctx.TranslationStack.Count;
        }

        public void FallDownToNextReactor(bool fallDown = true)
        {
            FallDown = fallDown;
        }
    }
}