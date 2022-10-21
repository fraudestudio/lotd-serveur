using System.Collections.Concurrent;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Server.Utils
{
	public class EMail : MimeMessage, IDisposable
	{
		static private SmtpClient _smtpClient;
		static private BlockingCollection<EMail> _messageQueue = new BlockingCollection<EMail>();
		static private String _ADDRESS = Environment.GetEnvironmentVariable("BOT_EMAIL_ADDRESS");
		static private String _PASSWORD = Environment.GetEnvironmentVariable("BOT_EMAIL_PASSWORD");

		static public void SendMessages()
		{
			EMail._smtpClient = new SmtpClient();
			EMail._smtpClient.Connect("iut-dijon.u-bourgogne.fr", 25, SecureSocketOptions.StartTls);
			EMail._smtpClient.Authenticate(EMail._ADDRESS, EMail._PASSWORD);
			
			while (true)
			{
				Console.WriteLine("Waiting for emails to send ...");
				EMail message = EMail._messageQueue.Take();
				try {
					Console.WriteLine("Sending email ...");
					EMail._smtpClient.Send(message);
					Console.WriteLine("Email sent successfully");
				}
				catch (Exception e)
				{
					Console.WriteLine("Couldn't send email: {0}", e);
				}
			}
		}

		public EMail(String recipient, String subject, String body) : base ()
		{
			this.From.Add(new MailboxAddress("Lord Of The Dungeons", EMail._ADDRESS));
			this.To.Add(MailboxAddress.Parse(recipient));

			this.Subject = subject;

   			this.Body = new TextPart(TextFormat.Html) { Text = body };

			if (EMail._smtpClient == null)
			{
				
			}
		}

		public void Send()
		{
			Console.WriteLine("Email is ready to be sent");
			EMail._messageQueue.Add(this);
		}
	}
}