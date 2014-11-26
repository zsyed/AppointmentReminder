////using System.Collections.Generic;
////using System.Web.Mvc;

////namespace Appointmentcontact.Controllers
////{
////	using AppointmentReminder.Data;

////	[Authorize]
////	public class ContactController : Controller
////	{
////		private IReminderDb _db;

////		public ContactController(IReminderDb db)
////		{
////			_db = db;
////		}

////		//
////		// GET: /contact/

////		public ActionResult Index()
////		{
////			return View();
////		}

////		//
////		// GET: /contact/Create

////		public ActionResult Create()
////		{
////			return this.View();
////		}

////		public ActionResult Edit()
////		{
////			return View();
////		}

////		//
////		// GET: /contact/Delete/5

////		public ActionResult Delete(int id)
////		{
////			return this.View();
////		}

////		//
////		// POST: /contact/Delete/5

////		public JsonResult GetTimeZones()
////		{
////			var timeZoneList = new List<SelectListItem>();
////			timeZoneList.Add(new SelectListItem() { Text = "Pacific Standard Time", Value = Common.TimeZone.PST.ToString() });
////			timeZoneList.Add(new SelectListItem() { Text = "Mountain Standard Time", Value = Common.TimeZone.MST.ToString() });
////			timeZoneList.Add(new SelectListItem() { Text = "Central Standard Time", Value = Common.TimeZone.CST.ToString() });
////			timeZoneList.Add(new SelectListItem() { Text = "Eastern Standard Time", Value = Common.TimeZone.EST.ToString() });


////			return Json(timeZoneList, JsonRequestBehavior.AllowGet);

////		}

////	}
////}
