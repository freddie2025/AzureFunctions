using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System.Text;

namespace SendMail
{
	public static class SendMailFunction
    {
		[FunctionName("SendEmail")]
		public static void Run(
			[ServiceBusTrigger("myqueue", Connection = "ServiceBusConnection")] Message email,
			[SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] out SendGridMessage message)
		{
			var emailObject = JsonConvert.DeserializeObject<OutgoingEmail>(Encoding.UTF8.GetString(email.Body));

			message = new SendGridMessage();
			message.AddTo(emailObject.To);
			message.AddContent("text/html", emailObject.Body);
			message.SetFrom(new EmailAddress(emailObject.From));
			message.SetSubject(emailObject.Subject);
		}

		public class OutgoingEmail
		{
			public string To { get; set; }
			public string From { get; set; }
			public string Subject { get; set; }
			public string Body { get; set; }
		}
	}
}
