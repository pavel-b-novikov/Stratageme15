namespace Stratageme15.Core.Transaltion.Logging
{
    /// <summary>
    /// Interface supplying logging functionality
    /// </summary>
    public interface ITranslationLogger
    {
        void LogEvent(TranslationEvent evt);
    }
}