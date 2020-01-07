using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using hr_app.Models;
using hr_app.EntityFramework;

namespace hr_app.Content
{
    public class SendGridService
    {
        private readonly DataContext _context;
        private IConfiguration _configuration;
        public SendGridService(DataContext context,  IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task  SendMailNotification(JobApplication ja)
        {
            var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("hr_app@example.com", "HrApp team");

            JobOffer jo = _context.JobOffers.Where(x => x.Id == ja.JobOfferId).First();
            User u = _context.Users.Where(x => x.Id == jo.UserId).First();
            List<EmailAddress> tos = new List<EmailAddress>
            {
                new EmailAddress(u.Email, u.Name)
            };
         

            var subject = "New job application has been submitted!";
            var htmlContent = "<strong>Job offer with id:" + jo.Id + " received new application</strong><br/>You can see the cv at: " + ja.CvUrl;
           
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, false);
            var response = await client.SendEmailAsync(msg);
        }


       
    }
}
