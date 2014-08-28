using System.Text;
using Microsoft.CodeAnalysis;

namespace Stratageme15.Core.Transaltion.Logging
{
    /// <summary>
    /// Describes loggable event ovvured during translation process
    /// </summary>
    public class TranslationEvent
    {
        private readonly bool _isLocationSpecified;
        private string _informationalString;

        public TranslationEvent(SyntaxTree root, SyntaxNode node, string message, TranslationEventType eventType,
                                string sourceFile)
        {
            SourceFile = sourceFile;
            FileLinePositionSpan span = root.GetLineSpan(node.FullSpan);
            Line = span.StartLinePosition.Line;
            Column = span.StartLinePosition.Character;
            CodePiece = node.ToString();
            _isLocationSpecified = true;

            Message = message;
            EventType = eventType;
            BuildString();
        }

        public TranslationEvent(string message, TranslationEventType eventType, string sourceFile)
        {
            Message = message;
            EventType = eventType;
            SourceFile = sourceFile;
            BuildString();
        }

        public TranslationEvent(string message, TranslationEventType eventType)
        {
            Message = message;
            EventType = eventType;
            BuildString();
        }

        /// <summary>
        /// Mentioned code piece
        /// </summary>
        public string CodePiece { get; private set; }

        /// <summary>
        /// Location of mentioned code piece - line
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Location of mentioned code piece - column
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Description of event
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Event type
        /// </summary>
        public TranslationEventType EventType { get; private set; }

        /// <summary>
        /// File with source code where event was occured
        /// </summary>
        public string SourceFile { get; private set; }

        private void BuildString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("[{0}] ", EventType);
            if (_isLocationSpecified)
            {
                sb.AppendFormat("[line: {0}, col: {1}] ", Line, Column);
            }

            if (!string.IsNullOrWhiteSpace(SourceFile))
            {
                sb.AppendFormat("[{0}] ", SourceFile);
            }

            sb.Append(Message);
            _informationalString = sb.ToString();
        }

        public override string ToString()
        {
            return _informationalString;
        }
    }
}