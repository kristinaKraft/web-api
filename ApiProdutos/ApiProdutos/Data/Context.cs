using ApiProdutos.Entidade;
using Microsoft.EntityFrameworkCore;

namespace ApiProdutos.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options): base(options) { }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

    }
}
