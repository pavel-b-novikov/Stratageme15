using Roslyn.Compilers.CSharp;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion.Reactors
{
    public interface ISituationReactor : IReactor
    {
        bool IsAcceptable(TranslationContext context);
    }
}
