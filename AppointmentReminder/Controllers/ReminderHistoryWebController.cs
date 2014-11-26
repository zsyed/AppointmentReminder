using System.Linq;
using System.Web.Http;
using AppointmentReminder.Data;

namespace AppointmentReminder.Controllers
{
	using System;
	using System.Collections.Generic;

	using AppointmentReminder.Models;

	public class ReminderHistoryWebController : ApiController
	{
		private IReminderDb _db;

		public ReminderHistoryWebController(IReminderDb db)
		{
			_db = db;
		}

		public List<ReminderHistoryModel> Get()
		{
			try
			{
				var profile = _db.Profiles.ToList().Find(p => p.UserName == "zsyed"); // == User.Identity.Name);
				var reminderHistories = _db.ReminderHistories.Where(rh => rh.ProfileId == profile.Id).OrderByDescending(rh => rh.MessageSentDateTime);
				var reminderHistoryModels = new List<ReminderHistoryModel>();
				foreach (var reminderHistory in reminderHistories)
				{
					var contact = new ReminderDb().Contacts.ToList().Find(c => c.Id == reminderHistory.ContactId);
					var reminder = new ReminderDb().Reminders.ToList().Find(r => r.Id == reminderHistory.ReminderId);
					if (reminder != null)
					{
						reminderHistoryModels.Add(
							new ReminderHistoryModel()
								{
									Message = reminderHistory.Message,
									ReminderDateTime = reminderHistory.ReminderDateTime,
									ContactName = string.Format("{0} {1}", contact.FirstName, contact.LastName),
									SMSSent = reminderHistory.SMSSent,
									EmailSent = reminderHistory.EmailSent,
									MessageSentDateTime = reminderHistory.MessageSentDateTime,
									Recurrence = reminder.Recurrence,
									Weekday = reminder.WeekDay
								});
					}
				}
				return reminderHistoryModels;
			}
			catch (Exception exception)
			{
				throw exception;
			}

		}
	}
}