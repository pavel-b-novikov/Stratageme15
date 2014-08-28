using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.Transaltion.Logging;

namespace Stratageme15.Boudoir
{
    class ConsoleTranslationLogger : ITranslationLogger
    {
        public void LogEvent(TranslationEvent evt)
        {
            Console.WriteLine(evt.ToString());
        }
    }
}
