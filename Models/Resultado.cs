using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Resultado
    {
        [Key()]
        public int Id { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }
        
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Entrada { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Saida { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ResultadoFinanceiro { get; set; }

    }
}
