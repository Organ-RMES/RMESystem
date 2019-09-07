using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Models
{
    public class LoginModel
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Pw { get; set; }
    }
}
