using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.EndPoints;
public static class UserEndPoint
{
    public static async Task<IResult> GetUsers(IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Users.GetAllAsync();
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> GetUserById(int id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Users.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> AddUser(IUnitOfWork _unitOfWork, BlogUser user)
    {
        if (user == null)
            return TypedResults.BadRequest();

        var userEx = await _unitOfWork.Users.GetUserByUserName(user.Username);

        if (userEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Users.AddAsync(user);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditUser(int id, IUnitOfWork _unitOfWork, BlogUser user)
    {
        if (user == null)
            return TypedResults.BadRequest();

        var findUser = await _unitOfWork.Users.GetByIdAsync(id);

        if (findUser == null)
            return TypedResults.NotFound();

        var userEx = await _unitOfWork.Users.GetUserByUserName(user.Username);

        if (userEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Users.UpdateAsync(user);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteUser(int id, IUnitOfWork _unitOfWork)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
            return TypedResults.NotFound();

        var result = await _unitOfWork.Users.DeleteAsync(id);
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }
}