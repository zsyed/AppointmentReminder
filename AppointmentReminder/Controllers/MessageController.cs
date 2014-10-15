﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
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

		public string CurrentDateTimeValue()
		{
#if DEBUG
			return DateTime.Now.ToString();
#else
			return DateTime.Now.AddHours(-7.0).ToString();
#endif
		}

		public JsonResult Send()
		{
			var contactList = new List<SelectListItem>();
			try
			{
				var reminders = new ReminderDb().Reminders;
				

				foreach (var reminder in reminders)
				{
					TimeSpan timeDifference;
#if DEBUG
					timeDifference = reminder.ReminderDateTime - DateTime.Now;
#else
					timeDifference = reminder.ReminderDateTime - DateTime.Now.AddHours(-7.0);
#endif

					int RemdinerHours = Convert.ToInt32(ConfigurationManager.AppSettings["ReminderHours"]);


					if (timeDifference.Seconds > 0 && timeDifference.Hours <= RemdinerHours && reminder.ReminderDateTime.Date.Equals(DateTime.Now.Date) && !reminder.Sent)
					{
						var contact = new ReminderDb().Contacts.Where(c => c.Id == reminder.ContactId).FirstOrDefault();
						var profile = _db.Profiles.Where(p => p.Id == contact.ProfileId).FirstOrDefault();
						if (contact.Active)
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
								reminderHistory.EmailSent = true;
								reminderHistory.Message = reminder.Message;
								reminderHistory.ProfileId = profile.Id;
								reminderHistory.ReminderDateTime = reminder.ReminderDateTime;
								reminderHistory.ReminderId = reminder.Id;
								reminderHistory.SMSSent = false;
								reminderHistory.MessageSentDateTime = DateTime.Now;
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
								reminderHistory.EmailSent = false;
								reminderHistory.Message = reminder.Message;
								reminderHistory.ProfileId = profile.Id;
								reminderHistory.ReminderDateTime = reminder.ReminderDateTime;
								reminderHistory.ReminderId = reminder.Id;
								reminderHistory.SMSSent = true;
								reminderHistory.MessageSentDateTime = DateTime.Now;
								_db.ReminderHistories.Add(reminderHistory);
								_db.Save();
							}

							if (reminderSent)
							{
								_db.Reminders.Where(r => r.Id == reminder.Id).FirstOrDefault().Sent = true;
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
			string fromPhoneNumber = ConfigurationManager.AppSettings["TwilioNumber"];
			string toPhoneNumber = string.Format("1{0}", contact.PhoneNumber);
			string message = string.Format("Hi {0}, This is a reminder for you to {1} at {2}. Sincerely, {3}", contact.FirstName.Trim(), reminder.Message, reminder.ReminderDateTime.ToString(), profile.FirstName);

			string AccountSid = ConfigurationManager.AppSettings["AccountSid"]; 
			string AuthToken = ConfigurationManager.AppSettings["AuthToken"]; 

			var twilio = new TwilioRestClient(AccountSid, AuthToken);

			var messageSent = twilio.SendSmsMessage(fromPhoneNumber, toPhoneNumber, message, "");

			// todo: Log sent Email Message to the table as history.
		}

    }
}
