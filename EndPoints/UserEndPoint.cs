using BlogPlatformApi.Mapping.User;
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
        return TypedResults.Ok(result.Select(user => user.AsDto()));
    }

    public static async Task<IResult> GetUserById(Guid id, IUnitOfWork _unitOfWork)
    {
        var result = await _unitOfWork.Users.GetByIdAsync(id);
        if (result == null)
            return TypedResults.NotFound();
        return TypedResults.Ok(result.AsDto());
    }

    public static async Task<IResult> AddUser(IUnitOfWork _unitOfWork, CreateUserDto user)
    {
        if (user == null)
            return TypedResults.BadRequest();

        var userEx = await _unitOfWork.Users.GetUserByUserName(user.Username);

        if (userEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Users.AddAsync(user.ToUserFromCreate());
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> EditUser(Guid id, IUnitOfWork _unitOfWork, UpdateUserDto user)
    {
        if (user == null)
            return TypedResults.BadRequest();

        var findUser = await _unitOfWork.Users.GetByIdAsync(id);

        if (findUser == null)
            return TypedResults.NotFound();

        var userEx = await _unitOfWork.Users.GetUserByUserName(user.Username);

        if (userEx != null)
            return TypedResults.BadRequest("User already existes");

        var result = await _unitOfWork.Users.UpdateAsync(user.ToUserFromUpdate());
        _unitOfWork.Commit();

        if (result == 0)
            return TypedResults.BadRequest();

        return TypedResults.Ok();
    }

    public static async Task<IResult> DeleteUser(Guid id, IUnitOfWork _unitOfWork)
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