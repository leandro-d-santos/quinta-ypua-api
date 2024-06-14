using Data.Context;
using Data.Users.Entities;
using Domain.Common.Utils;
using Domain.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly DbConnection context;

        public UserRepository(DbConnection context)
        {
            this.context = context;
        }

        public void Add(User user)
        {
            Check.ThrowIfNull(user, nameof(user));
            UserEntity entity = UserEntity.CreateFromModel(user);
            context.Set<UserEntity>().Add(entity);
        }

        public IList<User> FindAll()
        {
            return context.Set<UserEntity>()
                .Select((user) => user.TransformToModel())
                .ToList();
        }

        public User? FindByUserName(string userName)
        {
            Check.ThrowIfNull(userName, nameof(userName));
            UserEntity? user = context.Set<UserEntity>()
                .AsNoTracking()
                .SingleOrDefault(e => e.UserName == userName);
            if (user is null)
            {
                return null;
            }
            return user.TransformToModel();
        }

        public void Update(User user)
        {
            Check.ThrowIfNull(user, nameof(user));
            UserEntity entity = UserEntity.CreateFromModel(user);
            context.Set<UserEntity>().Update(entity);
        }

        public void Remove(User user)
        {
            Check.ThrowIfNull(user, nameof(user));
            UserEntity entity = UserEntity.CreateFromModel(user);
            context.Set<UserEntity>().Remove(entity);
        }
    }
}