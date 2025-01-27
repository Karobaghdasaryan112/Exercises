using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clonable.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public Customer? Customer { get; set; }
        public List<Product>? Products {  get; set; } 
    }
}
