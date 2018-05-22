namespace VinculacionBackend.Models
{
    public class TokenModel
    {
        public long Id;
        public string Token;
        public string AccountId { get; set; }
    }
}