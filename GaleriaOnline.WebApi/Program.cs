using GaleriaOnline.WebApi.DbContextImagem;
using GaleriaOnline.WebApi.Interfaces;
using GaleriaOnline.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IImagemRepository, ImagemRepository>();
builder.Services.AddDbContext<GaleriaOnlineDbContext>
(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens")),
//    RequestPath = "/imagens"
//});
    

// A SEQUENCIA IMPORTA MUITOOOOO! CUIDADOOOOO


app.UseHttpsRedirection(); 

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseStaticFiles(); // Habilita servir arquivos estáticos da wwwroot

app.MapControllers();

app.Run();
