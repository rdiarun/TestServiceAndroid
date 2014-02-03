using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace DetectorService
{

    public class TechnicianSync : ITechnicianSync
    {
        public ResultSet DedectionComplite(RootObject syncData)
        {
            var objInfo = new ResultSet();
            var jsonSerial = new JavaScriptSerializer();
            var booking = syncData;
            if (booking != null)
            {
                var sqlcmd = new SqlCommand();
                sqlcmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString);
                sqlcmd.Connection.Open();
                SqlTransaction trans = sqlcmd.Connection.BeginTransaction();
                try
                {
                    Int32 serviceSheetId = 0;
                    var param = new string[20];
                    var paramValue = new object[20];
                    List<ReportSection> rptSection = new List<ReportSection>();
                    List<ReportPhoto> rptPhoto;
                    rptSection = booking.report.reportSections;

                    if (rptSection != null)
                    {
                        foreach (ReportSection rpt in rptSection)
                        {
                            // save service Sheet 
                            param = new string[] { "BookingId", "TechnicianId", "Notes", "ElectricalNotes", "IsElectricianRequired", "HasProblem", "ProblemNotes", "IsCardLeft", "HasSignature", "IsElectrical", "ServiceSheetID" };  //,
                            paramValue = new object[] { booking.bookingId, booking.technicianId, "", Check(rpt.reportItems.electricalNote), (rpt.reportItems.voltProblem = true ? true : false), (Check(rpt.reportItems.problemNote) == "" ? false : true), Check(rpt.reportItems.problemNote), rpt.reportItems.isCardLeft, rpt.reportItems.hasSignature, false, 0 };
                            serviceSheetId = System.Convert.ToInt32(Common.ExecuteSpWithOutPut("p_SaveServiceSheet", param, paramValue, sqlcmd, trans).Parameters["ServiceSheetID"].Value);

                            //Save  service sheet items
                            param = new string[] { "ServiceSheetId", "Location", "DetectorTypeId", "Manufacturer", "ExpiryYear", "NewExpiryYear", "IsBatteryReplaced", "IsReplacedByElectrician", "IsRepositioned", "IsDecibelTested", "IsCleaned", "HasSticker", "IsOptional", "HasProblem", "ServiceSheetItemId" };
                            paramValue = new object[] { serviceSheetId, rpt.n, rpt.reportItems.detectorType, rpt.reportItems.manufacturer, rpt.reportItems.expiryYear, rpt.reportItems.newExpiryYear, rpt.reportItems.batteryReplaced, false, false, rpt.reportItems.decibelTest, rpt.reportItems.cleaned, rpt.reportItems.stickedApplied, false, false, 0 };
                            var serviceSheetItemId = System.Convert.ToInt32(Common.ExecuteSpWithOutPut("P_ServiceSheetItem", param, paramValue, sqlcmd, trans).Parameters["ServiceSheetItemId"].Value);
                            rptPhoto = new List<ReportPhoto>();
                            foreach (ReportPhoto photo in rptPhoto)
                            {
                                //Save Images
                                param = new string[] { "ServiceSheetId", "PropertyInfoId", "ServiceSheetItemId", "TechnicianId", "ImageName", "Extension", "ImageBytes", "ImagePath" };
                                paramValue = new object[] { serviceSheetId, Check(booking.report.iid), serviceSheetItemId, booking.technicianId, Check(photo.name), "jpg", "{0,0,0}", "photoPath" };
                                var code = Common.ExecuteSp("P_SavPhoto", param, paramValue, sqlcmd, trans);
                            }
                        }
                        //Save location
                        param = new string[] { "ServiceSheetId", "PropertyInfoId", "TechnicianId", "Longitute", "Latitute" };
                        paramValue = new object[] { serviceSheetId, booking.report.iid, booking.technicianId, Convert.ToDecimal(CheckNo(booking.latitute)), Convert.ToDecimal(CheckNo(booking.longitute)) };
                        Common.ExecuteSp("p_SaveTechnicianLocation", param, paramValue, sqlcmd, trans);
                        trans.Commit();
                    }
                    objInfo.message = "successful";
                    objInfo.status = 1;
                    objInfo.propertyId = booking.report.iid;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    objInfo.message = "failure";
                    objInfo.status = 0;
                    objInfo.propertyId = "";
                }
                finally
                {
                    if (sqlcmd.Connection.State == ConnectionState.Open)
                        sqlcmd.Connection.Close();
                }

            }
            return objInfo;
        }

        public string Check(string prop)
            {
                return (prop == null ? string.Empty : prop);                            

            }

        public int CheckNo(string prop)
        {
            return (prop == null ? 0 : Convert.ToInt32(prop));

        }
        
        
        //public UserInfo UpdateImages(ReportPhoto syncData)
        //{



        //    var objInfo = new UserInfo();
        //    objInfo.message = "successful";
        //    objInfo.status = 1;
        //    return objInfo;
        //}
    }
}
