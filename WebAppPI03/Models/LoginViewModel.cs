using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppPI03.Models
{
    [NotMapped]
    public class LoginViewModel
    {

        [Required]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string ID { get; set; }
    }
}