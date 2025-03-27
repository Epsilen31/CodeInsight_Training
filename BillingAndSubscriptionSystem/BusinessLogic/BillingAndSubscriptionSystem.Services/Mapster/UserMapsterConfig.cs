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
                        target.RoleId = source.RoleId;

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
                        target.Role = source.Role?.RoleName;
                        target.Password = source.Password;
                    }
                );
        }
    }
}
