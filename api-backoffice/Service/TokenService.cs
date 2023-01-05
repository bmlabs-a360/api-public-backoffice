using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using api_public_backOffice.Models;
using api_public_backOffice.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace api_public_backOffice.Service
{
    public interface ITokenService
    {
        string BuildToken(UsuarioModel user);
        bool IsTokenValid(string token);
        public string BuildTokenSimpleToken(string DurationMinutes);
        bool IsTokenValidSimpleToken(string token);
        Dictionary<string, string> GetTokenInfo(string token);
    }

    public class TokenService : ITokenService, IDisposable
    {
        public IConfiguration Configuration { get; }

        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string BuildToken(UsuarioModel user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(Configuration["Jwt:Key"])) throw new ArgumentNullException("Jwt:Key");
            if (string.IsNullOrEmpty(Configuration["Jwt:Issuer"])) throw new ArgumentNullException("Jwt:Issuer");
            if (string.IsNullOrEmpty(Configuration["Jwt:Audience"])) throw new ArgumentNullException("Jwt:Audience");
            if (string.IsNullOrEmpty(Configuration["Jwt:DurationMinutesToken"])) throw new ArgumentNullException("Jwt:DurationMinutesToken");

            string key = Configuration["Jwt:Key"];
            string issuer = Configuration["Jwt:Issuer"];
            string audience = Configuration["Jwt:Audience"];
            double expiryDurationMinutes = double.Parse(Configuration["Jwt:DurationMinutesToken"]);

            var claims = new[] {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
                expires: DateTime.Now.AddMinutes(expiryDurationMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public string BuildTokenSimpleToken(string DurationMinutes)
        {
            if (string.IsNullOrEmpty(Configuration["Jwt:Key"])) throw new ArgumentNullException("Jwt:Key");
            if (string.IsNullOrEmpty(DurationMinutes)) throw new ArgumentNullException("DurationMinutes");

            string key = Configuration["Jwt:Key"];
            double expiryDurationMinutes = double.Parse(DurationMinutes);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken("x", "x", null,
                expires: DateTime.Now.AddMinutes(expiryDurationMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsTokenValidSimpleToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    LifetimeValidator = LifetimeValidator,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = "x",
                    ValidAudience = "x",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    LifetimeValidator = LifetimeValidator,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }

        public Dictionary<string, string> GetTokenInfo(string token)
        {
            var TokenInfo = new Dictionary<string, string>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();

            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            return TokenInfo;
        }

        public void Dispose() { }
    }
}
