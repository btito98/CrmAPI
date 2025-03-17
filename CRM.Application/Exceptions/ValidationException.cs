namespace CRM.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(string message, Exception exception) : base(message, exception)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(string message, List<string> errors) : base(message)
        {
            Errors = errors ?? new List<string>();
        }

        public ValidationException(List<string> errors) : base("Ocorreram um ou mais erros de validação.")
        {
            Errors = errors ?? new List<string>();
        }
    }
}