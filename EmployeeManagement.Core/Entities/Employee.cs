namespace EmployeeManagement.Core.Entities {
    public class Employee
    {
        public int Id { get; set; }  // Cette propriété est configurée pour l'auto-incrémentation par défaut
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }
}