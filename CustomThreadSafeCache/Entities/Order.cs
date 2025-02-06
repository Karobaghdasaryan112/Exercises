using CustomThreadSafeCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomThreadSafeCache.Entities
{
    public class Order : IEntity
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }

        public int Id => OrderId;

        public override string ToString()
        {
            return $"{OrderId},{UserId},{ProductId},{Quantity},{Status}";
        }
    }

}
