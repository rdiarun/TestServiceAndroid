using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace DetectorService
{
    public class Emai11lHelper
    {/// <summary>
        /// Send Mail 
        /// </summary>
        /// <param name="_FromEmail"></param>
        /// <param name="_FromName"></param>
        /// <param name="_ToEmail"></param>
        /// <param name="_ToName"></param>
        /// <param name="_Subject"></param>
        /// <param name="_EmailBody"></param>
        /// <param name="_IsBodyHtml"></param>
        /// <param name="_Attachments"></param>
        /// <param name="_EmailServer"></param>
        /// <param name="_LoginName"></param>
        /// <param name="_LoginPassword"></param>
        /// <returns></returns>
        public bool SendEmail(string _FromEmail, string _FromName, string _ToEmail, string _ToName, string _Subject, string _EmailBody, bool _IsBodyHtml, string[] _Attachments, string _EmailServer, string _LoginName, string _LoginPassword)
        {
            try
            { 
                // setup email header
                System.Net.Mail.MailMessage _MailMessage = new System.Net.Mail.MailMessage();

                // Set the message sender
                // sets the from address for this e-mail message.
                _MailMessage.From = new System.Net.Mail.MailAddress(_FromEmail, _FromName);
                // Sets the address collection that contains the recipients of this e-mail message.
                _MailMessage.To.Add(new System.Net.Mail.MailAddress(_ToEmail, _ToName));

                // sets the message subject.
                _MailMessage.Subject = _Subject;
                // sets the message body.
                _MailMessage.Body = _EmailBody;
                // sets a value indicating whether the mail message body is in Html.
                // if this is false then ContentType of the Body content is "text/plain".
                _MailMessage.IsBodyHtml = _IsBodyHtml;
                _MailMessage.Priority = MailPriority.Normal;

                // add all the file attachments if we have any
                if (_Attachments != null && _Attachments.Length > 0)
                    foreach (string _Attachment in _Attachments)
                        _MailMessage.Attachments.Add(new System.Net.Mail.Attachment(_Attachment));

                // SmtpClient Class Allows applications to send e-mail by using the Simple Mail Transfer Protocol (SMTP).
                System.Net.Mail.SmtpClient _SmtpClient = new System.Net.Mail.SmtpClient(_EmailServer);
                //System.Net.Mail.SmtpClient _SmtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //_SmtpClient.Port = 25;
                //Specifies how email messages are delivered. Here Email is sent through the network to an SMTP server.
                _SmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                // Some SMTP server will require that you first authenticate against the server.
                // Provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.
                System.Net.NetworkCredential _NetworkCredential = new System.Net.NetworkCredential(_LoginName, _LoginPassword);

                _SmtpClient.UseDefaultCredentials = false;
                _SmtpClient.Credentials = _NetworkCredential;
                //_SmtpClient.EnableSsl = true;


                //Let's send it
              _SmtpClient.Send(_MailMessage);

                // Do cleanup
                _MailMessage.Dispose();
                _SmtpClient = null;
                return true;
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception((ex.Message + (SessionVariables.InSessionUser != null ? "#" + SessionVariables.InSessionUser.OrganizationID + "##" + SessionVariables.InSessionUser.Staff_ID : ""))));
                return false;
            }


        }
    }
}