using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Usuario
    {
        [Key()]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Senha { get; set; }
    }
}
