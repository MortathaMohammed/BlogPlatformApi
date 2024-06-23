using System.Data;
using BlogPlatformApi.Repository;
using BlogPlatformApi.Repository.IRepository;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped((s) => new NpgsqlConnection(builder.Configuration.GetConnectionString("dbConnection")));
builder.Services.AddScoped<IDbTransaction>(s =>
{
    NpgsqlConnection npgsql = s.GetRequiredService<NpgsqlConnection>();
    npgsql.Open();
    return npgsql.BeginTransaction();
});


var app = builder.Build();

app.MapGet("/", (IUnitOfWork unitofwork) =>
{
});


app.Run();
