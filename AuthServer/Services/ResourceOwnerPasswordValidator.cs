using AuthServer.Models;
using AuthServer.Repositories;
using IdentityModel;
using IdentityServer4.Validation;

namespace AuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICustomUserRepository _customUserRepository;

        public ResourceOwnerPasswordValidator(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isUser = await _customUserRepository.ValidateAsync(context.UserName, context.Password);

            if (isUser)
            {
                CustomUser user = await _customUserRepository.FindByEmailAsync(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}
