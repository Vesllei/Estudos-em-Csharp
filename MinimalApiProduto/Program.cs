using Microsoft.AspNetCore.Mvc;
using MinimalApiProduto;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProdutoContext>();

var app = builder.Build();

List<Produto> produtos = new List<Produto>();

// Endpoint para listar todos os produtos
app.MapGet("/api/produto/listar", ([FromServices] ProdutoContext context) =>
{
    var produtos = context.Produtos.ToList();
    if (produtos.Any())
    {
        return Results.Ok(produtos);
    }
    return Results.NotFound("Não existem produtos na tabela");
});

// Endpoint para buscar um produto por ID
app.MapGet("/api/produto/buscar/{id}", ([FromRoute] int id, [FromServices] ProdutoContext context) =>
{
    var produto = context.Produtos.FirstOrDefault(x => x.Id == id);
    if (produto == null)
    {
        return Results.NotFound("Produto não encontrado!");
    }
    return Results.Ok(produto);
});

// Endpoint para adicionar um novo produto
app.MapPost("/api/produto/adicionar", ([FromBody] Produto produto, [FromServices] ProdutoContext context) =>
{
    context.Produtos.Add(produto);
    context.SaveChanges();
    return Results.Created($"/api/produto/buscar/{produto.Id}", produto);
});

// Endpoint para atualizar um produto existente
app.MapPut("/api/produto/atualizar/{id}", ([FromRoute] int id, [FromBody] Produto produto, [FromServices] ProdutoContext context) =>
{
    var existingProduto = context.Produtos.FirstOrDefault(x => x.Id == id);
    if (existingProduto == null)
    {
        return Results.NotFound("Produto não encontrado!");
    }

    existingProduto.Nome = produto.Nome;
    existingProduto.Preco = produto.Preco;

    context.SaveChanges();
    return Results.Ok(existingProduto);
});

// Endpoint para excluir um produto
app.MapDelete("/api/produto/excluir/{id}", ([FromRoute] int id, [FromServices] ProdutoContext context) =>
{
    var produto = context.Produtos.FirstOrDefault(x => x.Id == id);
    if (produto == null)
    {
        return Results.NotFound("Produto não encontrado!");
    }

    context.Produtos.Remove(produto);
    context.SaveChanges();
    return Results.NoContent();
});


app.Run();

// dotnet ef migrations add InitialCreate: Crie a primeira migração. Este comando criará a pasta Migrations e um arquivo de migração inicial.
// dotnet ef database update: Aplique a migração ao banco de dados. Este comando aplicará as alterações de migração ao banco de dados, criando ou atualizando o esquema de acordo com o modelo de dados da aplicação.
