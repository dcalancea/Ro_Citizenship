using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Checker;
using Microsoft.Extensions.Options;
using WebApplication1.Repository;
using WebApplication1.Models;

namespace Ro_Citizenship.Controllers
{
    //[Route("api/[controller]")]
    public class ValuesController : Controller
    {
        IChecker _checker;
        IUserRepository _userRepository;
        public ValuesController(IChecker checker, IUserRepository userRepository)
        {
            _checker = checker;
            _userRepository = userRepository;
        }

        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet]
        public User Get(string id)
        {
            //_checker.CheckName("asd", "asd");
            //var dossierTask = _checker.DownloadDossierFiles();
            //var orderTask = _checker.DownloadOrderFiles();
            //Task.WaitAll(new Task[] { dossierTask, orderTask });

            //var users = _checker.GetRemoteUsers();
            //_userRepository.Upsert(users.ToList());

            var user = _userRepository.Get(id);
            return user;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
