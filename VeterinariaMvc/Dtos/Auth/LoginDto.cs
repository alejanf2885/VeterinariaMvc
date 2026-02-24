using System.ComponentModel.DataAnnotations;

namespace VeterinariaMvc.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }


    }
}
