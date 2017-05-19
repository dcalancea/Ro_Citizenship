using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class UserRepository : IUserRepository
    {
        PlayGroundContext context;
        public UserRepository(PlayGroundContext context)
        {
            this.context = context;
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public User Get(string dossierNumber)
        {
            return context.Users.FirstOrDefault(u => u.DossierNr == dossierNumber);
        }

        public void Save(User entity)
        {
            throw new NotImplementedException();
        }

        public void Upsert(User user)
        {
            if (!context.Users.Any(u => u.DossierNr == user.DossierNr))
            {
                context.Users.Add(user);
            }
            else
            {
                context.Users.Update(user);
            }
            context.SaveChanges();
        }

        public void Upsert(List<User> users)
        {
            //var existingUserIds = users.Select(u => u.DossierNr).Intersect(context.Users.Select(u => u.DossierNr)).ToList();
            //var existingUsers = users.Where(u => existingUserIds.Contains(u.DossierNr));
            //var newUsers = users.Except(existingUsers);

            //foreach (User existingUser in existingUsers)
            //{
            //    context.Users.Update(existingUser);
            //}
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Users]");
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private void UpdateUser(User dbUser, User updatedUser)
        {
            dbUser.DossierNr = updatedUser.DossierNr;
            dbUser.FirstName = updatedUser.FirstName;
            dbUser.LastName = updatedUser.LastName;
            dbUser.OrderNr = updatedUser.OrderNr;
            dbUser.RegisterDate = updatedUser.RegisterDate;
            dbUser.ResolutionDate = updatedUser.ResolutionDate;
            dbUser.Term = updatedUser.Term;
        }

        private class UserIdComparer : IEqualityComparer<User>
        {
            public int Compare(User x, User y)
            {
                if (x == null || y == null)
                {
                    return -1;
                }
                return x.DossierNr == y.DossierNr ? 0 : -1;
            }

            public bool Equals(User x, User y)
            {
                return x.DossierNr == y.DossierNr;
            }

            public int GetHashCode(User obj)
            {
                return obj.DossierNr.GetHashCode();
            }
        }
    }
}
