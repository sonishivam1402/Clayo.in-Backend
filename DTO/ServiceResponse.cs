namespace e_commerce_backend.DTO
{
    public class ServiceResponse<T> where T : class
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }
    }
}
