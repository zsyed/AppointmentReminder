using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Data
{
	public class Profile
	{
		public virtual int Id { get; set; }
			   
		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual string UserName { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual string EmailAddress { get; set; }
	}
}