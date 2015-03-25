namespace Stratageme15.Core.Translation.Logging
{
    /// <summary>
    /// Interface supplying logging functionality
    /// </summary>
    public interface ITranslationLogger
    {
        void LogEvent(TranslationEvent evt);
    }
}