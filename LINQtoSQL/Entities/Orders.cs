using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSQL.Entities
{
    public class Orders
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int ProductCount {  get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal OrderAmount { get; set; }
        public string? Status { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
