using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
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
                var obj = new DetectorService.EmailHelper();
                string[] att = new string[0];
                var param = new string[] { "Email", "NewPassword" };
                var paramValue = new string[] { emailId, newPassword };
                var UserEmailId = Common.GetDataSet("p_ChangePassword", param, paramValue);
                var sent = DetectorService.EmailHelper.SendEmail("jagmohankasana7@gmail.com", "Jagmohan kasana", emailId, emailId, "New Password", "Your New Password is " + newPassword, true, att, "gmail.com", "jagmohankasana7@gmail.com", "ommomdad");
                if (sent == true)  // if sent == true that means Email Is Sent on Technician Email Id
                {
                    objInfo.message = "Mail Sent successfully";
                    objInfo.status = 1;
                    return objInfo;
                }
                else           // if Email Is Not Sent On Technician Email Id
                {
                    objInfo.message = "Mail Sending Failure";
                    objInfo.status = 0;
                    return objInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { sqlcmd.Connection.Close(); }
        }
    }
}
