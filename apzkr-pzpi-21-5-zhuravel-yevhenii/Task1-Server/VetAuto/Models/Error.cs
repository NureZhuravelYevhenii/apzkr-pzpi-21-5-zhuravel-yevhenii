namespace VetAuto.Models
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public Error(int status, string message)
        {
            StatusCode = status;
            Message = message;
        }
    }
}
