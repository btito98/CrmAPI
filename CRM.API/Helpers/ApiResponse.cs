namespace CRM.API.Helpers
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }

        public ApiResponse(int statusCode, string message = null, object errors = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Errors = errors;
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Ocorreu um erro na requisição",
                404 => "Recurso não encontrado",
                500 => "Erro interno do servidor",
                _ => null
            };
        }
    }
}
