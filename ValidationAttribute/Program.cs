using System.Reflection;
using ValidationAttribute.Roles;
using ValidationAttribute.ValidationHandler;

namespace ValidationAttribute
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Handler handler = new Handler();
            User user = new User("Name", "LastName", "NameLastName@gmail.com", "Name123");
            try
            {
                handler.Handle(user);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
