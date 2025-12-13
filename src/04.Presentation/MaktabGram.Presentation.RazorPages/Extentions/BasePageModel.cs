using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Extentions
{
    public class BasePageModel : PageModel
    {
        public int GetUserId()
        {
            return 3;
            //if (Request.Cookies.TryGetValue("Id", out var userIdStr) &&
            //    int.TryParse(userIdStr, out var userIdFromCookie))
            //{
            //    return userIdFromCookie;
            //}

           //throw new Exception("User is not logged in.");
        }

        public bool IsAdmin()
        {
            return Request.Cookies.Any(x => x.Key == "IsAdmin" && x.Value == "1");

            throw new Exception("User is not admin.");
        }

        public bool UserIsLoggedIn()
        {
            return Request.Cookies.Any(x => x.Key == "Id");
        }

        //public bool GetUserName()
        //{
        //    var username =  Request.Cookies.Where(x => x.Key == "Username").FirstOrDefault();

        //    if(username.)
        //    throw new Exception("User is not logged in.");
        //}
    }
}
