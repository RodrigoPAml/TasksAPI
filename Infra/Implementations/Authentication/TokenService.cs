using Domain.Enums;
using System.Text;
using Domain.Models.Token;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Domain.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;


namespace Infra.Implementations.Authentication
{
    /// <summary>
    /// Token service implementation
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private string _token = string.Empty;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(TokenModel model, DateTime expiration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["JwtKey"];

            var claims = new List<Claim>();

            claims.Add(new Claim("Id", model.Id.ToString()));
            claims.Add(new Claim("Email", model.Email));
            claims.Add(new Claim("Username", model.Username));
            claims.Add(new Claim("Profile", ((int)model.Profile).ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = expiration,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), 
                    SecurityAlgorithms.HmacSha512
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void SetCurrentToken(string token)
        {
            _token = token.Replace("Bearer ", string.Empty);
        }

        public TokenModel GetToken()
        {
            if (string.IsNullOrEmpty(_token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["JwtKey"];

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            };

            try
            {
                var principal = tokenHandler.ValidateToken(_token, validationParameters, out var validatedToken);

                var idClaim = principal.FindFirst("Id")?.Value;
                var emailClaim = principal.FindFirst("Email")?.Value;
                var usernameClaim = principal.FindFirst("Username")?.Value;
                var profileClaim = principal.FindFirst("Profile")?.Value;

                return new TokenModel
                {
                    Id = int.Parse(idClaim),
                    Email = emailClaim,
                    Username = usernameClaim,
                    Profile = (ProfileTypeEnum)int.Parse(profileClaim),
                };
            }
            catch
            {
                return null;
            }
        }
    }
}