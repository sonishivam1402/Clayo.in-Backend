namespace e_commerce_backend.DTO
{
    public class AddOrUpdateUsers
    {
        public Guid? Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public string password_hash { get; set; }

        public string? address { get; set; }
        public string? profile_picture { get; set; }
    }
}
