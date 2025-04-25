namespace e_commerce_backend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string password_hash { get; set; }

        public string Mobile_no { get; set; }

        public string Role { get; set; }

        public string? profile_picture { get; set; }

        public bool hasAccess { get; set; }
        public bool isVerified { get; set; }

        public string? address {  get; set; }
        public DateTime? last_updated { get; set; } = default(DateTime?);

    }
}
