namespace TimeManager.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public float MonthlyHours { get; set; }
        public int VacationDays { get; set; }
        public string Location { get; set; }

        public Employee(string name, string position, float monthlyHours, int vacationDays, string location)
        {
            this.Name = name;
            this.Position = position;
            this.MonthlyHours = monthlyHours;
            this.VacationDays = vacationDays;
            this.Location = location;
        }
    }
}
