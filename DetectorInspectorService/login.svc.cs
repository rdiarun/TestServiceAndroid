using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace DetectorService
{

    public class login : Ilogin
    {
        public UserInfo ValidateTechnician(string emailId, string password)
        {

            var objInfo = new UserInfo();                               //object of UserInfo Class
            var lisTDetail = new TechnicianDetail();                    //Object Of TechnicianDetail Class
            var param = new string[] { "EmailId", "Password" };         //Array Of Parameter Name
            var paramValue = new object[] { emailId, password };        //Array Of Parameter Value
            DataSet ds;
            using (ds = new DataSet())
            {
                ds = Common.GetDataSet("GetUserByEmailPassword", param, paramValue);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)                    //If There is any value in database according to 
                    {                                                   //user name and password then send successful to the  
                        //android application     
                        //1. technicianId
                        //2. company 
                        //3. telephone 
                        //4. mobile
                        //5. address
                        //6. suburb 
                        //7. postalcode 
                        //8. stateId 

                        objInfo.message = "successful";
                        objInfo.status = 1;
                        lisTDetail.technicianId = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        lisTDetail.company = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        lisTDetail.telephone = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        lisTDetail.mobile = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        lisTDetail.address = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        lisTDetail.suburb = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                        lisTDetail.postalcode = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                        lisTDetail.stateId = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                        objInfo.technician = lisTDetail;
                    }
                    else
                    {
                        objInfo.message = "invalid user";               //if there is no any value in database 
                        objInfo.status = 0;                             //According to the user name and password
                        //return failed to the user
                    }
                }
            }
            return objInfo;
        }

    }
}
