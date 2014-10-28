using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using AppointmentReminder.Data;
using AppointmentReminder.Models;

namespace AppointmentReminder.Controllers
{
	using System;
	using System.Net;
	using System.Net.Http;

	public class ReminderWebController : ApiController
	{
		private IReminderDb _db;

		public ReminderWebController(IReminderDb db)
		{
			_db = db;
		}

		public HttpResponseMessage Put([FromBody] ReminderModel reminder)
		{
			try
			{
				var dbReminder = _db.Reminders.ToList().Find(r => r.Id == reminder.Id);
				dbReminder.Message = reminder.Message;
				dbReminder.ProfileId = reminder.ProfileId;
				dbReminder.ReminderDateTime = reminder.ReminderDateTime;
				dbReminder.Sent = reminder.Sent;
				_db.Save();
				return Request.CreateResponse(HttpStatusCode.Created, reminder);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}
		}

		public List<ReminderModel> GetAllReminders()
		{
			var profile = _db.Profiles.ToList().Find(p => p.UserName == User.Identity.Name);
			var reminders = new ReminderDb().Reminders.Where(r => r.ProfileId == profile.Id).OrderBy(r => r.ReminderDateTime);

			var remindersModel = new List<ReminderModel>();
			foreach (var reminder in reminders)
			{
				var contact = _db.Contacts.ToList().Find(c => c.Id == reminder.ContactId);
				remindersModel.Add(new ReminderModel()
					{
						Id = reminder.Id,
						Message = reminder.Message,
						ProfileId = reminder.ProfileId,
						ReminderDateTime = reminder.ReminderDateTime,
						ContactName = string.Format("{0} {1}", contact.FirstName, contact.LastName),
						Sent = reminder.Sent
					}
				);
			}
			return remindersModel;
		}

		public ReminderModel Get(int Id)
		{
			var profile = _db.Profiles.ToList().Find(p => p.UserName == User.Identity.Name);
			var reminder = _db.Reminders.Where(p => p.ProfileId == profile.Id).ToList().Find(r => r.Id == Id);
			var contact = _db.Contacts.ToList().Find(c => c.Id == reminder.ContactId);
			var reminderModel = new ReminderModel()
				                    {
					                    Id = reminder.Id,
					                    Message = reminder.Message,
					                    ProfileId = reminder.ProfileId,
					                    ReminderDateTime = reminder.ReminderDateTime,
					                    ContactName = string.Format("{0} {1}", contact.FirstName, contact.LastName),
					                    Sent = reminder.Sent
				                    };
			return reminderModel;
		}

		public HttpResponseMessage Delete(int Id)
		{
			var dbReminder = _db.Reminders.ToList().Find(r => r.Id == Id);

			if (dbReminder == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}

			try
			{
				_db.Reminders.Remove(dbReminder);
				_db.Save();
				return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}
		}

	}
}