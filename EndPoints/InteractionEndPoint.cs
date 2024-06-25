using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class InteractionEndPoint
{
    public static async Task<IResult> GetInteractionByUser(int id, IUnitOfWork _unitOfWork)
    {
        var interactions = await _unitOfWork.Interactions.GetInteractionsByUserId(id);
        if (interactions == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(interactions);
    }

    public static async Task<IResult> GetInteractionByPost(int id, IUnitOfWork _unitOfWork)
    {
        var interactions = await _unitOfWork.Interactions.GetInteractionsByPostId(id);
        if (interactions == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(interactions);
    }

    public static async Task<IResult> GetInteractionById(int id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Interactions.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> AddInteraction(IUnitOfWork _unitOfWork, Interaction interaction)
    {
        if (interaction == null)
            return TypedResults.BadRequest("Empty body");


        var result = await _unitOfWork.Interactions.AddAsync(interaction);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditInteraction(int id, IUnitOfWork _unitOfWork, Interaction interaction)
    {
        if (interaction == null)
            return TypedResults.BadRequest("Empty body");

        var result = await _unitOfWork.Interactions.UpdateAsync(interaction);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest("Faild");

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteInteraction(int id, IUnitOfWork _unitOfWork)
    {
        var interaction = await _unitOfWork.Interactions.GetByIdAsync(id);

        if (interaction == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Interactions.DeleteAsync(id);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}