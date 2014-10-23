namespace AppointmentReminder.Data
{
	using System.Data.Entity;

	public interface IReminderDb
	{
		DbSet<Profile> Profiles { get; set; }

		DbSet<Reminder> Reminders { get; set; }

		DbSet<Contact> Contacts { get; set; }

		DbSet<ReminderHistory> ReminderHistories { get; set; }

		Profile GetProfile(string userName);

		Profile GetProfile(int id);

		void Save();
	}
}