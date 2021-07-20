using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecrutariBlazorWasm.Server.Interfaces;
using RecrutariBlazorWasm.Shared.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<Recruiter> _userManager;
        private readonly ApiSettings _apiSettings;

        public TokenService(IConfiguration config, UserManager<Recruiter> userManager)
        {

            var appSettingsSection = config.GetSection("ApiSettings");
            this._apiSettings = appSettingsSection.Get<ApiSettings>();
            this._userManager = userManager;
            this._key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
        }

        public async Task<string> CreateToken(Recruiter recruiter)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, recruiter.Name),
                new Claim(ClaimTypes.Email, recruiter.Email),
                new Claim("Id", recruiter.Id)
            };

            var roles = await _userManager.GetRolesAsync(recruiter);

            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken
                (
                 issuer: _apiSettings.ValidIssuer,
                 audience: _apiSettings.ValidAudience,
                 claims: claims,
                 expires: DateTime.Now.AddDays(7),
                 signingCredentials: creds
                );


            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }
    }
}
