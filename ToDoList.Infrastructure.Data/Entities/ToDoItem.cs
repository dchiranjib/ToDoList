using System.ComponentModel.DataAnnotations;

namespace ToDoList.Infrastructure.Data.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Done { get; set; }
        public User User { get; set; }
    }
}