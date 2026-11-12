using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Login
    {
        [Required(AllowEmptyStrings = false , ErrorMessage ="Email or user name is Required")]
        public string UsernameOrEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
