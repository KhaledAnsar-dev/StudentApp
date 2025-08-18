using StudentShared.Dtos;

namespace Student_API_Project.Services.Auth
{
    public interface IJwtTokenGenerator
    {
        string Generate(UserDto user);
    }
}
