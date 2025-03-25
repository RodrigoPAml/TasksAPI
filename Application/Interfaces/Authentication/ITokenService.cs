using Domain.Models.Token;

namespace Domain.Interfaces.Authentication
{
    /// <summary>
    /// Interface for token service
    /// </summary>
    public interface ITokenService
    {
        public string CreateToken(TokenModel model, DateTime expiration);
        public void SetCurrentToken(string token);
        public TokenModel GetToken();
    }
}
