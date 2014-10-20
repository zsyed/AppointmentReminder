using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Data
{
	using System.Threading;
	using System.Web.Mvc;

	[Authorize]
	public class IdentityService : IIdentityService
	{
		private string _currentUser;

		public IdentityService()
		{
			_currentUser = Thread.CurrentPrincipal.Identity.Name; // new Controllers.UserContoller().UserName; //"zsyed";
		}

		public string CurrentUser
		{
			get
			{
				return _currentUser;
			}
		}
	}
}