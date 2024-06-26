using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class PostEndPoint
{
    public static async Task<IResult> GetPosts(IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Posts.GetAllAsync();
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetPostsByUser(string id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Posts.GetPostsByUser(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetPostById(string id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Posts.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> AddPost(IUnitOfWork _unitOfWork, Post post)
    {
        if (post == null)
            return TypedResults.BadRequest();

        var userEx = await _unitOfWork.Posts.GetPostByTitle(post.Title);

        if (userEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Posts.AddAsync(post);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditPost(string id, IUnitOfWork _unitOfWork, Post post)
    {
        if (post == null)
            return TypedResults.BadRequest("The post is empty");

        var findUser = await _unitOfWork.Posts.GetByIdAsync(id);

        if (findUser == null)
            return TypedResults.NotFound();

        var userEx = await _unitOfWork.Posts.GetPostByTitle(post.Title);

        if (userEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Posts.UpdateAsync(post);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeletePost(string id, IUnitOfWork _unitOfWork)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);

        if (post == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Posts.DeleteAsync(id);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}