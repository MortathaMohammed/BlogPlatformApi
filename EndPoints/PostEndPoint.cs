using BlogPlatformApi.Mapping.Posts;
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

        return TypedResults.Ok(result.Select(post => post.AsDto()).ToList());
    }

    public static async Task<IResult> GetPostsByUser(Guid id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Posts.GetPostsByUser(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result.Select(post => post.AsDto()));
    }

    public static async Task<IResult> GetPostById(Guid id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Posts.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result.AsDto());
    }

    public static async Task<IResult> AddPost(IUnitOfWork _unitOfWork, CreatePostDto post)
    {
        if (post == null)
            return TypedResults.BadRequest();

        var postEx = await _unitOfWork.Posts.GetPostByTitle(post.Title);

        if (postEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Posts.AddAsync(post.ToPostFromCreate());
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditPost(Guid id, IUnitOfWork _unitOfWork, UpdatePostDto post)
    {
        if (post == null)
            return TypedResults.BadRequest("The post is empty");

        var findPost = await _unitOfWork.Posts.GetByIdAsync(id);

        if (findPost == null)
            return TypedResults.NotFound();

        var postEx = await _unitOfWork.Posts.GetPostByTitle(post.Title);

        if (postEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Posts.UpdateAsync(post.ToPostFromUpdate());
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeletePost(Guid id, IUnitOfWork _unitOfWork)
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