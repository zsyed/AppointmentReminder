using System.Collections.Generic;
using System.Web.Mvc;

namespace Appointmentcontact.Controllers
{
	using AppointmentReminder.Data;

	[Authorize]
	public class ContactController : Controller
    {
		private IReminderDb _db;

		public ContactController(IReminderDb db)
		{
			_db = db;
		}

        //
        // GET: /contact/

        public ActionResult Index()
        {
			return View();
        }

        //
        // GET: /contact/Create

        public ActionResult Create()
        {
	        return this.View();
	        //var contactModel = new ContactModel();
	        //contactModel.ProfileId = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault().Id;
	        //return View(contactModel);
        }

        //
        // POST: /contact/Create

		//[HttpPost]
		//public ActionResult Create(ContactModel contactModel)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_db.Contacts.Add(new Contact() 
		//			{ 
		//				FirstName = contactModel.FirstName, 
		//				LastName = contactModel.LastName, 
		//				Active = contactModel.Active, 
		//				TimeZone = contactModel.TimeZone,
		//				PhoneNumber = contactModel.PhoneNumber,
		//				EmailAddress = contactModel.EmailAddress,
		//				ProfileId = contactModel.ProfileId,
		//				SendEmail = contactModel.SendEmail,
		//				SendSMS = contactModel.SendSMS
		//			}
		//		);
		//		_db.Save();
		//		return RedirectToAction("Index");
		//	}

		//	return this.View(contactModel);
		//}

		public ActionResult Edit()
		{
			return View();
		}

        //
        // GET: /contact/Delete/5

        public ActionResult Delete(int id)
        {
			//var contact = _db.Contacts.Where(c => c.Id == id).FirstOrDefault();
			//var contactModel = new ContactModel()
			//{
			//	Id = contact.Id,
			//	FirstName = contact.FirstName,
			//	ProfileId = contact.ProfileId,
			//	LastName = contact.LastName,
			//	TimeZone = contact.TimeZone,
			//	PhoneNumber = contact.PhoneNumber,
			//	EmailAddress = contact.EmailAddress,
			//	SendEmail = contact.SendEmail,
			//	SendSMS = contact.SendSMS 
			//};
			//return View(contactModel);
	        return this.View();
        }

        //
        // POST: /contact/Delete/5

		//[HttpPost]
		//public ActionResult Delete(ContactModel contactModel)
		//{

		//	var contact = _db.Contacts.Where(c => c.Id == contactModel.Id).FirstOrDefault();
		//	_db.Contacts.Remove(contact);
		//	_db.Save();

		//	return RedirectToAction("Index");

		//}

		//public JsonResult GetContacts()
		//{
		//	var profile = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
		//	var contacts = new ReminderDb().Contacts.Where(c => c.ProfileId == profile.Id).OrderBy(c => c.LastName);
		//	var contactList = new List<SelectListItem>();
		//	foreach (var contact in contacts)
		//	{
		//		contactList.Add(new SelectListItem() { Text = string.Format("{0} {1}", contact.FirstName.Trim(), contact.LastName.Trim()), Value = contact.Id.ToString().Trim() });
		//	}

		//	return Json(contactList, JsonRequestBehavior.AllowGet);

		//}

		public JsonResult GetTimeZones()
		{
			var timeZoneList = new List<SelectListItem>();
			timeZoneList.Add(new SelectListItem() { Text = "Pacific Standard Time", Value = Common.TimeZone.PST.ToString() });
			timeZoneList.Add(new SelectListItem() { Text = "Mountain Standard Time", Value = Common.TimeZone.MST.ToString() });
			timeZoneList.Add(new SelectListItem() { Text = "Central Standard Time", Value = Common.TimeZone.CST.ToString() });
			timeZoneList.Add(new SelectListItem() { Text = "Eastern Standard Time", Value = Common.TimeZone.EST.ToString() });


			return Json(timeZoneList, JsonRequestBehavior.AllowGet);

		}

    }
}
