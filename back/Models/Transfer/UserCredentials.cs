namespace back.Models.Transfer
{
    public class UserCredentials
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public string GetValidPassword()
        {
            // TODO: Implement password validation
            return Password ?? throw new System.ArgumentException("Password must be provided");
        }
    }
}