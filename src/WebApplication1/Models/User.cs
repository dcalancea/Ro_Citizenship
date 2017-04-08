using System;

namespace WebApplication1.Models
{
    public partial class User
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
