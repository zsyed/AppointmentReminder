using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AppointmentReminder.Data
{
	public class MessageEmail 
	{
		public void Send(string fromEmailAddress, string toEmailAddress, string emailSubject, string emailBody)
		{
			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
			mail.To.Add(toEmailAddress);
			mail.From = new MailAddress(fromEmailAddress, fromEmailAddress, System.Text.Encoding.UTF8);
			mail.Subject = emailSubject;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			mail.Body = emailBody;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			mail.IsBodyHtml = true;
			SmtpClient client = new SmtpClient();

			string EmailAccountAddress = ConfigurationManager.AppSettings["EmailAccountAddress"];
			string EmailAccountPassword = ConfigurationManager.AppSettings["EmailAccountPassword"];
			string EmailHost = ConfigurationManager.AppSettings["EmailHost"];
			int EmailPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);

			client.Credentials = new System.Net.NetworkCredential(EmailAccountAddress, EmailAccountPassword);
			client.Port = EmailPort;
			client.Host = EmailHost;

			client.EnableSsl = true; 
			client.Send(mail);
		}
	}
}