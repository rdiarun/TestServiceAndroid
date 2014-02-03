using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace DetectorService
{
    [ServiceContract]
    public interface ITechnicianSync
    {
        //Interface for Login Service
        //Created BY:Jagmohan Krishan
        //Created Date: 19 sep 2013
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "DedectionComplite", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        ResultSet DedectionComplite(RootObject syncData);
        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "DedectionComplite", RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        //UserInfo UpdateImages(ReportPhoto syncData);

    }


    [DataContract]
    public class ReportPhoto
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string dt { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string photo_guid { get; set; }
        [DataMember]
        public int q { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int angle { get; set; }
        [DataMember]
        public int x { get; set; }
        [DataMember]
        public int y { get; set; }
    }
     [DataContract]
    public class ReportItems
    {
         [DataMember]
         public string problemNote { get; set; }
         [DataMember]
         public int detectorType { get; set; }
         [DataMember]
         public string electricalNote { get; set; }
         [DataMember]
         public string expiryYear { get; set; }
         [DataMember]
         public string manufacturer { get; set; }
         [DataMember]
         public string newExpiryYear { get; set; }
         [DataMember]
         public bool notRequired { get; set; }
         [DataMember]
         public bool decibelTest { get; set; }
         [DataMember]
         public bool cleaned { get; set; }
         [DataMember]
         public bool batteryReplaced { get; set; }
         [DataMember]
         public bool stickedApplied { get; set; }
         [DataMember]
         public bool voltProblem { get; set; }              // if true then  electricalNote is =”asdhsd” else  electricalNote = “”
         [DataMember]
         public bool isCardLeft { get; set; }
         [DataMember]
         public bool hasSignature { get; set; }
    }
     [DataContract]
    public class ReportSection
    {
         [DataMember]
         public int rid { get; set; }                   //Report Id
         [DataMember]
         public List<ReportPhoto> reportPhotos { get; set; }
         [DataMember]
         public string n { get; set; }              // Location Name
         [DataMember]
         public ReportItems reportItems { get; set; }
         [DataMember]
         public bool is_deleted { get; set; }
         [DataMember]
         public int id { get; set; }
         [DataMember]
         public int dr { get; set; }
    }
     [DataContract]
    public class Report
    {
         [DataMember]
         public int id { get; set; }                    //Report UniqueId
         [DataMember]
         public string iid { get; set; }                //propertyId     
         [DataMember]
         public string n { get; set; }                       //Property Name 
         [DataMember]
         public List<ReportSection> reportSections { get; set; }
    }
     [DataContract]
    public class RootObject
    {
         [DataMember]
         public string bookingId { get; set; }
         [DataMember]
         public string technicianId { get; set; }
         [DataMember]
         public string latitute { get; set; }
         [DataMember]
         public string longitute { get; set; }
         [DataMember]
         public Report report { get; set; }                     //Property 
    }

   
}