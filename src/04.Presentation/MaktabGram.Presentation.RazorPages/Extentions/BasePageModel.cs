using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MaktabGram.Presentation.RazorPages.Extentions
{
    public class BasePageModel : PageModel
    {
        public int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }

        public bool IsAdmin()
        {
            // Adjust the claim type or value as per your application's admin role setup
            return User.IsInRole("Admin") ||
                   User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        public bool UserIsLoggedIn()
        {
            return User.Identity != null && User.Identity.IsAuthenticated;
        }

        public string? GetUserName()
        {
            return User.Identity?.Name ?? User.FindFirst(ClaimTypes.Name)?.Value;
        }

        public string? GetEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
