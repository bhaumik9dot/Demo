using System.ComponentModel.DataAnnotations;

namespace Demo.Model
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName{ get; set; }
    }
}
