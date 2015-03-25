using System;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation.Logging
{
    /// <summary>
    /// Shortcuts for error logging
    /// </summary>
    public static class TranslationContextLoggingExtensions
    {
        /// <summary>
        /// Perform information message
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public static void Info(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Info));
        }

        /// <summary>
        /// Perform debug message
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public static void Debug(this TranslationContext ctx, string message, params object[] args)
        {
#if DEBUG
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Debug));
#endif
        }

        /// <summary>
        /// Perform translation warning
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public static void Warn(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Warning));
        }

        /// <summary>
        /// Perform translation error (user is responsible)
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public static void Error(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Error));
        }

        /// <summary>
        /// Perform internal translation error (translator is responsible)
        /// </summary>
        /// <param name="ctx">Translation context</param>
        /// <param name="message">Informational message (format string)</param>
        /// <param name="args">Formatting arguments</param>
        public static void Crit(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Critical));
            throw new Exception(string.Format(message,args));
        }

        private static TranslationEvent ConstructEvent(TranslationContext ctx, string message, TranslationEventType type)
        {
            if (ctx.SourceNode == null || ctx.TranslationRoot == null)
            {
                return new TranslationEvent(message, type, ctx.FileName);
            }
            else
            {
                return new TranslationEvent(ctx.TranslationRoot, ctx.SourceNode, message, type, ctx.FileName);
            }
        }
    }
}