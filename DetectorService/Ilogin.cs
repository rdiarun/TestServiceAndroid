using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DetectorService
{
    [ServiceContract]
    public interface Ilogin
    {
        
        //Interface for Login Service
        //Created BY:Deeksha
        //Created Date: 08/30/2013
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidateTechnician", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        UserInfo ValidateTechnician(string emailId, string password);
    }
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public TechnicianDetail technician;

    }

    [DataContract]
    public class ResultSet
    {
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string propertyId { get; set; }
    }
    [DataContract]
    public class TechnicianDetail
    {
        [DataMember]
        public string technicianId { get; set; }
        [DataMember]
        public string technicianName { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string telephone { get; set; }
        [DataMember]
        public string mobile { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string suburb { get; set; }
        [DataMember]
        public string postalcode { get; set; }
        [DataMember]
        public string stateId { get; set; }
        [DataMember]
        public Boolean turnOffExpiryYear { get; set; }
        [DataMember]
        public Boolean turnOffAlarmType { get; set; }
        [DataMember]
        public Boolean turnOffNoOfAlarms { get; set; }
    }
}