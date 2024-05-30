namespace back.Models.Transfer
{
    public class UserCredentials
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public required string Password { get; set; }
    }
}