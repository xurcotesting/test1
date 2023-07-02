using BLL;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        UserBLL userbll = new UserBLL();
        
        public IActionResult Index()
        {
            UserDTO dto = new UserDTO();
            return View(dto);
        }

        [HttpPost]
        public ActionResult Index(UserDTO model)
        {
            if(ModelState.IsValid)
            {
                UserDTO user = userbll.GetUserWithUsernameAndPassword(model);

                if (user.ID != 0)
                {
                    return RedirectToAction("index", "Post");
                }
                else
                    return View(model);
            }
            else 
                return View(model);
            
        }
    }
}
