using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccsessControl.Entities
{
    public class User : Human
    {
        public int UserID {  get; set; }
        public User(int id, string firstName, string LastName, string PhoneNumber, string Email) : base(firstName, LastName, PhoneNumber, Email)
        {
            UserID = id;
        }

    }

}
