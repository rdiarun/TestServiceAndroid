using System;
using System.Data.SqlClient;
using System.Configuration;

namespace DetectorService
{

    public class ForgotPassword : IForgotPassword
    {


        public ForgetPwdInfo ForgetPassword(string emailId)
        {
            var sqlcmd = new SqlCommand();
            sqlcmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
            sqlcmd.Connection.Open();
            try
            {
                var newPassword = DetectorService.Common.GeneratePassword();
                var objInfo = new ForgetPwdInfo();
                var param = new string[] { "Email", "NewPassword" };
                var paramValue = new string[] { emailId, newPassword };
                var userEmailId = Common.GetDataSet("p_ChangePassword", param, paramValue);
                var sent = EmailHelper.SendEmail(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["UserName"].ToString(), emailId, emailId, "New password info", "Your new Password is " + newPassword, true, null, ConfigurationManager.AppSettings["HostName"].ToString(), ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                //var sent = DetectorService.EmailHelper.SendEmail("webinfo@smartdatainc.net", "Jagmohan kasana", emailId, emailId, "New Password", "Your New Password is " + newPassword, true, null, "gmail.com", "webinfo@smartdatainc.net", "Sdm#Chd$W0");
                if (sent == true)  // if sent == true that means Email Is Sent on Technician Email Id
                {
                    objInfo.message = "Mail Sent successfully";
                    objInfo.status = 1;
                    objInfo.newPassword = newPassword;
                    return objInfo;
                }
                else           // if Email Is Not Sent On Technician Email Id
                {
                    objInfo.message = "Mail Sending Failure";
                    objInfo.status = 0;
                    objInfo.newPassword = "";
                    return objInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { sqlcmd.Connection.Close(); }
        }



        public ForgetPwdInfo SendNotification(string toEmail, string toName, string subject, string emailBody)
        {
            var objInfo = new ForgetPwdInfo();
            try
            {
                var sent = EmailHelper.SendEmail(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["UserName"].ToString(), toEmail, toName, subject, emailBody, true, null, ConfigurationManager.AppSettings["HostName"].ToString(), ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                if (sent == true)  // if sent == true that means Email Is Sent on Technician Email Id
                {
                    objInfo.message = "Mail Sent successfully";
                    objInfo.status = 1;
                }
                else           // if Email Is Not Sent On Technician Email Id
                {
                    objInfo.message = "Mail Sending Failure";
                    objInfo.status = 0;
                }

            }
            catch (Exception ex)
            {
                objInfo.message = "Mail Sending Failure. Error : " + ex.Message;
                objInfo.status = 0;
            }
            return objInfo;
        }
    }
}
