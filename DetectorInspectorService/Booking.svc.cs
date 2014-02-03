using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            var contact = new List<Contact>();
            DataSet ds;
            using (ds = new DataSet())
            {
                ds = Common.GetDataSet("getBookingDetailsByTechnicianId", param, paramValue);  //ssp_getBookingDetails
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objBookingInfo.message = "successful";
                        objBookingInfo.status = 1;
                        listAgencies.agencyId = ds.Tables[0].Rows[0].ItemArray[22].ToString();
                        listAgencies.agencyName = ds.Tables[0].Rows[0].ItemArray[23].ToString();
                        listAgencies.telephone = ds.Tables[0].Rows[0].ItemArray[24].ToString();
                        objBookingInfo.technicianId = ds.Tables[0].Rows[0].ItemArray[21].ToString();
                        propertyManager.propertyManagerId = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        propertyManager.propertyManagerName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        objBookingInfo.propertyInfo = new List<PropertyInfo>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objBookingInfo.propertyInfo.Add(new PropertyInfo
                            {
                                bookingId = dr.ItemArray[25].ToString(),
                                propertyId = dr.ItemArray[2].ToString(),
                                status = 1,
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
                                contact = contact

                            });
                        }
                    }
                }
            }
            return objBookingInfo;
        }

    }


}
