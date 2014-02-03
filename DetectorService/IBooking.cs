using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace DetectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBooking" in both code and config file together.
    [ServiceContract]
    public interface IBooking
    {
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BookingDetails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        BookingInfo BookingDetails(string technicianId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "LastBookingDetails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        BookingInfo LastBookingDetails(string technicianId);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UnAllowcateBooking", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Boolean UnAllowcateBooking(Int32 BookingId_, string Key_, string Notes_ ,Int32 propertyId);


    }
    [DataContract]
    public class BookingInfo
    {
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string technicianId { get; set; }
        [DataMember]
        public List<PropertyInfo> propertyInfo;

    }
    [DataContract]
    public class PropertyInfo
    {
        [DataMember]
        public string bookingId { get; set; }
        [DataMember]
        public string propertyId { get; set; }
        [DataMember]
        public Int32 status { get; set; }
        [DataMember]
        public AgencyDetail agency;
        [DataMember]
        public PropertyManager propertyManager;
        [DataMember]
        public string unitShopNumber { get; set; }
        [DataMember]
        public string unit { get; set; }
        [DataMember]
        public string keyTime { get; set; }
        [DataMember]
        public string streetNumber { get; set; }
        [DataMember]
        public string streetName { get; set; }
        [DataMember]
        public string suburb { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string postCode { get; set; }
        [DataMember]
        public string keyNumber { get; set; }
        [DataMember]
        public string notes { get; set; }
        [DataMember]
        public string hasLargeLadder { get; set; }
        [DataMember]
        public string hasSendNotification { get; set; }
        [DataMember]
        public string inspectionDate { get; set; }
        [DataMember]
        public string occupantName { get; set; }
        [DataMember]
        public string occupantEmail { get; set; }
        [DataMember]
        public string postalAddress { get; set; }
        [DataMember]
        public string postalSuburb { get; set; }
        [DataMember]
        public string postalState { get; set; }
        [DataMember]
        public string postalPostCode { get; set; }
        [DataMember]
        public string postalCountry { get; set; }
        [DataMember]
        public List<Contact> contact { get; set; }
        [DataMember]
        public List<previousHistory> previousHistory { get; set; }
        [DataMember]
        public Int32 srno { get; set; }
    }

    [DataContract]
    public class AgencyDetail
    {
        [DataMember]
        public string agencyId { get; set; }
        [DataMember]
        public string agencyName { get; set; }
        [DataMember]
        public string telephone { get; set; }
    }
    
    [DataContract]
    public class PropertyManager
    {
        [DataMember]
        public string propertyManagerId { get; set; }
        [DataMember]
        public string propertyManagerName { get; set; }
    }
    
    [DataContract]
    public class Contact
    {
        [DataMember]
        public string contactNumber { get; set; }
        [DataMember]
        public string contactType { get; set; }

    }
    [DataContract]
    public class previousHistory
    {
        [DataMember]
        public string locationName { get; set; }
        [DataMember]
        public string detectorType { get; set; }
        [DataMember]
        public string manufacturer { get; set; }
        [DataMember]
        public string expiryYear { get; set; }
        [DataMember]
        public string newExpiryYear { get; set; }
        [DataMember]
        public Int32 serviceSheetId { get; set; }
    }

}





 