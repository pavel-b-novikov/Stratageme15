using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation.Reactors
{
    public interface ISituationReactor : IReactor
    {
        bool IsAcceptable(TranslationContext context);
    }
}