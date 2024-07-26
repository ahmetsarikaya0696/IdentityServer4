using AuthServer.Repositories;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserRepository _customUserRepository;

        public CustomProfileService(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            int userId = int.Parse(context.Subject.GetSubjectId());

            var user = await _customUserRepository.FindByIdAsync(userId);

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("name",user.Username),
                new Claim("city",user.City),
                ];

            // Rol tablosu olsaydı burdan eklenebilirdi
            if (user.Id == 1)
            {
                claims.Add(new Claim("role", "admin"));
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claims); // user-info endpointinden claimleri almasını sağlar

            //context.IssuedClaims = claims; // direkt olarak token içine claimleri kaydeder
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            int userId = int.Parse(context.Subject.GetSubjectId());

            var user = await _customUserRepository.FindByIdAsync(userId);

            context.IsActive = user != null;
        }
    }
}
