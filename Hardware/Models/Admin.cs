using System.ComponentModel.DataAnnotations;

namespace Hardware.Models
{
    public class Admin
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
