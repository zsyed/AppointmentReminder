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
			string userName = User.Identity.Name;
			var profile = _db.GetProfile(userName);
			return profile;
		}

		public HttpResponseMessage Put([FromBody] Profile profile)
		{
			try
			{
				var existProfile = _db.Profiles.ToList().Find(p => p.Id == profile.Id);
				existProfile.FirstName = profile.FirstName;
				existProfile.LastName = profile.LastName;
				existProfile.EmailAddress = profile.EmailAddress;
				existProfile.PhoneNumber = profile.PhoneNumber;
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
