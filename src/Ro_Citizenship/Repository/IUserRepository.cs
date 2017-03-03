using Ro_Citizenship.Models;
using System.Collections.Generic;

namespace Ro_Citizenship.Repository
{
    public interface IUserRepository : IRepository<User, string>
    {
        void Upsert(User user);
        void Upsert(List<User> users);
    }
}
