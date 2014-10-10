using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Models
{
	public class ReminderHistoryModel
	{
		public int Id { get; set; }
		public DateTime ReminderDateTime { get; set; }
		public string Message { get; set; }
		public int ContactId { get; set; }
		public string ContactName { get; set; }
		public bool EmailSent { get; set; }
		public bool SMSSent { get; set; }
		public DateTime MessageSentDateTime { get; set; }
	}
}