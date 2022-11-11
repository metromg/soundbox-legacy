namespace Soundbox.Reloaded.Ui.Web.Controllers
{
    using System.Web.Mvc;

    public class IndexController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return this.View("~/views/index.cshtml");
        }

        [HttpGet]
        public ActionResult Template(string template)
        {
            return this.PartialView("~/views/" + template + ".cshtml");
        }

        [HttpGet]
        public ActionResult DirectiveTemplate(string template)
        {
            return this.PartialView("~/views/directives/" + template + ".cshtml");
        }
    }
}
