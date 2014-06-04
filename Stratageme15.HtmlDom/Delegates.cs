namespace Stratageme15.HtmlDom
{
    public delegate void EventDelegate<TEvent>(TEvent evt) where TEvent : IEvent;

    public delegate void ContextEventDelegate<TThis, TEvent>(TThis thisContext, TEvent evt) where TEvent : IEvent;
}
