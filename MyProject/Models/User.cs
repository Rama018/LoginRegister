using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace MyProject.Models
{
    public class User
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Please Enter The username")]

        public string Name { get; set; }

        [DataType(DataType.Password)]

        [Required(ErrorMessage = "Please Enter The Password")]
        public string Password { get; set; }

        

    }
}
