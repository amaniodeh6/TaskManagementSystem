using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TaskItem
    {

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [RegularExpression("^(Low|Medium|High)$")]
        public string Priority { get; set; }

        [RegularExpression("^(Pending|InProgress|Completed)$")]
        public string Status { get; set; }



    }
}
