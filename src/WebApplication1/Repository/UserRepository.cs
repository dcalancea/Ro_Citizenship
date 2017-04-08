using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Collections.Generic;
using System;

namespace WebApplication1.Repository
{
    public class UserRepository : IUserRepository
    {
        DbContext context;
        public UserRepository(PlayGroundContext context)
        {
            this.context = context;
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public User Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(User entity)
        {
            throw new NotImplementedException();
        }

        public void Upsert(User user)
        {
            context.Entry(user).State = string.IsNullOrEmpty(user.DossierNr) ?
                                           EntityState.Added :
                                           EntityState.Modified;
            context.SaveChanges();
        }

        public void Upsert(List<User> users)
        {
            users.ForEach(u => context.Entry(u).State = string.IsNullOrEmpty(u.DossierNr) ?
                                            EntityState.Added : 
                                            EntityState.Modified);
            context.SaveChanges();
        }
    }
}
