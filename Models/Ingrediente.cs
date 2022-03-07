using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Ingrediente
    {
        [Key()]
        public int Id { get; set; }
        public int QuantidadeOvos { get; set; }
        public int QuantidadeAcucar { get; set; }
        public int QuantidadeGlacucar { get; set; }
    }
}
