namespace back.Models.Internal
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string? Nick { get; set; }

        public User(Models.Transfer.UserCredentials user)
        {
            if (user.Email == null && user.Username == null)
            {
                throw new System.ArgumentException("Email or Username must be provided");
            }
            else if (user.Password == null)
            {
                throw new System.ArgumentException("Password must be provided");
            }
            else
            {
                this.Username = user.Username;
                this.Password = user.Password;
                this.Email = user.Email;
            }
        }
    }
}