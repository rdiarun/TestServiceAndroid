using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
namespace DetectorService
{
    public class Common
    {           
        public static DataSet GetDataSet(string spName, string[] paramNames, object[] paramValues)
        {
            var sqlda = new SqlDataAdapter();
            var sqlcmd = new SqlCommand();
            var set = new DataSet();
            try
            {
                sqlcmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = spName;
                if (paramNames.Length > 0)
                {
                    var sqlparam = new SqlParameter();
                    for (var ctr = 0; ctr < paramNames.Length; ctr++)
                    {
                        sqlcmd.Parameters.AddWithValue(paramNames[ctr], paramValues[ctr]);
                    }
                }

                sqlda.SelectCommand = sqlcmd;
                sqlda.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (sqlcmd.Connection.State == ConnectionState.Open)
                    sqlcmd.Connection.Close();
            }
            return set;
        }
        public static SqlCommand ExecuteSp(string spName, string[] paramNames, object[] paramValues, SqlCommand sqlcmd, SqlTransaction trans)
        {
            try
            {
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Transaction = trans;
                sqlcmd.CommandText = spName;
                if (paramNames.Length > 0)
                {
                    sqlcmd.Parameters.Clear();
                    var sqlparam = new SqlParameter();
                    for (var ctr = 0; ctr < paramNames.Length; ctr++)
                    {
                        sqlcmd.Parameters.AddWithValue(paramNames[ctr], paramValues[ctr]);
                    }
                }
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sqlcmd;
        }
        public static SqlCommand ExecuteSpWithOutPut(string spName, string[] paramNames, object[] paramValues, SqlCommand sqlcmd, SqlTransaction trans)
        {
            try
            {
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Transaction = trans;
                sqlcmd.CommandText = spName;
                if (paramNames.Length > 0)
                {
                    var sqlparam = new SqlParameter();
                    sqlcmd.Parameters.Clear();
                    for (var ctr = 0; ctr < paramNames.Length; ctr++)
                    {
                        if (ctr == paramNames.Length - 1)
                        {
                            sqlcmd.Parameters.AddWithValue(paramNames[ctr], paramValues[ctr]).Direction = ParameterDirection.Output;
                        }
                        else
                        { sqlcmd.Parameters.AddWithValue(paramNames[ctr], paramValues[ctr]); }

                    }
                }
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sqlcmd;
        }
        public static string GeneratePassword()
        {
            char[] pwdNonAlhpaArray = "~!@#$%^&*()=".ToCharArray();
            string password;

            //Get a GUID
            string guid = System.Guid.NewGuid().ToString();

            //Remove  hyphens
            guid = guid.Replace("-", string.Empty);

            // Return the first length bytes
            password = guid.Substring(0, System.Web.Security.Membership.MinRequiredPasswordLength);

            //add non alpha characters
            for (int i = 0; i < System.Web.Security.Membership.MinRequiredNonAlphanumericCharacters; i++)
            {
                password += pwdNonAlhpaArray[RandomNumber(0, pwdNonAlhpaArray.Length - 1)];
            }

            return password;
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
    public class EmailHelper
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
        public static bool SendEmail(string _FromEmail, string _FromName, string _ToEmail, string _ToName, string _Subject, string _EmailBody, bool _IsBodyHtml, string[] _Attachments, string _EmailServer, string _LoginName, string _LoginPassword)
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
                return false;
            }


        }
   }
}