using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GerenciamentoHotel.Filters;
using Timesheet.Filters;
using GerenciamentoHotel.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace GerenciamentoHotel.Controllers
{
    [Authenticated]
    [Permission]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.color="#333";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult EnviarEmail(EnviaEmail model)
        {
            RetornoEmail result = new RetornoEmail();

            string emailTo = "noahdisantini@yahoo.com.br";
            string smtpServer = "smtp.gmail.com";
            string smtpUser = "locirrs@gmail.com";
            string smtpPassword = "10355134";

            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = smtpServer,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(smtpUser,smtpPassword)
                };

                StringBuilder sb = new StringBuilder();

                sb.Append("Email - Gerenciamento de Reservas\n");
                sb.AppendFormat("De:{0}\n", model.email);
                sb.AppendFormat("Nome:{0}\n", model.fullname);
                sb.AppendFormat("Mensagem:{0}\n", model.message);
                
                MailMessage message = new MailMessage();
                message.From = new MailAddress(smtpUser, "Gerenciamento de Reservas de Hotel");
                message.To.Add(emailTo);
                message.Subject = model.subject;
                message.Body = sb.ToString();

                smtp.Send(message);
                result.retorno = "true";
            }
            catch (Exception ex)
            {
                result.retorno = "false";
            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }

}