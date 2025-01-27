using LINQtoSQL.Data;
using LINQtoSQL.Repositories;
using LINQtoSQL.Service;

namespace LINQtoSQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DataSeeder seeder = new DataSeeder();
            UserRepository userRepository = new UserRepository(seeder);
            OrderRepository orderRepository = new OrderRepository(seeder);
            var users = userRepository.Entities.AsQueryable();
            var orders = orderRepository.Entities.AsQueryable();

            var res = users.UsersWithContitionAmount(10);
            foreach (var item in orders)
            {
                Console.WriteLine(item.UserId);
            }
            Console.WriteLine("____________________________________________________");

            var res1 = users.UsersWithRegistrationDataConditionLINQ(0, 5).ToList();
            foreach (var item in res1)
            {
                Console.WriteLine(item.FirstName);
            }
            Console.WriteLine("____________________________________________________");

            var res2 = users.UsersWithContitionAmountLikeSQlQuary(100, 300).ToList();
            foreach (var item in res2)
            {
                Console.WriteLine(item.FirstName);
            }


            var res3 = users.UsersWithContitionAmountWithLINQ(orders, 200, 500).ToList();
            Console.WriteLine("____________________________________________________");

            foreach (var item in res3)
            {
                Console.WriteLine(item.FirstName);
            }

            Console.WriteLine("____________________________________________________");

            var res4 = users.UserWithSpecifiedProductLINQ(orders, "Telephone");
            foreach (var item in res4)
            {
                Console.WriteLine(item.FirstName);
            }

            Console.WriteLine("____________________________________________________");

            var res5 = users.UserWithSpecifiedProductSQL("Telephone");
            foreach (var item in res5)
            {
                Console.WriteLine(item.FirstName);
            }
            Console.WriteLine("____________________________________________________");

            var res6 = users.AvverageAgeOfUsersWithHavingOrderSQL();
            Console.WriteLine(res6);


            Console.WriteLine("____________________________________________________");
            var res7 = users.AvverageAgeOfUsersWithHavingOrderLINQ();
            Console.WriteLine(res7);


            Console.WriteLine("____________________________________________________");
            var res8 = orders.TotalNumberOfOrdersInLast30DaysSQL();
            Console.WriteLine(res8);

            Console.WriteLine("____________________________________________________");
            var res9 = orders.ToalNumberOfOrdersInLast30DaysLINQ();
            Console.WriteLine(res9);



            Console.ReadLine();
        }

    }
}