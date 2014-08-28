﻿using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion.Logging
{
    /// <summary>
    /// Shortcuts for error logging
    /// </summary>
    public static class TranslationContextLoggingExtensions
    {
        public static void Info(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Info));
        }

        public static void Debug(this TranslationContext ctx, string message, params object[] args)
        {
#if DEBUG
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Debug));
#endif
        }

        public static void Warn(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Warning));
        }

        public static void Error(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Error));
        }

        public static void Crit(this TranslationContext ctx, string message, params object[] args)
        {
            if (args != null && args.Length != 0) message = string.Format(message, args);
            ctx.Logger.LogEvent(ConstructEvent(ctx, message, TranslationEventType.Critical));
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