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

		private IReminderDb _db;

		public ProfileController(IReminderDb db)
		{
			_db = db;
		}

		//
        // GET: /Profile/

        public ActionResult Index()
        {

	        var model = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
	        ProfileModel profileModel = null;
	        if (model != null)
	        {
		        profileModel = new ProfileModel()
			                           {
				                           Id = model.Id,
				                           FirstName = model.FirstName,
				                           LastName = model.LastName,
										   PhoneNumber = model.PhoneNumber,
										   EmailAddress = model.EmailAddress
			                           };
				return View(profileModel);
			}

	        return this.View(profileModel);
        }

        //
        // GET: /Profile/Create

        public ActionResult Create()
        {
	        var profileModel = new ProfileModel();
			return View(profileModel);
        }

        //
        // POST: /Profile/Create

        [HttpPost]
        public ActionResult Create(ProfileModel profileModel)
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
				return RedirectToAction("Index");
			}

	        return this.View(profileModel);

        }

        //
        // GET: /Profile/Edit/5

        public ActionResult Edit(int id)
        {
			var profile = _db.Profiles.Where(p => p.Id == id).FirstOrDefault();
	        var profileModel = new ProfileModel() 
				{ 
					Id = id, 
					FirstName = profile.FirstName, 
					LastName = profile.LastName, 
					PhoneNumber = profile.PhoneNumber, 
					EmailAddress = profile.EmailAddress
				};
			return View(profileModel);
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        public ActionResult Edit(ProfileModel profileModel)
        {
			if (ModelState.IsValid)
			{

				var model = _db.Profiles.Where(p => p.Id == profileModel.Id).FirstOrDefault();
				model.FirstName = profileModel.FirstName;
				model.LastName = profileModel.LastName;
				model.PhoneNumber = profileModel.PhoneNumber;
				model.EmailAddress = profileModel.EmailAddress;
				_db.Save();

                return RedirectToAction("Index");
            }
			return this.View(profileModel);

        }
    }
}
