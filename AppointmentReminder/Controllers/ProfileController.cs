using System.Web.Mvc;

namespace AppointmentReminder.Controllers
{
	[Authorize]
	public class ProfileController : Controller
    {
		//
        // GET: /Profile/

        public ViewResult Index()
        {
			return this.View();
        }

        //
        // GET: /Profile/Create

        public ViewResult Create()
        {
			return View("Create");
        }

        //
        // GET: /Profile/Edit/5

        public ViewResult Edit()
        {
			return this.View();
        }
    }
}
