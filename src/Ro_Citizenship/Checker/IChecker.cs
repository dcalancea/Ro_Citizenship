using Ro_Citizenship.Models;
using System.Collections.Generic;

namespace Ro_Citizenship.Checker
{
    public interface IChecker
    {
        bool CheckName(string firstName, string lastName);
        IEnumerable<User> GetRemoteUsers();
    }
}
