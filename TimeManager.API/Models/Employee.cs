namespace TimeManager.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Employee(string name)
        {
            this.Name = name;
        }
    }
}
