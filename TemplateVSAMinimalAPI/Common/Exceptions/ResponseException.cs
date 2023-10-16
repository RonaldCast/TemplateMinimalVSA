namespace TemplateVSAMinimalAPI.Common.Exceptions
{
    public class ResponseException : Exception
    {
        public int StatusCode { get; } = StatusCodes.Status400BadRequest;
        public override string Message { get; }
        public object? Errors { get; }

        public ResponseException( string message)
        {
            Message = message;
        }
        public ResponseException(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public ResponseException(int statusCode, string message, object errors)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }
    }
}
