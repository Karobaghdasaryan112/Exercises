using AccsessControl.Attributes;
using AccsessControl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccsessControl.Interfaces
{
    public interface IEntityService
    {


        [CustomAuthorize(role: Roles.Admin)]
        [CustomAuthorize(role: Roles.User)]
        public void DeleteUser(int userId);




        [CustomAuthorize(role: Roles.Admin)]
        [CustomAuthorize(role: Roles.User)]
        public void UserEdit(int userId);




        [CustomAuthorize(role: Roles.Admin)]
        [CustomAuthorize(role: Roles.Meneger)]
        public void ChangeSalaryOfMeneger(int MenegerId,decimal Salary);

            


        [CustomAuthorize(role: Roles.Meneger)]
        [CustomAuthorize(role: Roles.Admin)]
        public void UpdateMeneger(int MenegerId);

    }
}
