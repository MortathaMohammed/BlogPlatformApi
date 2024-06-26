using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class ReplyCommentsEndPoint
{
    public static async Task<IResult> GetReplyComments(IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.ReplyComments.GetAllAsync();
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetReplyCommentsById(string id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.ReplyComments.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetReplyCommentsByParent(string id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.ReplyComments.GetReplyCommentsByParent(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetReplyCommentsByUser(string id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.ReplyComments.GetReplyCommentsByUser(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> AddReplyComments(IUnitOfWork _unitOfWork, ReplyComments replyComments)
    {
        if (replyComments == null)
            return TypedResults.BadRequest();

        var result = await _unitOfWork.ReplyComments.AddAsync(replyComments);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditReplyComments(string id, IUnitOfWork _unitOfWork, ReplyComments replyComments)
    {
        if (replyComments == null)
            return TypedResults.BadRequest("The post is empty");

        var findComment = await _unitOfWork.ReplyComments.GetByIdAsync(id);

        if (findComment == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.ReplyComments.UpdateAsync(replyComments);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteReplyComments(string id, IUnitOfWork _unitOfWork)
    {
        var post = await _unitOfWork.ReplyComments.GetByIdAsync(id);

        if (post == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.ReplyComments.DeleteAsync(id);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}