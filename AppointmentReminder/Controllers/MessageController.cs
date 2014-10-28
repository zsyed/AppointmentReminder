using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using AppointmentReminder.Data;
using Twilio;

namespace AppointmentReminder.Controllers
{
    public class MessageController : Controller
    {
        //
        // GET: /Message/ 

		private IReminderDb _db;

		public MessageController(IReminderDb db)
		{
			_db = db;
		}

		public string CurrentDateTimeValue(string TimeZone)
		{
			int prodServerTimeDifference=0;

			switch (TimeZone)
			{
				case "PST": prodServerTimeDifference = -7; break;
				case "MST": prodServerTimeDifference = -6; break;
				case "CST": prodServerTimeDifference = -5; break;
				case "EST": prodServerTimeDifference = -4; break;
			}

			return DateTime.Now.AddHours(prodServerTimeDifference).ToString(); 
		}

		public JsonResult Send()
		{
			var contactList = new List<SelectListItem>();
			DateTime currentDateTime;
			try
			{
				var reminders = new ReminderDb().Reminders;

				foreach (var reminder in reminders)
				{
					TimeSpan timeDifference;

					var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();

					int prodServerTimeDifference = 0;

					switch (contact.TimeZone)
					{
						case "PST": prodServerTimeDifference = -7; break;
						case "MST": prodServerTimeDifference = -6; break;
						case "CST": prodServerTimeDifference = -5; break;
						case "EST": prodServerTimeDifference = -4; break;
					}

#if DEBUG
					currentDateTime = DateTime.Now;
#else
					currentDateTime = DateTime.Now.AddHours(prodServerTimeDifference);
#endif

					timeDifference = reminder.ReminderDateTime - currentDateTime;
					int RemdinerMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["RemdinerMinutes"]);


					if (timeDifference.Seconds > 0 && timeDifference.Minutes <= RemdinerMinutes && reminder.ReminderDateTime.Date.Equals(currentDateTime.Date) && !reminder.Sent)
					{

						var profile = _db.Profiles.ToList().Find(p => p.Id == contact.ProfileId);
						if (contact.Active && !profile.DeActivate)
						{
							bool reminderSent = false;
							if (contact.SendEmail)
							{
								SendEmail(reminder, profile, contact);

								contactList.Add(new SelectListItem()
													{
															Text = string.Format("{0} {1}", contact.FirstName.Trim(), contact.LastName.Trim()),
															Value = reminder.Message

													});
								reminderSent = true;

								var reminderHistory = new ReminderHistory();
								reminderHistory.ContactId = contact.Id;
								reminderHistory.Message = reminder.Message;
								reminderHistory.ProfileId = profile.Id;
								reminderHistory.ReminderDateTime = reminder.ReminderDateTime;
								reminderHistory.ReminderId = reminder.Id;
								reminderHistory.EmailSent = true;
								reminderHistory.SMSSent = false;
								reminderHistory.MessageSentDateTime = currentDateTime;
								_db.ReminderHistories.Add(reminderHistory);
								_db.Save();
							}

							if (contact.SendSMS)
							{
								SendSMS(reminder, profile, contact);

								contactList.Add(new SelectListItem()
								{
									Text = string.Format("{0} {1}", contact.FirstName.Trim(), contact.LastName.Trim()),
									Value = contact.Id.ToString().Trim()
								});

								reminderSent = true;

								var reminderHistory = new ReminderHistory();
								reminderHistory.ContactId = contact.Id;
								reminderHistory.Message = reminder.Message;
								reminderHistory.ProfileId = profile.Id;
								reminderHistory.ReminderDateTime = reminder.ReminderDateTime;
								reminderHistory.ReminderId = reminder.Id;
								reminderHistory.EmailSent = false;
								reminderHistory.SMSSent = true;
								reminderHistory.MessageSentDateTime = currentDateTime;
								_db.ReminderHistories.Add(reminderHistory);
								_db.Save();
							}

							if (reminderSent)
							{
								_db.Reminders.ToList().Find(r => r.Id == reminder.Id).Sent = true;
								_db.Save();
							}

						}
					}

				}

			}
			catch (Exception exception)
			{
				contactList.Add(new SelectListItem(){Selected = false, Text = exception.Message, Value = exception.InnerException.ToString()});
			}

			if (contactList.Count == 0)
			{
				contactList.Add(new SelectListItem(){Selected = false, Text = "No appointments to send out", Value = "none"});
			}

			return Json(contactList, JsonRequestBehavior.AllowGet);
		}

		public void SendEmail(Reminder reminder, Profile profile, Contact contact)
		{
			string fromEmailAddress = profile.EmailAddress;
			string toEmailAddress = contact.EmailAddress;
			string emailSubject = string.Format("Reminder from {0} {1} - {2}", profile.FirstName, profile.LastName, DateTime.Now.ToString());
			string emailBody = string.Format("Hi {0}, <br/> This is a reminder for you to {1} at {2}. <br/> Sincerely,<br/> {3}", contact.FirstName.Trim(),  reminder.Message, reminder.ReminderDateTime.DayOfWeek + " " + reminder.ReminderDateTime.ToString(), profile.FirstName);

			var emailMessage = new MessageEmail();
			emailMessage.Send(fromEmailAddress, toEmailAddress, emailSubject, emailBody);
		}

		public void SendSMS(Reminder reminder, Profile profile, Contact contact)
		{
			string fromPhoneNumber = profile.PhoneNumberIssued; 
			string toPhoneNumber = string.Format("1{0}", contact.PhoneNumber);
			string message = string.Format("Hi {0}, This is a reminder for you to {1} at {2}. Sincerely, {3}", contact.FirstName.Trim(), reminder.Message, reminder.ReminderDateTime.ToString(), profile.FirstName);

			string AccountSid = profile.AccountSid;
			string AuthToken = profile.AuthToken;

			var twilio = new TwilioRestClient(AccountSid, AuthToken);

			var messageSent = twilio.SendSmsMessage(fromPhoneNumber, toPhoneNumber, message, "");
		}

    }
}
