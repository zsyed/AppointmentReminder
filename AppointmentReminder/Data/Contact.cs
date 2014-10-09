using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Data
{
	public class Contact
	{
		public virtual int Id { get; set; }

		public virtual int ProfileId { get; set; }

		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual string EmailAddress { get; set; }

		public virtual bool Active { get; set; }

		public virtual bool SendEmail { get; set; }

		public virtual bool SendSMS { get; set; }
	}
}