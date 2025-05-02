using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionFirst
{
    public class EmailTimerFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public EmailTimerFunction(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _logger = loggerFactory.CreateLogger<EmailTimerFunction>();
            _config = config;
        }

        [Function("EmailTimerFunction")]
        public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var smtp = _config["SMTP"];
            var port = int.Parse(_config["MailPort"]);
            var username = _config["MailUsername"];
            var password = _config["Password"];
            var fromAddress = _config["MailAddress"];

            _logger.LogInformation(new {smtp, port, username, password, fromAddress}.ToString());
            var client = new SmtpClient();

            var message = new MailMessage()
            {
                From = new MailAddress(fromAddress),
                Body = "This is a test message from azure function",
                Subject = "Azure function",
                IsBodyHtml = false
            };

            message.To.Add(fromAddress);

            var credentials = new NetworkCredential(username, password);

            client.Credentials = credentials;

            client.EnableSsl = true;
            client.Host = smtp;
            client.Port = port;


            client.Send(message);

        }
    }
}
