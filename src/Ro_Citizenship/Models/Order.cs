using System;
using System.Collections.Generic;

namespace Ro_Citizenship.Models
{
    public class Order
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
