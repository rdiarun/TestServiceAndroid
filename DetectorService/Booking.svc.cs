using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace DetectorService
{
    public class Booking : IBooking
    {

        public BookingInfo BookingDetails(string technicianId)
        {
            var objBookingInfo = new BookingInfo();                         //object of BookingInfo Class
            var listAgencies = new AgencyDetail();                          //Obect of AgencyDetail Class
            var propertyManager = new PropertyManager();                    //Object Of PropertyManager Class
            var param = new string[] { "technicianId" };                    //Valriable which Takes Parameter Names     
            var paramValue = new object[] { technicianId };                 //Valriable which Takes Parameter Values    
            DataSet ds;
            using (ds = new DataSet())
            {
                ds = Common.GetDataSet("p_getBookingDetails", param, paramValue);  //ssp_getBookingDetails
                if (ds != null)
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    objBookingInfo.message = "successful";
                    objBookingInfo.status = 1;
                    objBookingInfo.technicianId = technicianId;

                    var srno = 1;
                    objBookingInfo.propertyInfo = new List<PropertyInfo>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objBookingInfo.propertyInfo.Add(new PropertyInfo
                        {
                            bookingId = dr.ItemArray[25].ToString(),
                            propertyId = dr.ItemArray[2].ToString(),
                            status = 1,
                            srno = srno,
                            agency = listAgencies,
                            propertyManager = propertyManager,
                            unitShopNumber = dr.ItemArray[3].ToString(),
                            streetNumber = dr.ItemArray[4].ToString(),
                            streetName = dr.ItemArray[5].ToString(),
                            suburb = dr.ItemArray[6].ToString(),
                            state = dr.ItemArray[7].ToString(),
                            postCode = dr.ItemArray[8].ToString(),
                            keyNumber = dr.ItemArray[9].ToString(),
                            notes = dr.ItemArray[10].ToString(),
                            hasLargeLadder = dr.ItemArray[11].ToString(),
                            hasSendNotification = dr.ItemArray[12].ToString(),
                            inspectionDate = dr.ItemArray[13].ToString(),
                            occupantName = dr.ItemArray[14].ToString(),
                            occupantEmail = dr.ItemArray[15].ToString(),
                            postalAddress = dr.ItemArray[16].ToString(),
                            postalSuburb = dr.ItemArray[17].ToString(),
                            postalState = dr.ItemArray[18].ToString(),
                            postalPostCode = dr.ItemArray[19].ToString(),
                            postalCountry = dr.ItemArray[20].ToString(),
                            keyTime = dr.ItemArray[26].ToString(),
                            contact = GetContactDetails((Int32)dr.ItemArray[2]),
                            previousHistory = GetHistoryDetails((Int32)dr.ItemArray[2]

                            )
                        });
                        srno += 1;
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        listAgencies.agencyId = ds.Tables[0].Rows[0].ItemArray[22].ToString();
                        listAgencies.agencyName = ds.Tables[0].Rows[0].ItemArray[23].ToString();
                        listAgencies.telephone = ds.Tables[0].Rows[0].ItemArray[24].ToString();
                        propertyManager.propertyManagerId = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        propertyManager.propertyManagerName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    }
                }
            }
            return objBookingInfo;
        }
        public List<Contact> GetContactDetails(Int32 PropertyInfoId)
        {
            var param = new string[] { "PropertyInfoId" };                    //Valriable which Takes Parameter Names     
            var paramValue = new object[] { PropertyInfoId };
            DataSet ds;
            var contactList = new List<Contact>();
            ds = Common.GetDataSet("p_GetContactList", param, paramValue);
            if (ds != null)
            {
                contactList.AddRange(from DataRow dr in ds.Tables[0].Rows
                    select new Contact
                    {
                        contactNumber = dr.ItemArray[0].ToString(), contactType = dr.ItemArray[1].ToString()
                    });
            }
            return contactList;
        }
        public List<previousHistory> GetHistoryDetails(Int32 PropertyInfoId)
        {
            var param = new string[] { "PropertyInfoId" };                    //Valriable which Takes Parameter Names     
            var paramValue = new object[] { PropertyInfoId };
            DataSet ds;
            var previousHistory = new List<previousHistory>();
            ds = Common.GetDataSet("p_GetPreviousInspectionDetails", param, paramValue);
            if (ds == null) return previousHistory;
            {
                previousHistory.AddRange(from DataRow dr in ds.Tables[0].Rows
                select new previousHistory
                {
                    locationName = dr.ItemArray[0].ToString(), detectorType = dr.ItemArray[1].ToString(), manufacturer = dr.ItemArray[2].ToString(), expiryYear = dr.ItemArray[3].ToString(), newExpiryYear = dr.ItemArray[4].ToString(), serviceSheetId= Convert.ToInt32(dr.ItemArray[5].ToString())
                });
            }
            return previousHistory;
        }
        public bool UnAllowcateBooking(Int32 BookingId_, string Key_, string Notes_, Int32 propertyId)
        {
            var sqlcmd = new SqlCommand
            {
                Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString)
            };
            sqlcmd.Connection.Open();
            var trans = sqlcmd.Connection.BeginTransaction();
            try
            {
                var param = new string[] { "BookingId", "Key", "Notes" };                    //Valriable which Takes Parameter Names     
                var paramValue = new object[] { BookingId_, Key_, Notes_ };
                Common.ExecuteSp("p_UnAllowcateBooking", param, paramValue, sqlcmd, trans);
                trans.Commit();
                return true;
            }
            catch (Exception ex) { trans.Rollback(); return false; }
            finally { sqlcmd.Connection.Close(); }
        }
        public BookingInfo LastBookingDetails(string technicianId)
        {
            var objBookingInfo = new BookingInfo();                         //object of BookingInfo Class
            var listAgencies = new AgencyDetail();                          //Obect of AgencyDetail Class
            var propertyManager = new PropertyManager();                    //Object Of PropertyManager Class
            var param = new string[] { "technicianId" };                    //Valriable which Takes Parameter Names     
            var paramValue = new object[] { technicianId };                 //Valriable which Takes Parameter Values    
            DataSet ds;
            using (ds = new DataSet())
            {
                ds = Common.GetDataSet("p_GetLastBookingDetails", param, paramValue);  //ssp_getBookingDetails
                if (ds != null)
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    objBookingInfo.message = "successful";
                    objBookingInfo.status = 1;
                    objBookingInfo.technicianId = technicianId;

                    var srno = 1;
                    objBookingInfo.propertyInfo = new List<PropertyInfo>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objBookingInfo.propertyInfo.Add(new PropertyInfo
                        {
                            bookingId = dr.ItemArray[25].ToString(),
                            propertyId = dr.ItemArray[2].ToString(),
                            status = 1,
                            srno = srno,
                            agency = listAgencies,
                            propertyManager = propertyManager,
                            unitShopNumber = dr.ItemArray[3].ToString(),
                            streetNumber = dr.ItemArray[4].ToString(),
                            streetName = dr.ItemArray[5].ToString(),
                            suburb = dr.ItemArray[6].ToString(),
                            state = dr.ItemArray[7].ToString(),
                            postCode = dr.ItemArray[8].ToString(),
                            keyNumber = dr.ItemArray[9].ToString(),
                            notes = dr.ItemArray[10].ToString(),
                            hasLargeLadder = dr.ItemArray[11].ToString(),
                            hasSendNotification = dr.ItemArray[12].ToString(),
                            inspectionDate = dr.ItemArray[13].ToString(),
                            occupantName = dr.ItemArray[14].ToString(),
                            occupantEmail = dr.ItemArray[15].ToString(),
                            postalAddress = dr.ItemArray[16].ToString(),
                            postalSuburb = dr.ItemArray[17].ToString(),
                            postalState = dr.ItemArray[18].ToString(),
                            postalPostCode = dr.ItemArray[19].ToString(),
                            postalCountry = dr.ItemArray[20].ToString(),
                            keyTime = dr.ItemArray[26].ToString(),
                            contact = GetContactDetails((Int32)dr.ItemArray[2]),
                            previousHistory = GetHistoryDetails((Int32)dr.ItemArray[2]

                            )
                        });
                        srno += 1;
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        listAgencies.agencyId = ds.Tables[0].Rows[0].ItemArray[22].ToString();
                        listAgencies.agencyName = ds.Tables[0].Rows[0].ItemArray[23].ToString();
                        listAgencies.telephone = ds.Tables[0].Rows[0].ItemArray[24].ToString();
                        propertyManager.propertyManagerId = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        propertyManager.propertyManagerName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    }
                }
            }
            return objBookingInfo;
        }
    }
}
