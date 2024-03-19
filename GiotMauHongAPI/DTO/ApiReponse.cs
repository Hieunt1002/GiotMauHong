namespace GiotMauHongAPI.DTO
{
    public class ApiReponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Token { get; set; }

        public T Data { get; set; }
    }
}
