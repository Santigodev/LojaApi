using LojaApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Conecta no banco
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Registra o Service do Repository
builder.Services.AddSingleton<ProdutoRepository>(provider => new ProdutoRepository(connectionString));
builder.Services.AddSingleton<UsuarioRepository>(provider => new UsuarioRepository(connectionString));
builder.Services.AddSingleton<CarrinhoRepository>(provider => new CarrinhoRepository(connectionString));


//Swagger Parte 1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

//Swagger Parte 2
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loja V1");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
