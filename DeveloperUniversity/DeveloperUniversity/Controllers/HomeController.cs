using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using DeveloperUniversity.Models.ViewModels;
using reCAPTCHA.MVC;

namespace DeveloperUniversity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult EmailError()
        {
            return View();
        }


        [HttpPost]
        [CaptchaValidator(
            PrivateKey = "6LehwAgUAAAAACXHKuFtJmdZWWPvf7--XsaZiRsw",
            ErrorMessage = "Invalid input captcha.",
            RequiredMessage = "The captcha field is required.")]
        public ActionResult Contact(MailViewModel viewModel)
        {
            //For help on setting up the captcha visit the link below and refer to the "Quick Start" guide.
            //http://recaptchamvc.apphb.com/

            //To get your site and private keys for the captcha, visit the link below.
            // https://www.google.com/recaptcha/intro/index.html

            //If you receive an error on Line smtp.Send(mail) then you might need to log into that Gmail 
            //account you are trying to send the emails from and find the option to "Enable Less Secure Apps"
            //Google is being nice and disabling the Gmail account to be accessed by less secure applications.

            //See the referenced code for explanation of this example.
            //http://www.c-sharpcorner.com/uploadfile/sourabh_mishra1/sending-an-e-mail-using-asp-net-mvc/

            var IsDebug = false;

            #if DEBUG
                IsDebug = true;
            #endif

            var toEmail = "adelantehispanic@gmail.com";
            var toEmailPassword = "Adelante";

            //Debug SMTP Settings
            var debugSmtpClient = new SmtpClient();
            debugSmtpClient.Host = "smtp.gmail.com";
            debugSmtpClient.Port = 587;
            debugSmtpClient.EnableSsl = true;
            //Setup credentials to login to our sender email address("UserName", "Password")
            NetworkCredential credentials = new NetworkCredential(toEmail, toEmailPassword);
            debugSmtpClient.UseDefaultCredentials = false;
            debugSmtpClient.Credentials = credentials;

            //Release SMTP Settings
            var releaseSmtpClient = new SmtpClient();
            releaseSmtpClient.EnableSsl = false;
            releaseSmtpClient.Host = "relay-hosting.secureserver.net";
            releaseSmtpClient.Port = 25;


            if (ModelState.IsValid || (ModelState.IsValid == false && IsDebug))            
            {
                string Body = viewModel.Messge;

                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.From = new MailAddress(viewModel.Email);
                mail.Subject = "Contact Form Submission";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
#if DEBUG
                smtp = debugSmtpClient;
#else
                smtp = releaseSmtpClient;
#endif
                try
                {
                    smtp.Send(mail);
                }
                catch (SmtpException)
                {
                    return View("EmailError");
                }
            return View("Index");
            }
            else
            {
                return View();
            }
        }
    }
}