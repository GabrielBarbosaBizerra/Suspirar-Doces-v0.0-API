using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackEnd.Models
{
    public class Estoque
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar nome!")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar quantidade mínima.")]
        public int QuantidadeMinima { get; set; }

        [Required(ErrorMessage = "Obrigatório informar quantidade.")]
        public int Quantidade { get; set; }

    }    
}