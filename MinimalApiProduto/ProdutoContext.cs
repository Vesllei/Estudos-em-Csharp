using Microsoft.EntityFrameworkCore;

namespace MinimalApiProduto;

public class ProdutoContext : DbContext
{
    public DbSet
<Produto> Produtos { get; set; }


protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("Data Source=produtos.db");
}
