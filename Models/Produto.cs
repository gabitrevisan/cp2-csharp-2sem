namespace AppWebCP.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(Sku), IsUnique = true)] // Garante a regra de SKU único no banco!
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Sku { get; set; } // SKU = Código do produto
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
