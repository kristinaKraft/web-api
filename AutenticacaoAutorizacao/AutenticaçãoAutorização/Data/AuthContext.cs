
using AutenticacaoAutorizacao.Models;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoAutorizacao.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
