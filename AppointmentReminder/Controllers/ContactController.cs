using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appointmentcontact.Controllers
{
	using System.Text.RegularExpressions;

	using AppointmentReminder.Data;
	using AppointmentReminder.Models;

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
	        var profile = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
	        var contacts = new ReminderDb().Contacts.Where(c => c.ProfileId == profile.Id).OrderBy(c => c.LastName);
	        var contactsModel = new List<ContactModel>();
			foreach (var contact in contacts)
			{
				contactsModel.Add(new ContactModel() 
					{ 
						Id = contact.Id, 
						FirstName = contact.FirstName, 
						LastName = contact.LastName, 
						Active = contact.Active, 
						PhoneNumber = contact.PhoneNumber+"1",
						EmailAddress = contact.EmailAddress,
						SendEmail = contact.SendEmail,
						SendSMS = contact.SendSMS
					}
				);
			}
			return View(contactsModel);
        }

        //
        // GET: /contact/Create

        public ActionResult Create()
        {
			var contactModel = new ContactModel();
	        contactModel.ProfileId = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault().Id;
			return View(contactModel);
        }

        //
        // POST: /contact/Create

        [HttpPost]
		public ActionResult Create(ContactModel contactModel)
        {
			if (ModelState.IsValid)
			{
				_db.Contacts.Add(new Contact() 
					{ 
						FirstName = contactModel.FirstName, 
						LastName = contactModel.LastName, 
						Active = contactModel.Active, 
						PhoneNumber = contactModel.PhoneNumber,
						EmailAddress = contactModel.EmailAddress,
						ProfileId = contactModel.ProfileId,
						SendEmail = contactModel.SendEmail,
						SendSMS = contactModel.SendSMS
					}
				);
				_db.Save();
				return RedirectToAction("Index");
			}

			return this.View(contactModel);
        }

        //
        // GET: /contact/Edit/5

		public ActionResult Edit(int id)
		{
			var contact = _db.Contacts.Where(c => c.Id == id).FirstOrDefault();

			var contactModel = new ContactModel()
				                    {
					                    Id = contact.Id,
										FirstName = contact.FirstName,
					                    ProfileId = contact.ProfileId,
										LastName = contact.LastName,
										Active = contact.Active,
										PhoneNumber = contact.PhoneNumber,
										EmailAddress = contact.EmailAddress,
										SendEmail = contact.SendEmail,
										SendSMS = contact.SendSMS										
				                    };
			return View(contactModel);
		}

        //
        // POST: /contact/Edit/5

        [HttpPost]
		public ActionResult Edit(ContactModel contactModel)
        {
			if (ModelState.IsValid)
			{

				var contact = _db.Contacts.Where(c => c.Id == contactModel.Id).FirstOrDefault();
				Regex rgx = new Regex(@"[^\d]");

				contact.FirstName = contactModel.FirstName;
				contact.LastName = contactModel.LastName;
				contact.Active= contactModel.Active;
				contact.PhoneNumber = rgx.Replace(contactModel.PhoneNumber, "");
				contact.EmailAddress = contactModel.EmailAddress;
				contact.SendEmail = contactModel.SendEmail;
				contact.SendSMS = contactModel.SendSMS;

				_db.Save();

				return RedirectToAction("Index");
			}
			return this.View(contactModel);
        }

        //
        // GET: /contact/Delete/5

        public ActionResult Delete(int id)
        {
			var contact = _db.Contacts.Where(c => c.Id == id).FirstOrDefault();
			var contactModel = new ContactModel()
			{
				Id = contact.Id,
				FirstName = contact.FirstName,
				ProfileId = contact.ProfileId,
				LastName = contact.LastName,
				PhoneNumber = contact.PhoneNumber,
				EmailAddress = contact.EmailAddress,
				SendEmail = contact.SendEmail,
				SendSMS = contact.SendSMS 
			};
			return View(contactModel);
        }

        //
        // POST: /contact/Delete/5

        [HttpPost]
        public ActionResult Delete(ContactModel contactModel)
        {

			var contact = _db.Contacts.Where(c => c.Id == contactModel.Id).FirstOrDefault();
			_db.Contacts.Remove(contact);
			_db.Save();

			return RedirectToAction("Index");

        }

		public JsonResult GetContacts()
		{
			var profile = _db.Profiles.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
			var contacts = new ReminderDb().Contacts.Where(c => c.ProfileId == profile.Id).OrderBy(c => c.LastName);
			var contactList = new List<SelectListItem>();
			foreach (var contact in contacts)
			{
				contactList.Add(new SelectListItem() { Text = string.Format("{0} {1}", contact.FirstName.Trim(), contact.LastName.Trim()), Value = contact.Id.ToString().Trim() });
			}

			return Json(contactList, JsonRequestBehavior.AllowGet);

		}

    }
}
