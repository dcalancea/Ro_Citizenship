using WebApplication1.Models;
using System.Collections.Generic;
using WebApplication1.Repository;

namespace WebApplication1.Repository
{
    public interface IUserRepository : IRepository<User, string>
    {
        void Upsert(User user);
        void Upsert(List<User> users);
    }
}
