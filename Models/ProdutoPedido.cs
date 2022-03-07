namespace BackEnd.Models
{
    public class ProdutoPedido
    {
        public int IdPedido { get; set; }
        public virtual Pedido Pedido { get; set; }
        public int IdProduto { get; set; }
        public virtual Produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}