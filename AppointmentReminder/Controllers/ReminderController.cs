using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
	        var profile = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
	        var reminders = new ReminderDb().Reminders.Where(r => r.ProfileId == profile.Id).OrderBy(r => r.ReminderDateTime);
			
	        var remindersModel = new List<ReminderModel>();
			foreach (var reminder in reminders)
			{
				var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();
				remindersModel.Add(new ReminderModel()
					{
						Id = reminder.Id, 
						Message = reminder.Message, 
						ProfileId = reminder.ProfileId, 
						ReminderDateTime = reminder.ReminderDateTime, 
						ContactName = string.Format("{0} {1}",contact.FirstName, contact.LastName),
						Sent = reminder.Sent
					}
				);
			}
			return View(remindersModel);
        }

        //
        // GET: /Reminder/Create

        public ActionResult Create()
        {
			var reminderModel = new ReminderModel();
	        reminderModel.ProfileId = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault().Id;
			return View(reminderModel);
        }

        //
        // POST: /Reminder/Create

        [HttpPost]
        public ActionResult Create(ReminderModel reminderModel)
        {
			if (ModelState.IsValid)
			{
				_db.Reminders.Add(new Reminder() 
					{
						Message = reminderModel.Message,
						ProfileId = reminderModel.ProfileId, 
						ReminderDateTime = reminderModel.ReminderDateTime, 
						ContactId = reminderModel.ContactId
					}
				);
				_db.Save();
				return RedirectToAction("Index");
			}

			return this.View(reminderModel);
        }

        //
        // GET: /Reminder/Edit/5

		public ActionResult Edit(int id)
		{
			var reminder = _db.Reminders.Where(r => r.Id == id).FirstOrDefault();
			var reminderModel = new ReminderModel()
				                    {
					                    Id = reminder.Id,
					                    Message = reminder.Message,
					                    ProfileId = reminder.ProfileId,
					                    ReminderDateTime = reminder.ReminderDateTime,
										ContactId = reminder.ContactId,
										Sent = reminder.Sent
				                    };
			return View(reminderModel);
		}

        //
        // POST: /Reminder/Edit/5

        [HttpPost]
        public ActionResult Edit(ReminderModel reminderModel)
        {
			if (ModelState.IsValid)
			{

				var reminder = _db.Reminders.Where(r => r.Id == reminderModel.Id).FirstOrDefault();
				reminder.Message = reminderModel.Message;
				reminder.ReminderDateTime = reminderModel.ReminderDateTime;
				reminder.ContactId = reminderModel.ContactId;
				reminder.Sent = reminderModel.Sent;
				_db.Save();

				return RedirectToAction("Index");
			}
			return this.View(reminderModel);
        }

        //
        // GET: /Reminder/Delete/5

        public ActionResult Delete(int id)
        {
			var reminder = _db.Reminders.Where(r => r.Id == id).FirstOrDefault();
			var reminderModel = new ReminderModel()
			{
				Id = reminder.Id,
				Message = reminder.Message,
				ProfileId = reminder.ProfileId,
				ReminderDateTime = reminder.ReminderDateTime
			};
			return View(reminderModel);
        }

        //
        // POST: /Reminder/Delete/5

        [HttpPost]
        public ActionResult Delete(ReminderModel reminderModel)
        {

			var reminder = _db.Reminders.Where(r => r.Id == reminderModel.Id).FirstOrDefault();
			_db.Reminders.Remove(reminder);
			_db.Save();

			return RedirectToAction("Index");

        }
    }
}
