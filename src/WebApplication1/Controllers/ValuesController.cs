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
    [Route("api/[controller]")]
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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //_checker.CheckName("asd", "asd");
            //var dossierTask = _checker.DownloadDossierFiles();
            //var orderTask = _checker.DownloadOrderFiles();
            //Task.WaitAll(new Task[] { dossierTask, orderTask });

            var users = _checker.GetRemoteUsers();
            //var users = new List<User>
            //{
            //    new User
            //    {
            //        DossierNr = "123",
            //        FirstName = "Bob",
            //        LastName = "Marley",
            //        OrderNr = "32323ff",
            //        RegisterDate = DateTime.Now,
            //        ResolutionDate = DateTime.Now,
            //        Term = DateTime.Now
            //    },
            //    new User
            //    {
            //        DossierNr = "1234",
            //        FirstName = "Bob",
            //        LastName = "Marley",
            //        OrderNr = "32323ff",
            //        RegisterDate = DateTime.Now,
            //        ResolutionDate = DateTime.Now,
            //        Term = DateTime.Now
            //    }
            //};
            _userRepository.Upsert(users.ToList());
            return "value";
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
