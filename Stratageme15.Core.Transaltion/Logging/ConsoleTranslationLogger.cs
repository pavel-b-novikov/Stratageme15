using System;

namespace Stratageme15.Core.Translation.Logging
{
    public class ConsoleTranslationLogger : ITranslationLogger
    {
        public void LogEvent(TranslationEvent evt)
        {
            Console.WriteLine(evt.ToString());
        }
    }
}
