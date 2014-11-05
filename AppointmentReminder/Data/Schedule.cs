namespace AppointmentReminder.Data
{
	using System;

	public class Schedule
	{
		public virtual int Id { get; set; }

		public virtual DateTime StartingOnDateTime { get; set; }

		public virtual string Recurrence { get; set; }

		public virtual int RecurEvery { get; set; }

		public virtual string RecurType { get; set; }

		public virtual DateTime EndingOnDateTime { get; set; }

		public virtual int EndingOnEndAfterOccurences { get; set; }

		public virtual bool EndingOnNoEndDate { get; set; }

		public virtual int ContactId { get; set; }

		public virtual int ReminderId { get; set; }
	}
}