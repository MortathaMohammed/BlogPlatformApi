using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class CommentEndPoint
{
    public static async Task<IResult> GetCommentsByPost(int id, IUnitOfWork _unitOfWork)
    {
        var comments = await _unitOfWork.Comments.GetCommentsByPostId(id);
        if (comments == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(comments);
    }

    public static async Task<IResult> GetCommentsByUser(int id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Comments.GetCommentsByUserId(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetCommentById(int id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Comments.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> AddComment(IUnitOfWork _unitOfWork, Comment comment)
    {
        if (comment == null)
            return TypedResults.BadRequest("Empty body");

        var result = await _unitOfWork.Comments.AddAsync(comment);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditComment(int id, IUnitOfWork _unitOfWork, Comment comment)
    {
        if (comment == null)
            return TypedResults.BadRequest("Empty body");

        var findUser = await _unitOfWork.Comments.GetByIdAsync(id);

        if (findUser == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Comments.UpdateAsync(comment);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteComment(int id, IUnitOfWork _unitOfWork)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);

        if (comment == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Comments.DeleteAsync(id);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}