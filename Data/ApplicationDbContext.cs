using AppWebCP.Models;
using Microsoft.EntityFrameworkCore;

namespace AppWebCP.Data
{ 
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        // Adicione os outros DbSets (Fornecedor) aqui se der tempo
    }
}
