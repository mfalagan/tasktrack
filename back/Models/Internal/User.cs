namespace back.Models.Internal
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Token> Tokens { get; set; } = new List<Token>();

        public User(string? Username, string? Email, string Password)
        {
            this.Username = Username;
            this.Email = Email;
            this.Password = Password;
        }
    }
}
