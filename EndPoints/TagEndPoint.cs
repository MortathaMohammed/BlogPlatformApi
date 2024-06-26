using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class TagEndPoint
{
    public static async Task<IResult> GetTags(IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Tags.GetAllAsync();
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetTagById(string id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Tags.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> AddTag(IUnitOfWork _unitOfWork, Tags tags)
    {
        if (tags == null)
            return TypedResults.BadRequest();

        var result = await _unitOfWork.Tags.AddAsync(tags);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditTag(string id, IUnitOfWork _unitOfWork, Tags tags)
    {
        if (tags == null)
            return TypedResults.BadRequest("The post is empty");

        var findComment = await _unitOfWork.Tags.GetByIdAsync(id);

        if (findComment == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Tags.UpdateAsync(tags);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteTag(string id, IUnitOfWork _unitOfWork)
    {
        var post = await _unitOfWork.Tags.GetByIdAsync(id);

        if (post == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Tags.DeleteAsync(id);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}