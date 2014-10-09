using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	public class ProfileModel 
	{
		[HiddenInput(DisplayValue=false)]
		public int Id { get; set; }
		
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string EmailAddress { get; set; }
	}
}