namespace back.Models.Transfer
{
    public class UserCredentials
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public string GetValidPassword()
        {
            if (Password == null)
                throw new System.ArgumentException("Password must be provided");
            else if (Password.Length < 8)
                throw new System.ArgumentException("Password must be at least 8 characters long");
            else if (Password.Length > 64)
                throw new System.ArgumentException("Password must be at most 64 characters long");
            else if (!Password.Any(char.IsUpper))
                throw new System.ArgumentException("Password must contain at least one uppercase letter");
            else if (!Password.Any(char.IsLower))
                throw new System.ArgumentException("Password must contain at least one lowercase letter");
            else if (!Password.Any(char.IsDigit))
                throw new System.ArgumentException("Password must contain at least one digit");
            else if (!Password.Any(char.IsPunctuation))
                throw new System.ArgumentException("Password must contain at least one special character");
            else
                return Password ?? throw new System.ArgumentException("Password must be provided");
        }
    }
}