using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace Parichay.MVC.Controllers.Helpers
{
    public class Email
    {
        private static readonly string AdminEmail = "webadmin@parichay.codeplex.com"; //ConfigurationManager.AppSettings["adminEmail"];
        private static readonly string AdminName = "Parichay Admin"; //ConfigurationManager.AppSettings["adminName"];
        private MailMessage m;
        private bool isDbMail = false;
        private int numOfParams = 0;
        private string[] _params;

        public Email()
        {
            m = new MailMessage();
            m.From = new MailAddress(AdminEmail, AdminName);
            m.IsBodyHtml = true;
        }

        public Email(string from, string to, string subject, string body, bool isBodyHtml)
        {
            m = new MailMessage();
            m.Sender = m.From = new MailAddress(from);
            m.To.Add(to);
            m.Subject = subject;
            m.Body = body;
            m.IsBodyHtml = isBodyHtml;
        }

        public bool isHTML
        {
            set
            {
                m.IsBodyHtml = value;
            }
        }

        public string to
        {
            set
            {
                m.To.Add(value);
            }
        }

        public string from
        {
            set
            {
                m.Sender = m.From = new MailAddress(value);
            }
        }
        public string subject
        {
            set
            {
                m.Subject = value;
            }
        }

        public string body
        {
            get
            {
                return body;
            }
            set
            {
                m.Body = value;

            }
        }

        public void send()
        {
            if (isDbMail)
            {
                if (!checkArr(_params))
                {
                    throw new Exception("Not sufficient number of parameters filled.");
                }
                else
                {
                    m.Body = string.Format(m.Body, _params);
                }
            }

            try
            {
                SmtpClient mailClient = new SmtpClient();

                //********Remember to comment out these settings once you configure your mail client********//
                mailClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                mailClient.PickupDirectoryLocation = Utilities.GetPhysicalLocation("/Mails");
                //********Remember to comment out these settings once you configure your mail client********//

                mailClient.Send(m);
            }
            catch //(Exception ex)
            {
                throw;
                //TextLog(ex.Message);
            }
        }

        public string[] dbMailParams
        {
            get { return (_params); }
            set { _params = value; }
        }

        private void initArr(string[] arr)
        {
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                arr[i] = "";
            }
        }
        private bool checkArr(string[] arr)
        {
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(arr[i]))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            string str = @"From: {0}, To: {1}, Subject: {2}, Body {3}, IsHTML: {4}, base: {5}";
            str = string.Format(str, m.From, m.To, m.Subject, m.Body, m.IsBodyHtml.ToString(), base.ToString());
            return (str);
        }
    }
}