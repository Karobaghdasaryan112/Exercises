using AccsessControl.Data;
using AccsessControl.Interfaces;

namespace AccsessControl.Services
{
    public class EntityServices : IEntityService
    {
        private DataSeeder? _seeder;
        public EntityServices(DataSeeder dataSeeder)
        {
            _seeder = dataSeeder;
        }
        public void ChangeSalaryOfMeneger(int MenegerId, decimal Salary)
        {
            var Meneger = _seeder?.Managers?.Where(Meneger => Meneger.ManagerId == MenegerId).FirstOrDefault();
            if (Meneger == null)
            {
                throw new ArgumentException($"Meneger with Id{MenegerId} is not Found");
            }
            if (Salary < 0)
                throw new ArgumentException("Salary Of Meanager Must be Greather than 0");


            Meneger.Salary = Salary;

        }

        public void DeleteUser(int userId)
        {
            var User = _seeder?.Users?.FirstOrDefault(User => User.UserID == userId);
            if (User == null)
            {
                throw new ArgumentException($"User with Id{userId} is Not Found");
            }
            _seeder?.Users?.Remove(User);
        }

        public void UpdateMeneger(int MenegerId)
        {
            var Manager = _seeder?.Managers?.FirstOrDefault(Meneger => Meneger.ManagerId == MenegerId);
            if (Manager == null)
                throw new ArgumentException($"{nameof(Manager)} is null");

            Console.WriteLine("Some Changing with Manager Entity");
        }

        public void UserEdit(int userId)
        {
            var User = _seeder?.Users?.FirstOrDefault(User => User.UserID == userId);
            if(User == null)
                throw new ArgumentException($"{nameof(User)} is null");

            Console.WriteLine("Some CHanges with User Entity");
        }
    }
}
