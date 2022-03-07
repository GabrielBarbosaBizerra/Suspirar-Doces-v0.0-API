using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Cliente
    {
        [Key()]
        public int Id { get; set; }

        public string CPF { get; set; }

        [Required(ErrorMessage = "Obrigatório informar nome!")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar telefone!")]
        [MaxLength(11)]
        public string Telefone { get; set; }

        [MaxLength(20)]
        public string Cidade { get; set; }

        public virtual List<Pedido> Pedidos { get; set; }
    }
}