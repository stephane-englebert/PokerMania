using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PM_BLL.Interfaces;
using PM_DAL.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PM_BLL.Services
{
    public class SecurityTokenService : ISecurityTokenService
    {
        public readonly IConfiguration _configuration;

        public SecurityTokenService(IConfiguration configuration){
            _configuration = configuration;
        }

        public string GetNewSecurityToken(Member member)
        {
            if(member != null)
            {
                Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(ClaimTypes.Role, member.Role),
                    new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                    new Claim("Id", member.Id.ToString()),
                    new Claim("Role", member.Role),
                    new Claim("Pseudo", member.Pseudo),
                    new Claim("Email", member.Email),
                    new Claim("Bankroll", member.Bankroll.ToString()),
                    new Claim("IsPlaying", member.IsPlaying.ToString()),
                    new Claim("IsDisconnected", member.IsDisconnected.ToString()),
                    new Claim("CurrentTournament", member.CurrentTournament.ToString()),
                };
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: signIn
                );
                string Token = new JwtSecurityTokenHandler().WriteToken(token);
                return Token;
            }
            return null;
        }
    }
}
