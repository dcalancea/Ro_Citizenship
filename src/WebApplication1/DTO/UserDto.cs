using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTO
{
    public class UserDto
    {
        public string DossierNr { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrderNr { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime? Term { get; set; }
    }
}
