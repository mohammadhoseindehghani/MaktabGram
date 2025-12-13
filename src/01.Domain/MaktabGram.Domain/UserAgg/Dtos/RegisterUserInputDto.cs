using MaktabGram.Domain.Core.UserAgg.Enum;
using Microsoft.AspNetCore.Http;

namespace MaktabGram.Domain.Core.UserAgg.Dtos
{
    public class RegisterUserInputDto
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string? Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Email { get; set; }
        public RoleEnum? Role { get; set; }
        public IFormFile? ProfileImg { get; set; }
        public int Otp { get; set; }
        public int IdentityUserId { get; set; }
    }

}