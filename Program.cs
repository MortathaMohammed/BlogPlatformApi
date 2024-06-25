using System.Data;
using BlogPlatformApi.EndPoints;
using BlogPlatformApi.Services.Repository;
using BlogPlatformApi.Services.Repository.IRepository;
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

#region EndPoints

#region User
app.MapGet("/user/users", UserEndPoint.GetUsers);
app.MapGet("/user/{id:int}", UserEndPoint.GetUserById);
app.MapPost("/user/add", UserEndPoint.AddUser);
app.MapPut("/user/edit/{id:int}", UserEndPoint.EditUser);
app.MapDelete("/user/delete/{id:int}", UserEndPoint.DeleteUser);
#endregion

#region Post
app.MapGet("/post/posts", PostEndPoint.GetPosts);
app.MapGet("/post/{id:int}", PostEndPoint.GetPostById);
app.MapPost("/post/add", PostEndPoint.AddPost);
app.MapPut("/post/edit/{id:int}", PostEndPoint.EditPost);
app.MapDelete("/post/delete/{id:int}", PostEndPoint.DeletePost);
#endregion

#region Comment
app.MapGet("/comment/post/{id:int}", CommentEndPoint.GetCommentsByPost);
app.MapGet("/comment/user/{id:int}", CommentEndPoint.GetCommentsByUser);
app.MapGet("/comment/{id:int}", CommentEndPoint.GetCommentById);
app.MapPost("/comment/add", CommentEndPoint.AddComment);
app.MapPut("/comment/edit/{id:int}", CommentEndPoint.EditComment);
app.MapDelete("/comment/delete/{id:int}", CommentEndPoint.DeleteComment);
#endregion

#region Interaction
app.MapGet("/interaction/post/{id:int}", InteractionEndPoint.GetInteractionByPost);
app.MapGet("/interaction/user/{id:int}", InteractionEndPoint.GetInteractionByUser);
app.MapGet("/interaction/{id:int}", InteractionEndPoint.GetInteractionById);
app.MapPost("/interaction/add", InteractionEndPoint.AddInteraction);
app.MapPut("/interaction/edit/{id:int}", InteractionEndPoint.EditInteraction);
app.MapDelete("/interaction/delete/{id:int}", InteractionEndPoint.DeleteInteraction);
#endregion

#endregion

app.Run();

