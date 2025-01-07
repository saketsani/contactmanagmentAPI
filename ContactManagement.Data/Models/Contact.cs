using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Data.Models
{
    public class Contact
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
