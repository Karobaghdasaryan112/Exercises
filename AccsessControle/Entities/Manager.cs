

namespace AccsessControl.Entities
{
    public class Manager : Human
    {


        public int ManagerId {  get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        public Manager(int Id, string Department, string Position, DateTime dateTime, decimal Salary, string firstName, string lastName, string phoneNumber, string email) : base(firstName, lastName, phoneNumber, email)
        {
            ManagerId = Id;
            this.Department = Department;
            this.Position = Position;
            this.HireDate = dateTime;
            this.Salary = Salary;
        }

    }
}
