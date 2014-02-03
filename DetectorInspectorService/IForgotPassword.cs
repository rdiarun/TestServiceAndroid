using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DetectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IForgotPassword" in both code and config file together.
    [ServiceContract]
    public interface IForgotPassword
    {
       
        //InterFace for ForgetPassword
        //Made By:Deeksha
        //Created On: 08302013
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ForgetPassword", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ForgetPwdInfo ForgetPassword(string emailId);
    }

    [DataContract]
    public class ForgetPwdInfo
    {
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public int status { get; set; }
    }
}