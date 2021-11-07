using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace EmailManagements
{
    public static class EmailManagement
    {
        public static string EmailID = string.Empty;
        public static SmtpClient GetSMTPDetails()
        {
            SmtpClient objSMTPClient = new SmtpClient();
            objSMTPClient.Host = "smtpout.secureserver.net";
            //objSMTPClient.Host = "relay-hosting.secureserver.net";
            objSMTPClient.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("noreply@emotionalbliss.co.uk", "No370456@emotionalbliss");
            objSMTPClient.UseDefaultCredentials = false;
            objSMTPClient.Credentials = NetworkCred;
            objSMTPClient.Port = 3535;
            //objSMTPClient.Port = 25;
            return objSMTPClient;
        }
        public static MailMessage GetEmailContent(String Subject, String Body, String ToEmail)
        {
            return new MailMessage("noreply@emotionalbliss.co.uk", ToEmail, Subject, Body) { IsBodyHtml = true };
        }
        private static String GetPayPalOrderContent(int CustomerOrderID, string paymentId, string token, string PayerID)
        {
            DataTable objDataTable = new DataTable();
            objDataTable = GetCustomerDetailsByOrderID(CustomerOrderID);
            EmailID = objDataTable.Rows[0]["email"].ToString();
            String HmtlContent = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/PaymentByPayPal.html")))
            {
                HmtlContent = reader.ReadToEnd();
            }
            HmtlContent = HmtlContent.Replace("{FullName}", objDataTable.Rows[0]["billName"].ToString());
            HmtlContent = HmtlContent.Replace("{PaymentID}", paymentId);
            HmtlContent = HmtlContent.Replace("{Token}", token);
            HmtlContent = HmtlContent.Replace("{PayerID}", PayerID);

            return HmtlContent;
        }
        private static String GetCreditCardOrderContent(int CustomerOrderID, string paymentId)
        {
            DataTable objDataTable = new DataTable();
            objDataTable = GetCustomerDetailsByOrderID(CustomerOrderID);
            EmailID = objDataTable.Rows[0]["email"].ToString();
            String HmtlContent = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/PaymentByCreditCard.html")))
            {
                HmtlContent = reader.ReadToEnd();
            }
            HmtlContent = HmtlContent.Replace("{FullName}", objDataTable.Rows[0]["billName"].ToString());
            HmtlContent = HmtlContent.Replace("{PaymentID}", paymentId);
            return HmtlContent;
        }
        public static void SendCreditCardEmail(int CustomerOrderID, string paymentId)
        {
            try
            {
                GetSMTPDetails().Send(GetEmailContent("Order Conformation Mail", GetCreditCardOrderContent(CustomerOrderID, paymentId), EmailID));
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
            }
            
        }
        public static void SendPayPalEmail(int CustomerOrderID,string paymentId,string token,string PayerID)
        {
            try
            {
                GetSMTPDetails().Send(GetEmailContent("Order Conformation Mail", GetPayPalOrderContent(CustomerOrderID, paymentId, token, PayerID), EmailID));
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
            }
          
            
        }
        public static DataTable GetCustomerDetailsByOrderID(int CustomerOrderID)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                if (CustomerOrderID > 0)
                {
                    SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand command = new SqlCommand("procShopCustomerByOrderIDSelect", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int)).Value = CustomerOrderID;
                    connection.Open();
                    objDataTable = new DataTable();
                    da = new SqlDataAdapter(command);
                    da.Fill(objDataTable);
                    da.Dispose();
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                return objDataTable;
            }
            catch (Exception)
            {
                return new DataTable();
            }


        }
    }
}
