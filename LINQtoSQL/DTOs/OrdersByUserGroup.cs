using LINQtoSQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSQL.DTOs
{
    public class OrdersByUserGroup
    {
        public int UserId { get; set; }
        public int Count {  get; set; }
        public decimal TotalAmount {  get; set; }
    }
}
