using System.Web.Http;
using AppointmentReminder.Data;

namespace AppointmentReminder.Controllers
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Net.Http;

	[System.Web.Mvc.Authorize]
	public class ProfileWebController : ApiController
    {
	    private IReminderDb _db;

		public ProfileWebController(IReminderDb db)
		{
			_db = db;
		}

		public Profile Get()
		{
			string userName = "zsyed"; //  User.Identity.Name;
			var profile = _db.GetProfile(userName);
			return profile;
		}

		public HttpResponseMessage Put([FromBody] Profile profile)
		{
			try
			{
				var dbProfile = _db.Profiles.ToList().Find(p => p.Id == profile.Id);
				dbProfile.FirstName = profile.FirstName;
				dbProfile.LastName = profile.LastName;
				dbProfile.EmailAddress = profile.EmailAddress;
				dbProfile.PhoneNumber = profile.PhoneNumber;
				dbProfile.DeActivate = profile.DeActivate;
				_db.Save();
				return Request.CreateResponse(HttpStatusCode.Created, profile);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}
		}

		public HttpResponseMessage Post([FromBody]Profile newProfile)
		{
			try
			{
				newProfile.UserName = User.Identity.Name;
				newProfile.DeActivate = false;
				_db.Profiles.Add(newProfile);
				_db.Save();
				return Request.CreateResponse(HttpStatusCode.Created, newProfile);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}
		}
    }
}
