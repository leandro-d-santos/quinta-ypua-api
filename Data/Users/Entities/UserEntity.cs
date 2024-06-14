using Data.Common.Entities;
using Domain.Users.Models;

namespace Data.Users.Entities
{
    public class UserEntity : Entity
    {
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static UserEntity CreateFromModel(User user)
        {
            return new UserEntity()
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
        }

        public override User TransformToModel()
        {
            return new User()
            {
                UserName = UserName,
                Name = Name,
                Email = Email,
                Password = Password
            };
        }
    }
}