namespace Stratageme15.Core.Tools.JavascriptParser
{
    public enum ErrorType
    {
        SyntaxError,
        SemanticError,
        Warning
    }

    public class ParseringIssueDescription
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public string LastToken { get; private set; }
        public string Message { get; set; }
        public ErrorType ErrorType { get; private set; }

        public ParseringIssueDescription(int row, int column, string lastToken, string message)
        {
            ErrorType = ErrorType.SyntaxError;
            Line = row;
            Column = column;
            LastToken = lastToken;
            Message = message;
        }

        public ParseringIssueDescription(int row, int column, string message)
        {
            Line = row;
            Column = column;
            Message = message;
            ErrorType = ErrorType.SemanticError;
        }

        public ParseringIssueDescription(string message)
        {
            Message = message;
            ErrorType = ErrorType.SemanticError;
        }

        public ParseringIssueDescription(int line, int column, string message, ErrorType errorType)
        {
            Line = line;
            Column = column;
            Message = message;
            ErrorType = errorType;
        }

        public ParseringIssueDescription(string message, ErrorType errorType)
        {
            Message = message;
            ErrorType = errorType;
        }

        public override string ToString()
        {
            return string.Format("Line: {0} Column: {1}. {2}: {3} [ last token: '{4}' ]", Line, Column, ErrorType, Message,LastToken);
        }
    }
}
