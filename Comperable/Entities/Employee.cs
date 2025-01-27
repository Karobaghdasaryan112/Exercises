using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comperable.Entities
{
    public class Employee : IComparable<Employee>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
        public Employee(string name, int age, string department)
        {
            Name = name;
            Age = age;
            Department = department;
        }
        public int CompareTo(Employee? other)
        {
            if (other == null) return 1;
            if (other == this) return 0;
            if (other.Name.CompareTo(this.Name) == 0)
            {
                return other.Age.CompareTo(this.Age);
            }
            else
            {
                return other.Name.CompareTo(this.Name);
            }
        }
    }
}
