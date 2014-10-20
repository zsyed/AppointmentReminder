using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentReminder.Data
{
	using System.Data.Entity;

	using AppointmentReminder.Models;

	public class ReminderDb : DbContext, IReminderDb
	{
		public ReminderDb() :base("DefaultConnection")
		{
			
		}
		
		public DbSet<Profile> Profiles { get; set; }

		public DbSet<Reminder> Reminders { get; set; }

		public DbSet<Contact> Contacts { get; set; }

		public DbSet<ReminderHistory> ReminderHistories { get; set; }

		public Profile GetProfile(string userName)
		{
			return Profiles.ToList().Find(p => p.UserName == userName);
		}

		public Profile GetProfile(int Id)
		{
			return Profiles.Where(p => p.Id == Id).FirstOrDefault();
		}

		void IReminderDb.Save()
		{
			this.SaveChanges();
		}
	}
}