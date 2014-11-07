using System.Web.Mvc;

namespace AppointmentReminder.Controllers
{
	using AppointmentReminder.Data;
	using AppointmentReminder.Models;

	[Authorize]
	public class ReminderController : Controller 
	{

		private IReminderDb _db;

		public ReminderController(IReminderDb db)
		{
			_db = db;
		}

        //
        // GET: /Reminder/

        public ActionResult Index()
        {
	        return this.View();
        }

		public ActionResult History()
		{
			return this.View();
		}

		// GET: /Reminder/Edit/5

		public ActionResult Edit(int id)
		{
			return this.View();
		}

		public ActionResult Delete(int id)
		{
			return this.View();
		}

		public ActionResult Create()
		{
			return this.View();
		}
    }
}
