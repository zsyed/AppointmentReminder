using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	public class ContactModel
	{
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }

		[HiddenInput(DisplayValue = false)]
		public int ProfileId { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string EmailAddress { get; set; }

		[Required]
		public string TimeZone { get; set; }

		[Required]
		public bool Active { get; set; }

		[Required]
		public bool SendEmail { get; set; }

		[Required]
		public bool SendSMS { get; set; }
	}
}