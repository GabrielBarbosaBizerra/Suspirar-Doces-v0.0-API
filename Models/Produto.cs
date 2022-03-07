using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackEnd.Models
{
    public class Produto
    {
        [Key()]
        public int Id { get; set; }

        [ForeignKey("Ingrediente")]
        public int? IdIngrediente { get; set; }
        public virtual Ingrediente Ingrediente { get; set; }

        [Required(ErrorMessage = "Obrigatório informar nome!")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar Valor!")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0,c}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        public string Descricao { get; set; }

        public virtual List<ProdutoPedido> ProdutosPedidos { get; set; }

    }
}