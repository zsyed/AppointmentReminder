using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentReminder.Controllers
{
	using System.Configuration;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Security.Claims;
	using System.Security.Principal;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	using AppointmentReminder.Data;
	using AppointmentReminder.Models;

	using Newtonsoft.Json;

	[Authorize]
	public class ProfileController : Controller
    {
		private IProfileModel _profileModel;
		private IReminderDb _db;

		public ProfileController(IReminderDb db, IProfileModel profileModel)
		{
			_profileModel = profileModel;
			_db = db;
		}

		//
        // GET: /Profile/

        public ViewResult Index()
        {
			return this.View();
        }

        //
        // GET: /Profile/Create

        public ViewResult Create()
        {
	        // var profileModel = new ProfileModel();
			return View(_profileModel);
        }

        //
        // POST: /Profile/Create

        [HttpPost]
        public ActionResult Create(IProfileModel profileModel)
        {
			if (ModelState.IsValid)
			{
				_db.Profiles.Add(
					new Profile()
						{
							FirstName = profileModel.FirstName,
							LastName = profileModel.LastName,
							UserName = User.Identity.Name,
							PhoneNumber = profileModel.PhoneNumber,
							EmailAddress = profileModel.EmailAddress
						});
				_db.Save();
				var profile = _db.GetProfile(User.Identity.Name);
				profileModel.Id = profile.Id;
				return View("Index", profileModel);
			}

	        return this.View(profileModel);

        }

        //
        // GET: /Profile/Edit/5

        public ViewResult Edit()
        {
			var profile = _db.GetProfile(User.Identity.Name);
			if (profile != null)
			{
				_profileModel.Id = profile.Id;
				_profileModel.FirstName = profile.FirstName;
				_profileModel.LastName = profile.LastName;
				_profileModel.PhoneNumber = profile.PhoneNumber;
				_profileModel.EmailAddress = profile.EmailAddress;
			}
			return this.View(_profileModel);
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        public ViewResult Edit(IProfileModel profileModel)
        {
			if (ModelState.IsValid)
			{
				var model = _db.Profiles.Where(p => p.Id == profileModel.Id).FirstOrDefault();
				model.FirstName = profileModel.FirstName;
				model.LastName = profileModel.LastName;
				model.PhoneNumber = profileModel.PhoneNumber;
				model.EmailAddress = profileModel.EmailAddress;
				_db.Save();

                return View("Index", profileModel);
            }
			return this.View(profileModel);

        }
    }


	public class BasicAuthHandler : DelegatingHandler
	{
		private const string BasicAuthResponseHeaderValue = "Basic";
		private const string Realm = "Apress";

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			bool identified = false;
			if (request.Headers.Authorization != null && string.Equals(request.Headers.Authorization.Scheme, BasicAuthResponseHeaderValue, StringComparison.CurrentCultureIgnoreCase))
			{
				var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(request.Headers.Authorization.Parameter));
				var user = credentials.Split(':')[0].Trim();
				var pwd = credentials.Split(':')[1].Trim();

				//validate username and password here and set identified flag
				//omitted for brevity

				if (identified)
				{
					var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user) }, BasicAuthResponseHeaderValue);
					request.GetRequestContext().Principal = new ClaimsPrincipal(new[] { identity });
				}
			}

			if (!identified)
			{
				var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
				unauthorizedResponse.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(BasicAuthResponseHeaderValue, Realm));
				return Task.FromResult(unauthorizedResponse);
			}

			return base.SendAsync(request, cancellationToken);
		}
	}
}
