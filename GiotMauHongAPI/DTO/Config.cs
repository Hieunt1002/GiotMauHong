using System.IO;
using Newtonsoft.Json;

namespace GiotMauHongAPI
{
    public class Config
    {
        public ErrorConfig ErrorMessages { get; set; }
        public SuccessConfig SuccessMessages { get; set; }

        public static Config LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }

    public class ErrorConfig
    {
        public ErrorMessage BadRequest { get; set; }
        public ErrorMessage InternalServerError { get; set; }
        public ErrorMessage InvalidEmailFormat { get; set; }
        public ErrorMessage EmailRequired { get; set; } 
        public ErrorMessage EmailFormat { get; set; }
        public ErrorMessage PasswordRequired { get; set; }
        public ErrorMessage PasswordLength { get; set; }
        public ErrorMessage EmailPassword { get; set; }
        public ErrorMessage ErrorPassword { get; set; }
        public ErrorMessage CheckEmpty { get; set; }
    }

    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class SuccessConfig
    {
        public SuccessMessage ResetPasswordEmailSent { get; set; }
        public SuccessMessage RegistrationSuccess { get; set; }
        public SuccessMessage ChangePassword { get; set; }
        public SuccessMessage Successfully { get; set; }
    }

    public class SuccessMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class SuccessResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }
}
