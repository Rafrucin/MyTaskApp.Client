using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTaskApp.Client.Models
{
    public class UserModel
    {
        [Required] [MaxLength(100)] public string UserName { get; set; }
        [Required] [MaxLength(100)] public string Password { get; set; }
        [Required] [MaxLength(100)] public string ConfirmPassword { get; set; }
        public string AccessToken { get; set; }

    }
}
