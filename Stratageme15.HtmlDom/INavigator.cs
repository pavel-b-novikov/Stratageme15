namespace Stratageme15.HtmlDom
{
    /// <summary>
    /// The navigator object contains information about the browser.
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Returns the code name of the browser
        /// </summary>
        string AppCodeName { get; }

        /// <summary>
        /// Returns the name of the browser
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// Returns the version information of the browser
        /// </summary>
        string AppVersion { get; }

        /// <summary>
        /// Determines whether cookies are enabled in the browser
        /// </summary>
        bool CookiesEnabled { get; }

        /// <summary>
        /// Returns the language of the browser
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Determines whether the browser is online
        /// </summary>
        bool OnLine { get; }

        /// <summary>
        /// Returns for which platform the browser is compiled
        /// </summary>
        string Platform { get; }

        /// <summary>
        /// Returns the engine name of the browser
        /// </summary>
        string Product { get; }

        /// <summary>
        /// Returns the user-agent header sent by the browser to the server
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Specifies whether or not the browser has Java enabled
        /// </summary>
        bool JavaEnabled { get; }
    }
}
