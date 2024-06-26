using System.Data;
using BlogPlatformApi.EndPoints;
using BlogPlatformApi.Services.Repository;
using BlogPlatformApi.Services.Repository.IRepository;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBlogUserRepository, BlogUserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IReplyCommentsRepository, ReplyCommentsRepository>();
builder.Services.AddScoped<ITagsRepository, TagsRepository>();
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
app.MapGet("/user/{id}", UserEndPoint.GetUserById);
app.MapPost("/user/add", UserEndPoint.AddUser);
app.MapPut("/user/edit/{id}", UserEndPoint.EditUser);
app.MapDelete("/user/delete/{id}", UserEndPoint.DeleteUser);
#endregion

#region Post
app.MapGet("/post/posts", PostEndPoint.GetPosts);
app.MapGet("/post/user/{id}", PostEndPoint.GetPostsByUser);
app.MapGet("/post/{id}", PostEndPoint.GetPostById);
app.MapPost("/post/add", PostEndPoint.AddPost);
app.MapPut("/post/edit/{id}", PostEndPoint.EditPost);
app.MapDelete("/post/delete/{id}", PostEndPoint.DeletePost);
#endregion

#region Comment
app.MapGet("/comment/post/{id}", CommentEndPoint.GetCommentsByPost);
app.MapGet("/comment/user/{id}", CommentEndPoint.GetCommentsByUser);
app.MapGet("/comment/{id}", CommentEndPoint.GetCommentById);
app.MapPost("/comment/add", CommentEndPoint.AddComment);
app.MapPut("/comment/edit/{id}", CommentEndPoint.EditComment);
app.MapDelete("/comment/delete/{id}", CommentEndPoint.DeleteComment);
#endregion

#region Comment
app.MapGet("/replycomments/user/{id}", ReplyCommentsEndPoint.GetReplyCommentsByUser);
app.MapGet("/replycomments/parent/{id}", ReplyCommentsEndPoint.GetReplyCommentsByParent);
app.MapGet("/replycomments/{id}", ReplyCommentsEndPoint.GetReplyCommentsById);
app.MapPost("/replycomments/add", ReplyCommentsEndPoint.AddReplyComments);
app.MapPut("/replycommetns/edit/{id}", ReplyCommentsEndPoint.EditReplyComments);
app.MapDelete("/replycomments/delete/{id}", ReplyCommentsEndPoint.DeleteReplyComments);
#endregion

#region Comment
app.MapGet("/tag/tags", TagEndPoint.GetTags);
app.MapGet("/tag/{id}", TagEndPoint.GetTagById);
app.MapPost("/tag/add", TagEndPoint.AddTag);
app.MapPut("/tag/edit/{id}", TagEndPoint.EditTag);
app.MapDelete("/tag/delete/{id}", TagEndPoint.DeleteTag);
#endregion

#endregion

app.Run();

