using System;
using System.Collections.Generic;

namespace Stratageme15.HtmlDom
{
    public class BaseEventType
    {
        private static readonly Dictionary<string,BaseEventType> _eventsCache = new Dictionary<string, BaseEventType>();
        private string _typeString;

        internal BaseEventType(string type)
        {
            _typeString = type;
        }

        protected static TEvent Evt<TEvent>(string name) where TEvent : BaseEventType
        {
            if (!_eventsCache.ContainsKey(name))
            {
                _eventsCache[name] = (BaseEventType) Activator.CreateInstance(typeof(TEvent),new []{name});
            }
            return (TEvent) _eventsCache[name];
        }
    }
}
