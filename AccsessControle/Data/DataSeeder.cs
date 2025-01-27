
using AccsessControl.Entities;

namespace AccsessControl.Data
{
    public class DataSeeder
    {
        public List<User>? Users;
        public List<Manager>? Managers;
        public List<Admin>? Admins;

        public void SetDatas()
        {
            Users = new List<User>();
            Managers = new List<Manager>();
            Admins = new List<Admin>();

            User user1 = new User(0, "Karen", "Hayrapetyan", "055175566", "KarenHayrapetyan@gmail.com");
            User user2 = new User(1, "Narek", "Harutyunyan", "093708557", "NarekHarutyunyan@gmail.com");

            Manager manager1 = new Manager(
                Id: 0,
                Department: "Sales",
                Position: "Sales Manager",
                dateTime: DateTime.Now.AddYears(-5),
                Salary: 85000,
                firstName: "John",
                lastName: "Doe",
                phoneNumber: "123-456-7890",
                email: "john.doe@example.com"
            );

            Manager manager2 = new Manager(
                Id: 1,
                Department: "IT",
                Position: "IT Manager",
                dateTime: DateTime.Now.AddYears(-3),
                Salary: 95000,
                firstName: "Alice",
                lastName: "Smith",
                phoneNumber: "987-654-3210",
                email: "alice.smith@example.com"
            );

            Admin admin = new Admin(0, "Admin", "Admin", "----", "AdminAdmin@gmail.com");

            Users.Add(user1);
            Users.Add(user2);

            Managers.Add(manager1);
            Managers.Add(manager2);

            Admins.Add(admin);
        }
    }
}
