using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using Mapster;

namespace BillingAndSubscriptionSystem.Services.Mapster
{
    public class UserMapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Mapping UserDto → User
            config
                .NewConfig<UserDto, User>()
                .AfterMapping(
                    (source, target) =>
                    {
                        target.RoleId = int.TryParse(source.Role, out int roleId) ? roleId : 0;
                        if (!string.IsNullOrEmpty(source.Password))
                        {
                            target.Password = BCrypt.Net.BCrypt.HashPassword(source.Password);
                        }
                    }
                );

            // Mapping User → UserDto
            config
                .NewConfig<User, UserDto>()
                .AfterMapping(
                    (source, target) =>
                    {
                        target.Role = source.Role != null ? source.Role.Id.ToString() : null;
                        target.Password = null; // not sending hashed passwords back to clients
                    }
                );
        }
    }
}
