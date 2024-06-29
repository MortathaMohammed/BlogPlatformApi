using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.User;
public static class UserExtensions
{
    public static GetUserDto AsDto(this BlogUser user)
    {
        return new GetUserDto
        {
            Id = user.user_uid,
            Username = user.username,
            Email = user.email,
            CreatedAt = user.created_at,
            Bio = user.bio
        };
    }

    public static UserDto AsUserDto(this BlogUser user)
    {
        return new UserDto
        {
            Id = user.user_uid,
            Username = user.username,
        };
    }

    public static BlogUser ToUserFromCreate(this CreateUserDto user)
    {
        return new BlogUser
        {
            username = user.Username,
            email = user.Email,
            password_hash = user.PasswordHash,
            bio = user.Bio
        };
    }
    public static BlogUser ToUserFromUpdate(this UpdateUserDto user)
    {
        return new BlogUser
        {
            user_uid = user.Id,
            username = user.Username,
            email = user.Email,
            bio = user.Bio
        };
    }
}