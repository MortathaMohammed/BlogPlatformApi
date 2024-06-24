using System.Data;
using BlogPlatformApi.EndPoints;
using BlogPlatformApi.Models;
using BlogPlatformApi.Repository;
using BlogPlatformApi.Repository.IRepository;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBlogUserRepository, BlogUserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IInteractionRepository, InteractionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped((s) => new NpgsqlConnection(builder.Configuration.GetConnectionString("dbConnection")));
builder.Services.AddScoped<IDbTransaction>(s =>
{
    NpgsqlConnection npgsql = s.GetRequiredService<NpgsqlConnection>();
    npgsql.Open();
    return npgsql.BeginTransaction();
});

var app = builder.Build();

app.MapGet("/user", UserEndPoint.GetUsers);
app.MapGet("/user/{id:int}", UserEndPoint.GetUserById);
app.MapPost("/user/add", UserEndPoint.AddUser);
app.MapPut("/user/edit/{id:int}", UserEndPoint.EditUser);
app.MapDelete("/user/delete/{id:int}", UserEndPoint.DeleteUser);

app.Run();
