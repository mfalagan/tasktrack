namespace back.Models.Internal
{
    public class Token
    {
        public int Id { get; set; }
        public required string Value { get; set; }
        public required DateTime Expiration { get; set; }
        public required int UserId { get; set; }
        public required User User { get; set; }
        public required bool IsValid { get; set; }
    }
}
