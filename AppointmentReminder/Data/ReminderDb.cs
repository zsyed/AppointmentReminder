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

		void IReminderDb.Save()
		{
			this.SaveChanges();
		}
	}
}