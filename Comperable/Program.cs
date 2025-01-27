using Comperable.Entities;
using Comperable.Enums;

namespace Comparable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Employes = new List<Employee> 
            {
                new Employee("aaaa", 20, "island"),
                new Employee("bbbb", 25, "island"),
                new Employee("abab", 20, "island"),
                new Employee("cccc", 35, "island")
            };

            Employes.Sort();

            foreach (var item in Employes)
            {
                Console.WriteLine("Name Of Employee " +  item.Name + " , " + "Age Of Employee " + item.Age);
            }



            var tasks = new List<TaskItem>
            {
                 new TaskItem( PriorityEnum.Medium, DateTime.Parse("2025-01-25"),"Task 1"),
                 new TaskItem( PriorityEnum.High, DateTime.Parse("2025-01-22"),"Task 1"),
                 new TaskItem(PriorityEnum.High, DateTime.Parse("2025-01-20"),"Task 1"),
                 new TaskItem(PriorityEnum.Low, DateTime.Parse("2025-01-30"),"Task 1")
            };

            tasks.Sort();
            foreach (var item in tasks)
            {
                Console.WriteLine(item.Priority);
            }
            Console.ReadLine();
        }
    }
}
