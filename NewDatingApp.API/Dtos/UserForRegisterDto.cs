using System.ComponentModel.DataAnnotations;

namespace NewDatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Please choose a username between 4 and 50 characters.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Please choose a password between 8 and 30 characters.")]
        public string Password { get; set; }

    }
}