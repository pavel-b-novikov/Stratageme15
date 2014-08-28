namespace Stratageme15.Core.Translation.Logging
{
    /// <summary>
    /// Describes type of event occured during translation process
    /// </summary>
    public enum TranslationEventType
    {
        /// <summary>
        /// Debug information message
        /// </summary>
        Debug,

        /// <summary>
        /// Any informational event
        /// </summary>
        Info,

        /// <summary>
        /// Translation warning. 
        /// Denotes a situtation when logical errors occur in source code.
        /// Also indicates situation when unwanted side effectcs can happen
        /// </summary>
        Warning,

        /// <summary>
        /// Translation error.
        /// Indicates a case when translation process can not continue due to critical source code error
        /// </summary>
        Error,

        /// <summary>
        /// Any critical error which is not caused by translated code.
        /// Configuration issues can be occured within "critical" event type 
        /// </summary>
        Critical
    }
}