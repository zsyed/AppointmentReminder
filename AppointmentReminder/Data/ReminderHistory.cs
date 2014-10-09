using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Data
{
	public class ReminderHistory
	{
		public virtual int Id { get; set; }

		public virtual int ReminderId { get; set; }

		public virtual int ProfileId { get; set; }

		public virtual DateTime ReminderDateTime { get; set; }

		public virtual string Message { get; set; }

		public virtual int ContactId { get; set; }

		public virtual bool EmailSent { get; set; }

		public virtual bool SMSSent { get; set; }

		public virtual DateTime MessageSentDateTime { get; set; }
	}
}