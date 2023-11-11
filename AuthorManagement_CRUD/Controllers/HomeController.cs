using Microsoft.AspNetCore.Mvc;

namespace AuthorManagement_CRUD.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
