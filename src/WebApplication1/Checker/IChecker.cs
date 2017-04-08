using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace WebApplication1.Checker
{
    public interface IChecker
    {
        bool CheckName(string firstName, string lastName);
        IEnumerable<User> GetRemoteUsers();
        Task DownloadDossierFiles();
        Task DownloadOrderFiles();
        Task DownloadFiles(Uri sourceUrl, string outputFolder);
    }
}
