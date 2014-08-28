using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.ExtensionsPack.Tactics;

namespace Stratageme15.ExtensionsPack.Extensions
{
    public static class TranslationContextExtensions
    {
        public static TacticsRepository GetTacticsRepository(this TranslationContext ctx)
        {
            return ctx.Reactors.GetReactorBatchData<TacticsRepository, ExtensionsPackReactorsBatch>();
        }
    }
}
