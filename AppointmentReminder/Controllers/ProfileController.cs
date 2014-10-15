using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentReminder.Controllers
{
	using AppointmentReminder.Data;
	using AppointmentReminder.Models;

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
			string userName = User.Identity.Name;
			var profile = _db.GetProfile(userName); 
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

        public ViewResult Edit(int id)
        {
			var profile = _db.GetProfile(id);
			if (profile != null)
			{
				_profileModel.Id = id;
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
}
