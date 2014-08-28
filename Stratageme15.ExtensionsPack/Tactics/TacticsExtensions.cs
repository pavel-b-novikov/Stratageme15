using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.ExtensionsPack.Tactics
{
    public static class TacticsExtensions
    {
        public static  TacticsRepository GetTacticsRepository(this TranslationContext ctx)
        {
            return ctx.Reactors.GetReactorBatchData<TacticsRepository,ExtensionsPackReactorsBatch>();
        }
    }
}
