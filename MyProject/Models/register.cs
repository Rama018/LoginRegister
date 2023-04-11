using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class register
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter The Username")]  
        [DisplayName("Username")]
        [StringLength(10, ErrorMessage =
            "username be less than or equal to ten characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Required(ErrorMessage = "You must provide a phone number")]

        public string Phone { get; set; }


        public string HashPassword { get; set; }

        public ICollection<Task> tasks { get; set; }


    }
}
