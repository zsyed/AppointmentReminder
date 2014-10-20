using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AppointmentReminder.Data;
using AppointmentReminder.Models;

namespace AppointmentReminder.Controllers
{
	using System.Security.Principal;
	using System.Threading;
	using System.Web;
	[System.Web.Mvc.Authorize]
	public class ProfileWebController : ApiController
    {
	    private IReminderDb _db;

		private IIdentityService _identityService;

		public ProfileWebController(IReminderDb db)
		{
			_db = db;
		}

		public object Get()
		{
			string userName = User.Identity.Name;
			var profile = _db.GetProfile(userName);
			return profile;
		}
    }
}
