using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Saida
    {
        [Key()]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar nome!")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar valor!")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [MaxLength(100)]
        public string Descricao { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }
    }
}