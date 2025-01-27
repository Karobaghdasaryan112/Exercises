using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccsessControl.Entities
{
    public class Admin : Human
    {
        public int AdminId {  get; set; }
        public Admin(int Id,string firstName, string lastName, string phoneNumber, string email) : base(firstName, lastName, phoneNumber, email)
        {
            AdminId = Id;
        }
    }
}
