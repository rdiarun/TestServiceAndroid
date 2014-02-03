using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DetectorInspector;

namespace DetectorService
{

    public class TechnicianSync : ITechnicianSync
    {
        public ResultSet DedectionComplite(RootObject syncData)
        {

           // var obShee = new DetectorInspector.Areas.Admin.Controllers.ImportController();



            var objInfo = new ResultSet();
            //var jsonSerial = new JavaScriptSerializer();
            var booking = syncData;
            string s = "1";
            if (booking != null)
            {
                var sqlcmd = new SqlCommand
                {
                    Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString)
                };
                sqlcmd.Connection.Open();
                var trans = sqlcmd.Connection.BeginTransaction();
                try
                {
                    s += " -2";
                    Int32 serviceSheetId = 0;
                    string[] param;
                    object[] paramValue;
                    List<ReportSection> rptSection = booking.report.reportSections;
                    if (rptSection != null)
                    {
                        s += " -3";
                        var electricalNotes = "";
                        var isElectricianRequired = false;
                        foreach (ReportSection rpt in rptSection)
                        {
                            if (rpt.reportItems.voltProblem)
                            {
                                if (Check(rpt.reportItems.electricalNote) != "")
                                {
                                    electricalNotes += rpt.n + " : " + rpt.reportItems.electricalNote + System.Environment.NewLine;
                                }
                            }
                            if (rpt.reportItems.voltProblem)
                            {
                                isElectricianRequired = true;
                            }
                        }
                        s += " -4";

                        s += " -BookingId " + booking.bookingId.ToString();
                        s += " -TechnicianId " + booking.technicianId.ToString();
                        s += " -Notes " + booking.report.serviceNote.ToString();
                        s += " -ElectricalNotes " + Check(electricalNotes).ToString();
                        s += " -IsElectricianRequired " + isElectricianRequired.ToString();
                        s += " -HasProblem " + (Check(booking.report.problemNote) == "" ? false : true).ToString();

                        s += " -ProblemNotes " + Check(booking.report.problemNote).ToString();
                        s += " -IsCardLeft " + booking.report.leftCard.ToString();
                        s += " -HasSignature " + booking.report.signature.ToString();
                        s += " -IsElectrical " + isElectricianRequired.ToString();
                        s += " -OldserviceSheetid " + booking.oldserviceSheetid.ToString();
                        s += " -PropertyInfoId " + Check(booking.report.iid).ToString();

                        param = new string[] { "BookingId", "TechnicianId", "Notes", "ElectricalNotes",
                                "IsElectricianRequired", "HasProblem", "ProblemNotes", "IsCardLeft", "HasSignature", 
                                "IsElectrical", "OldserviceSheetid","PropertyInfoId", "ServiceSheetID" };  //,
                        paramValue = new object[] { booking.bookingId, booking.technicianId, booking.report.serviceNote,
                            Check(electricalNotes), isElectricianRequired, (Check(booking.report.problemNote) == "" ? false : true), 
                            Check(booking.report.problemNote), booking.report.leftCard, booking.report.signature, 
                            isElectricianRequired, booking.oldserviceSheetid, Check(booking.report.iid), 0 };

                        s += " -4.5  " + serviceSheetId.ToString();

                        serviceSheetId = System.Convert.ToInt32(Common.ExecuteSpWithOutPut("p_SaveServiceSheet", param, paramValue, sqlcmd, trans).Parameters["ServiceSheetID"].Value);
                        s += " -5  " + serviceSheetId.ToString();
                        foreach (ReportSection rpt in rptSection)
                        {
                            //Save  service sheet items
                            param = new string[] { "ServiceSheetId", "TechnicianId", "PropertyInfoId", "Location", "DetectorTypeId", "Manufacturer", "ExpiryYear", "NewExpiryYear", "IsBatteryReplaced", "IsReplacedByElectrician", "IsRepositioned", "IsDecibelTested", "IsCleaned", "HasSticker", "IsOptional", "HasProblem", "ServiceSheetItemId" };
                            paramValue = new object[] { serviceSheetId, booking.technicianId, Check(booking.report.iid), rpt.n, rpt.reportItems.detectorType, rpt.reportItems.manufacturer, rpt.reportItems.expiryYear, rpt.reportItems.newExpiryYear, rpt.reportItems.batteryReplaced, false, false, rpt.reportItems.decibelTest, rpt.reportItems.cleaned, rpt.reportItems.stickedApplied, rpt.reportItems.notRequired, rpt.reportItems.voltProblem, 0 };
                            var serviceSheetItemId = System.Convert.ToInt32(Common.ExecuteSpWithOutPut("P_ServiceSheetItem", param, paramValue, sqlcmd, trans).Parameters["ServiceSheetItemId"].Value);
                            Int32 count = 1;
                            foreach (ReportPhoto photo in rpt.reportPhotos)
                            {
                                //Save Images
                                param = new string[] { "PropertyInfoId", "TechnicianId", "ImageName", "ImagePath", "Image_guid", "LocationName", "serviceSheetId", "serviceSheetItemId" };
                                paramValue = new object[] { Check(booking.report.iid), booking.technicianId, count, "", photo.photo_guid, rpt.n, serviceSheetId, serviceSheetItemId };
                                var code = Common.ExecuteSp("p_SavePhoto", param, paramValue, sqlcmd, trans);
                                count = count + 1;
                            }
                        }


                        //Save location
                        param = new string[] { "BookingId", "PropertyInfoId", "TechnicianId" };
                        paramValue = new object[] { booking.bookingId.ToString(), booking.report.iid, booking.technicianId };
                        Common.ExecuteSp("p_SaveServiceSheetStatus", param, paramValue, sqlcmd, trans);

                        //Save location
                        param = new string[] { "ServiceSheetId", "PropertyInfoId", "TechnicianId", "Longitute", "Latitute" };
                        paramValue = new object[] { serviceSheetId, booking.report.iid, booking.technicianId, Convert.ToDecimal(booking.lat == "" ? 0 : Convert.ToDecimal(booking.lat)), Convert.ToDecimal(booking.lng == "" ? 0 : Convert.ToDecimal(booking.lng)) };
                        Common.ExecuteSp("p_SaveTechnicianLocation", param, paramValue, sqlcmd, trans);

                    }
                    trans.Commit();
                    objInfo.message = "successful";
                    objInfo.status = 1;
                    objInfo.propertyId = booking.report.iid;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    objInfo.message = "failure";//+ e.Message + s ;
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
            return (prop ?? string.Empty);
        }

        public int CheckNo(string prop)
        {
            return (prop == null ? 0 : Convert.ToInt32(prop));

        }



        public ResultSet SaveLocation(int technicianId, string lat, string lng)
        {
            var objInfo = new ResultSet();
            var sqlcmd = new SqlCommand
            {
                Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnn"].ConnectionString)
            };
            sqlcmd.Connection.Open();
            var trans = sqlcmd.Connection.BeginTransaction();
            try
            {
                string[] param;
                object[] paramValue;
                param = new string[] {"ServiceSheetId", "PropertyInfoId", "TechnicianId", "Longitute", "Latitute"};
                paramValue = new object[]
                {
                    "", "", technicianId, Convert.ToDecimal(lat == "" ? 0 : Convert.ToDecimal(lat)),
                    Convert.ToDecimal(lng == "" ? 0 : Convert.ToDecimal(lng))
                };
                Common.ExecuteSp("p_SaveTechnicianLocation", param, paramValue, sqlcmd, trans);
                trans.Commit();
               objInfo.message = "Success";
                objInfo.status = 1;
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
            return objInfo;
        }


    }
}
