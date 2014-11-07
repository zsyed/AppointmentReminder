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
				case "PST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["PSTOffSetHours"]); break;
				case "MST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["MSTOffSetHours"]); break;
				case "CST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["CSTOffSetHours"]); break;
				case "EST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["ESTOffSetHours"]); break;
			}

			return DateTime.Now.AddHours(prodServerTimeDifference).ToString(); 
		}

		public string CurrentDateTime()
		{
			return DateTime.Now.ToString();
		}

		public JsonResult SendReminder()
		{
			// For each reminder
			var contactList = new List<SelectListItem>();
			try
			{
				var reminders = new ReminderDb().Reminders;

				foreach (var reminder in reminders)
				{
					var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();
					if (this.SendReminderAllowed(reminder))
					{
						// get server current datetime.
						DateTime serverCurrentDateTime = this.ServerCurrentDateTime(reminder);
						if (SendDaily(reminder, serverCurrentDateTime))
						{
							contactList.Add(new SelectListItem()
							{
								Text = string.Format("{0} {1}", contact.FirstName.Trim(), contact.LastName.Trim()),
								Value = reminder.Message
							});
						}

						//		is it daily.. figure out daily reminder send

						//		is it weekly.. figure out weekly reminder send

						//		is it once... figure out just once send code.
					}
				}
			}
			catch (Exception exception)
			{
				contactList.Add(new SelectListItem() { Selected = false, Text = exception.Message, Value = exception.InnerException.ToString() });
			}

			if (contactList.Count == 0)
			{
				contactList.Add(new SelectListItem() { Selected = false, Text = "No appointments to send out", Value = "none" });
			}

			return Json(contactList, JsonRequestBehavior.AllowGet);

		}

		private bool SendDaily(Reminder reminder, DateTime serverCurrentDateTime)
		{
			bool reminderSent = false;
			if (reminder.Recurrence == "Daily")
			{
				var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();
				var profile = _db.Profiles.ToList().Find(p => p.Id == reminder.ProfileId);

				TimeSpan timeDifference = reminder.ReminderDateTime - serverCurrentDateTime;
				int RemdinerMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["RemdinerMinutes"]);
				if (timeDifference.Minutes <= RemdinerMinutes)
				{
					
					if (contact.SendEmail)
					{
						this.SendEmailMessage(reminder, profile, contact);
						RecordSendEmailHistory(reminder, contact, profile, serverCurrentDateTime);
						reminderSent = true;
					}

					if (contact.SendSMS)
					{
						this.SendSMSMessage(reminder, profile, contact);
						RecordSendSMSHistory(reminder, contact, profile, serverCurrentDateTime);
						reminderSent = true;
					}

					if (reminderSent)
					{
						RecordReminderSent(reminder);	
					}
				}
			}
			return reminderSent;
		}

		private string MessageToSend(Reminder reminder, Profile profile, Contact contact)
		{
			string message = null; 
			if (reminder.Recurrence == "Daily")
			{
				message = string.Format("Hi {0}, <br/> This is a reminder for you to {1} at {2}. <br/> Sincerely,<br/> {3}", contact.FirstName.Trim(), reminder.Message, reminder.ReminderDateTime.ToShortTimeString(), profile.FirstName);
			}
			return message;
		}

		private void SendEmailMessage(Reminder reminder, Profile profile, Contact contact)
		{
			string fromEmailAddress = profile.EmailAddress;
			string toEmailAddress = contact.EmailAddress;
			string emailSubject = string.Format("Reminder from {0} {1} - {2}", profile.FirstName, profile.LastName, DateTime.Now.ToString());
			string emailBody = MessageToSend(reminder, profile, contact);
			var emailMessage = new MessageEmail();
			emailMessage.Send(fromEmailAddress, toEmailAddress, emailSubject, emailBody);
		}

		private void SendSMSMessage(Reminder reminder, Profile profile, Contact contact)
		{
			string fromPhoneNumber = profile.PhoneNumberIssued;
			string toPhoneNumber = string.Format("1{0}", contact.PhoneNumber);
			string message = MessageToSend(reminder, profile, contact);

			string AccountSid = profile.AccountSid;
			string AuthToken = profile.AuthToken;

			var twilio = new TwilioRestClient(AccountSid, AuthToken);

			var messageSent = twilio.SendSmsMessage(fromPhoneNumber, toPhoneNumber, message, "");
		}

		private void RecordReminderSent(Reminder reminder)
		{
			_db.Reminders.ToList().Find(r => r.Id == reminder.Id).Sent = true;
			_db.Save();
		}

		private void RecordSendEmailHistory(Reminder reminder, Contact contact, Profile profile, DateTime serverCurrentDateTime)
		{
			var reminderHistory = new ReminderHistory();
			reminderHistory.ContactId = contact.Id;
			reminderHistory.Message = reminder.Message;
			reminderHistory.ProfileId = profile.Id;
			reminderHistory.ReminderDateTime = reminder.ReminderDateTime;
			reminderHistory.ReminderId = reminder.Id;
			reminderHistory.EmailSent = true;
			reminderHistory.SMSSent = false;
			reminderHistory.MessageSentDateTime = serverCurrentDateTime;
			_db.ReminderHistories.Add(reminderHistory);
			_db.Save();
		}

		private void RecordSendSMSHistory(Reminder reminder, Contact contact, Profile profile, DateTime serverCurrentDateTime)
		{
			var reminderHistory = new ReminderHistory();
			reminderHistory.ContactId = contact.Id;
			reminderHistory.Message = reminder.Message;
			reminderHistory.ProfileId = profile.Id;
			reminderHistory.ReminderDateTime = reminder.ReminderDateTime;
			reminderHistory.ReminderId = reminder.Id;
			reminderHistory.EmailSent = false;
			reminderHistory.SMSSent = true;
			reminderHistory.MessageSentDateTime = serverCurrentDateTime;
			_db.ReminderHistories.Add(reminderHistory);
			_db.Save();
		}

		private bool SendReminderAllowed(Reminder reminder)
		{
			var profile = _db.Profiles.ToList().Find(p => p.Id == reminder.ProfileId);
			var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();
			if (contact.Active && !profile.DeActivate)
			{
				return true;
			}
			
			return false;
		}

		private DateTime ServerCurrentDateTime(Reminder reminder)
		{
			DateTime currentDateTime;
			var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();

			#if DEBUG
				currentDateTime = DateTime.Now;
			#else
				int prodServerTimeDifference = 0;

				switch (contact.TimeZone)
				{
					case "PST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["PSTOffSetHours"]); break;
					case "MST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["MSTOffSetHours"]); break;
					case "CST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["CSTOffSetHours"]); break;
					case "EST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["ESTOffSetHours"]); break;
				}
				currentDateTime = DateTime.Now.AddHours(prodServerTimeDifference);
			#endif

				return currentDateTime;
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
						case "PST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["PSTOffSetHours"]); break;
						case "MST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["MSTOffSetHours"]); break;
						case "CST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["CSTOffSetHours"]); break;
						case "EST": prodServerTimeDifference = Convert.ToInt32(ConfigurationManager.AppSettings["ESTOffSetHours"]); break;
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
