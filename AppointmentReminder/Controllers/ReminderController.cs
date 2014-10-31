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

        //
        // GET: /Reminder/Create 

		//public ActionResult Create()
		//{
		//	var reminderModel = new ReminderModel();
		//	reminderModel.ProfileId = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault().Id;
		//	return View(reminderModel);
		//}

        //
        // POST: /Reminder/Create

		//[HttpPost]
		//public ActionResult Create(ReminderModel reminderModel)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_db.Reminders.Add(new Reminder() 
		//			{
		//				Message = reminderModel.Message,
		//				ProfileId = reminderModel.ProfileId, 
		//				ReminderDateTime = reminderModel.ReminderDateTime, 
		//				ContactId = reminderModel.ContactId
		//			}
		//		);
		//		_db.Save();
		//		return RedirectToAction("Index");
		//	}

		//	return this.View(reminderModel);
		//}

        //
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

        //
        // POST: /Reminder/Delete/5

		//[HttpPost]
		//public ActionResult Delete(ReminderModel reminderModel)
		//{

		//	var reminder = _db.Reminders.Where(r => r.Id == reminderModel.Id).FirstOrDefault();
		//	_db.Reminders.Remove(reminder);
		//	_db.Save();

		//	return RedirectToAction("Index");

		//}
    }
}
