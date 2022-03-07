using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BackEnd.Models
{
    public class Pedido
    {
        [Key()]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public virtual Cliente Cliente { get; set; }

        [Display(Name = "Data do pedido")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [Column(TypeName = "date")]
        public DateTime DataDoPedido { get; set; }

        [Display(Name = "Data de entrega")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [Column(TypeName = "date")]
        public DateTime DataDeEntrega { get; set; }

        [Display(Name = "Local de entrega")]
        [Column(TypeName = "varchar(80)")]
        public string LocalDeEntrega { get; set; }

        [Required(ErrorMessage = "Obrigatório informar Valor!")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorTotal { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorDeEntrada { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorAPagar { get; set; }

        public bool Status { get; set; }

        public virtual List<ProdutoPedido> ProdutosPedidos { get; set; }
        public virtual Entrada Entrada { get; set; }
    }
}