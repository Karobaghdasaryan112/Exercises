using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSQL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public int Age { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
