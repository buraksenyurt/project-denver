using Denver.Common;
using Denver.Configuration;
using Denver.DAL;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;

namespace Denver.Tools
{
    public class MailUtiliy
    {
        private static string host = ConfigurationManager.AppSettings["SMTPHost"];
        private static string port = ConfigurationManager.AppSettings["SMTPPort"];

        public static void SendEmailToBoss(int Code, int Count)
        {            
            try
            {
                using (SmtpClient client = new SmtpClient(host, Convert.ToInt32(port)))
                {
                    MailMessage mailMessage = new MailMessage("billing@denver.com", ConfigurationManager.AppSettings["BossMail"]);
                    mailMessage.Body = String.Format(File.ReadAllText(@"C:\Projects\project-denver\Assets\Templates\Mail_Temp_Boss.txt"), Count.ToString(), Code.ToString());
                    mailMessage.IsBodyHtml = true;
                    client.Send(mailMessage);
                }
            }
            catch (Exception excp)
            {
                ExceptionManager excpManager = new ExceptionManager();
                excpManager.Error(string.Format(excp.Message));
            }
        }

        public static void SendEmailToManager(int currentUserId,int Code, int Count)
        {
            DALPerson person = new DALPerson();
            Person manager= person.FindManager(currentUserId);
            try
            {
                using (SmtpClient client = new SmtpClient(host, Convert.ToInt32(port)))
                {
                    MailMessage mailMessage = new MailMessage("billing@denver.com", manager.Email);
                    mailMessage.Body = String.Format(File.ReadAllText(@"C:\Projects\project-denver\Assets\Templates\Mail_Temp_Manager.txt"), manager.LastName, Count.ToString(), Code.ToString());
                    mailMessage.IsBodyHtml = true;
                    client.Send(mailMessage);
                }
            }
            catch (Exception excp)
            {
                ExceptionManager excpManager = new ExceptionManager();
                excpManager.Error(string.Format(excp.Message));
            }
        }

        public static void SendEmailToUnitHead(int currentUserId,int Code, int Count)
        {
            DALPerson person = new DALPerson();
            Person unitHead = person.FindUnitHead(currentUserId);
            try
            {
                using (SmtpClient client = new SmtpClient(host, Convert.ToInt32(port)))
                {
                    MailMessage mailMessage = new MailMessage("billing@denver.com", unitHead.Email);
                    mailMessage.Body = String.Format(File.ReadAllText(@"C:\Projects\project-denver\Assets\Templates\Mail_Temp_UnitHead.txt"), unitHead.LastName, Count.ToString(), Code.ToString());
                    mailMessage.IsBodyHtml = true;
                    client.Send(mailMessage);
                }
            }
            catch (Exception excp)
            {
                ExceptionManager excpManager = new ExceptionManager();
                excpManager.Error(string.Format(excp.Message));
            }
        }

        public static bool SendRequest(int code, int number, double price, int stockCount, string name, int quantity, string supplier)
        {
            string serviceAddress=ServicesConfig.goverment_central_bank_address;

            // BrasilBank'a bilgilendirme işlemi 2013 kanun değişkliğine göre iptal edilmiştir.

            return true;
        }
    }
}
