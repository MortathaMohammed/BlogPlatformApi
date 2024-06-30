using BlogPlatformApi.Mapping.Tag;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class TagEndPoint
{
    public static async Task<IResult> GetTags(IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Tags.GetAllAsync();
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result.Select(tag => tag.AsDto()));
    }

    public static async Task<IResult> GetPostsByTag(Guid id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Tags.GetPostsByTagId(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result.Select(tag => tag.AsTagPostsDto()));
    }

    public static async Task<IResult> GetTagById(Guid id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Tags.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result.AsDto());
    }

    public static async Task<IResult> AddTag(IUnitOfWork _unitOfWork, CreateTagDto tags)
    {
        if (tags == null)
            return TypedResults.BadRequest();

        var result = await _unitOfWork.Tags.AddAsync(tags.ToTagFromCreateDto());
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditTag(Guid id, IUnitOfWork _unitOfWork, UpdateTagDto tags)
    {
        if (tags == null)
            return TypedResults.BadRequest("The body is empty");

        var findTag = await _unitOfWork.Tags.GetByIdAsync(id);

        if (findTag == null)
            return TypedResults.NotFound();

        var tagEx = await _unitOfWork.Tags.GetTagByName(tags.TagName);

        if (tagEx != null)
            return TypedResults.BadRequest("Name of tag already exists");

        var result = await _unitOfWork.Tags.UpdateAsync(tags.ToTagFromUpdateDto());
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteTag(Guid id, IUnitOfWork _unitOfWork)
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