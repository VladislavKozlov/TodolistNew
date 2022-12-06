using System;
using System.Web.Mvc;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(Exception exception)
        {
            ViewBag.Error = exception.Message;
            return View("Error");
        }

        public ActionResult Http404()
        {
            ViewBag.Error = "Page not found!";
            return View("Error");
        }

        public ActionResult Http403()
        {
            ViewBag.Error = "Access denied!";
            return View("Error");
        }
    }
}