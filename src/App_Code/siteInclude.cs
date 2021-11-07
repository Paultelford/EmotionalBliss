using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;

/// <summary>
/// Summary description for siteInclude
/// </summary>

public class siteInclude
    {
        public enum _emailType { emailText, emailHtml };
        public siteInclude(){}
                
        public static void addError(string page, string msg)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procErrorInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@page", SqlDbType.VarChar, 50));
            oCmd.Parameters.Add(new SqlParameter("@msg", SqlDbType.VarChar, 4000));
            oCmd.Parameters["@Page"].Value = page;
            oCmd.Parameters["@msg"].Value = msg;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }
        public static void debug(string msg)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procDebugInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@msg", SqlDbType.VarChar, 2000));
            oCmd.Parameters["@msg"].Value = msg;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }

        public static void addToComponentHistory(int batchid, int qtyQAdded, int qtyQRemoved, int qtyStockAdded, int qtyStockRemoved, int qtyScrappedAdded, int qtyScrappedRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int compOrderID, int componentID, string description, string username, bool quarantine)
        {
            try
            {
                addToComponentHistory(batchid, qtyQAdded, qtyQRemoved, qtyStockAdded, qtyStockRemoved, qtyScrappedAdded, qtyScrappedRemoved, qtyFailedAdded, qtyFailedRemoved, action, compOrderID, componentID, description, username, quarantine, 0, false, 0, 0);
            }
            catch (Exception e)
            {
                throw (e);
            }            
        }
        public static void addToComponentHistory(int batchid, int qtyQAdded, int qtyQRemoved, int qtyStockAdded, int qtyStockRemoved, int qtyScrappedAdded, int qtyScrappedRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int compOrderID, int componentID, string description, string username, bool quarantine, int productAssemblyID)
        {
            try
            {
                addToComponentHistory(batchid, qtyQAdded, qtyQRemoved, qtyStockAdded, qtyStockRemoved, qtyScrappedAdded, qtyScrappedRemoved, qtyFailedAdded, qtyFailedRemoved, action, compOrderID, componentID, description, username, quarantine, productAssemblyID, false, 0, 0);
            }
            catch (Exception e)
            {
                throw (e);
            }            
        }
        public static void addToComponentHistory(int batchid, int qtyQAdded, int qtyQRemoved, int qtyStockAdded, int qtyStockRemoved, int qtyScrappedAdded, int qtyScrappedRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int compOrderID, int componentID, string description, string username, bool quarantine, int productAssemblyID, bool processed)
        {
            try
            {
                addToComponentHistory(batchid, qtyQAdded, qtyQRemoved, qtyStockAdded, qtyStockRemoved, qtyScrappedAdded, qtyScrappedRemoved, qtyFailedAdded, qtyFailedRemoved, action, compOrderID, componentID, description, username, quarantine, productAssemblyID, processed, 0, 0);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public static void addToComponentHistory(int batchid, int qtyQAdded, int qtyQRemoved, int qtyStockAdded, int qtyStockRemoved, int qtyScrappedAdded, int qtyScrappedRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int compOrderID, int componentID, string description, string username, bool quarantine, int productAssemblyID, bool processed, int warehouseAssemblyID)
        {
            try
            {
                addToComponentHistory(batchid, qtyQAdded, qtyQRemoved, qtyStockAdded, qtyStockRemoved, qtyScrappedAdded, qtyScrappedRemoved, qtyFailedAdded, qtyFailedRemoved, action, compOrderID, componentID, description, username, quarantine, productAssemblyID, processed, warehouseAssemblyID, 0);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        public static void addToComponentHistory(int batchid, int qtyQAdded, int qtyQRemoved, int qtyStockAdded, int qtyStockRemoved, int qtyScrappedAdded, int qtyScrappedRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int compOrderID, int componentID, string description, string username, bool quarantine, int productAssemblyID, bool processed, int warehouseAssemblyID, int orderID)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procComponentHistoryInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@batchid", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyQAdd", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyQRem", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyStockAdd", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyStockRem", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyScrappedAdd", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyScrappedRem", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyFailedAdd", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyFailedRem", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@actionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@compOrderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@componentID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar, 2000));
            oCmd.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar, 20));
            oCmd.Parameters.Add(new SqlParameter("@quarantine", SqlDbType.Bit));
            oCmd.Parameters.Add(new SqlParameter("@productAssemblyID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@processed", SqlDbType.Bit));
            oCmd.Parameters.Add(new SqlParameter("@warehouseAssemblyID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters["@batchid"].Value = batchid;
            oCmd.Parameters["@qtyQAdd"].Value = qtyQAdded;
            oCmd.Parameters["@qtyQRem"].Value = qtyQRemoved;
            oCmd.Parameters["@qtyStockAdd"].Value = qtyStockAdded;
            oCmd.Parameters["@qtyStockRem"].Value = qtyStockRemoved;
            oCmd.Parameters["@qtyScrappedAdd"].Value = qtyScrappedAdded;
            oCmd.Parameters["@qtyScrappedRem"].Value = qtyScrappedRemoved;
            oCmd.Parameters["@qtyFailedAdd"].Value = qtyFailedAdded;
            oCmd.Parameters["@qtyFailedRem"].Value = qtyFailedRemoved;
            oCmd.Parameters["@actionID"].Value = action;
            oCmd.Parameters["@compOrderID"].Value = compOrderID;
            oCmd.Parameters["@componentID"].Value = componentID;
            oCmd.Parameters["@description"].Value = description;
            oCmd.Parameters["@userName"].Value = username;
            oCmd.Parameters["@quarantine"].Value = quarantine;
            oCmd.Parameters["@productAssemblyID"].Value = productAssemblyID;
            oCmd.Parameters["@processed"].Value = processed;
            oCmd.Parameters["@warehouseAssemblyID"].Value = warehouseAssemblyID;
            oCmd.Parameters["@orderID"].Value = orderID;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }

        public static void addToProductHistory(int productID, int qtyAdded, int qtyRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int productionBatchID, string description, string username, bool quarantine, int warehouseAssemblyID, int qtyProductionAdded, int qtyProductionRemoved)
        {
            try
            {
                addToProductHistory(productID, qtyAdded, qtyRemoved, qtyFailedAdded, qtyFailedRemoved, action, productionBatchID, description, username, quarantine, warehouseAssemblyID, qtyProductionAdded, qtyProductionRemoved, 0);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        public static void addToProductHistory(int productID, int qtyAdded, int qtyRemoved, int qtyFailedAdded, int qtyFailedRemoved, int action, int productionBatchID, string description, string username, bool quarantine, int warehouseAssemblyID, int qtyProductionAdded, int qtyProductionRemoved,int orderID)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procProductHistoryInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@productID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyAdded", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyRemoved", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyFailedAdded", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyFailedRemoved", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@action", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@productionBatchID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar, 1000));
            oCmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 25));
            oCmd.Parameters.Add(new SqlParameter("@quarantine", SqlDbType.Bit));
            oCmd.Parameters.Add(new SqlParameter("@warehouseAssemblyID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyProductionAdded", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyProductionRemoved", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters["@productID"].Value = productID;
            oCmd.Parameters["@qtyAdded"].Value = qtyAdded;
            oCmd.Parameters["@qtyRemoved"].Value = qtyRemoved;
            oCmd.Parameters["@qtyFailedAdded"].Value = qtyFailedAdded;
            oCmd.Parameters["@qtyFailedRemoved"].Value = qtyFailedRemoved;
            oCmd.Parameters["@action"].Value = action;
            oCmd.Parameters["@productionBatchID"].Value = productionBatchID;
            oCmd.Parameters["@description"].Value = description;
            oCmd.Parameters["@username"].Value = username;
            oCmd.Parameters["@quarantine"].Value = quarantine;
            oCmd.Parameters["@warehouseAssemblyID"].Value = warehouseAssemblyID;
            oCmd.Parameters["@qtyProductionAdded"].Value = qtyProductionAdded;
            oCmd.Parameters["@qtyProductionRemoved"].Value = qtyProductionRemoved;
            oCmd.Parameters["@orderID"].Value = orderID;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }

        public static void addToWarehouseHistory(int warehouseProductID, int qtyAdded, int qtyRemoved, int action, string description, string username, int orderID, int qtyProductionAdded, int qtyProductionRemoved, int warehouseBatchID)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procWarehouseHistoryDescriptionInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            int descriptionID = 0;
            if (description != "")
            {
                //Add description and retrieve its ID (If description is passed)
                oCmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar, 1000));
                oCmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                oCmd.Parameters["@description"].Value = description;
                oCmd.Parameters["@id"].Direction = ParameterDirection.Output;
                try
                {
                    if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                    oCmd.ExecuteNonQuery();
                    descriptionID = (int)oCmd.Parameters["@id"].Value;
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    oCmd.Dispose();
                    oConn.Dispose();
                }
            }

            oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            oCmd = new SqlCommand("procWarehouseHistoryInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@warehouseProductID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyAdded", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyRemoved", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@action", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@descriptionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 25));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyProductionAdded", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyProductionRemoved", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@batchID", SqlDbType.Int));
            oCmd.Parameters["@warehouseProductID"].Value = warehouseProductID;
            oCmd.Parameters["@qtyAdded"].Value = qtyAdded;
            oCmd.Parameters["@qtyRemoved"].Value = qtyRemoved;
            oCmd.Parameters["@action"].Value = action;
            oCmd.Parameters["@descriptionID"].Value = descriptionID;
            oCmd.Parameters["@username"].Value = username;
            oCmd.Parameters["@orderID"].Value = orderID;
            oCmd.Parameters["@qtyProductionAdded"].Value = qtyProductionAdded;
            oCmd.Parameters["@qtyProductionRemoved"].Value = qtyProductionRemoved;
            oCmd.Parameters["@batchID"].Value = warehouseBatchID;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }
        public static void affAddToStatement(int affID, decimal credit, decimal debit, int orderID, int extOrderID, int actionID)
        {
            // Adds order detilas to an Affiliate or Distributors statement
            /* Used on:
             * shop/payment.aspx.vb
             * affiliates/payment.aspx.vb
             * affiliates/orders.aspx.vb
             */
            int v = affAddToStatement(affID, credit, debit, orderID, extOrderID, actionID, "", "", "",DateTime.Now);
        }
        public static int affAddToStatement(int affID, decimal credit, decimal debit, int orderID, int extOrderID, int actionID, string prefix,string cheque, string reason, DateTime dt)
        {
            // Adds order detilas to an Affiliate or Distributors statement
            /* Used on:
             * maintenance/statementPayment.aspx.vb
             */
            int pk = 0;
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procAffiliateStatementInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@affID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@statementCredit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@statementDebit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@extOrderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@actionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@linkedPrefix", SqlDbType.VarChar,5));
            oCmd.Parameters.Add(new SqlParameter("@cheque", SqlDbType.VarChar, 30));
            oCmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.VarChar, 2000));
            oCmd.Parameters.Add(new SqlParameter("@transDate", SqlDbType.DateTime, 2000));
            oCmd.Parameters.Add(new SqlParameter("@pk", SqlDbType.Int));

            oCmd.Parameters["@affID"].Value = affID;
            oCmd.Parameters["@statementCredit"].Value = credit;
            oCmd.Parameters["@statementDebit"].Value = debit;
            oCmd.Parameters["@orderID"].Value = orderID;
            oCmd.Parameters["@extOrderID"].Value = extOrderID;
            oCmd.Parameters["@actionID"].Value = actionID;
            oCmd.Parameters["@linkedPrefix"].Value = prefix;
            oCmd.Parameters["@cheque"].Value = cheque;
            oCmd.Parameters["@reason"].Value = reason;
            oCmd.Parameters["@transDate"].Value = dt;
            oCmd.Parameters["@pk"].Direction = ParameterDirection.Output;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                pk = (int)oCmd.Parameters["@pk"].Value;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
            return pk;
        }
        public static int affAddToStatementDistBuyingCurrency(int affID, decimal credit, decimal debit, int orderID, int extOrderID, int actionID, string prefix, string cheque, string reason, DateTime dt)
        {
            // Adds order detilas to an Affiliate or Distributors statement
            /* Used on:
             * maintenance/statementPayment.aspx.vb
             * maintenance/scan.aspx.vb
             */
            int pk = 0;
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procAffiliateStatementInsert2", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@affID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@statementCredit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@statementDebit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@extOrderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@actionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5));
            oCmd.Parameters.Add(new SqlParameter("@cheque", SqlDbType.VarChar, 30));
            oCmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.VarChar, 2000));
            oCmd.Parameters.Add(new SqlParameter("@transDate", SqlDbType.DateTime, 2000));
            oCmd.Parameters.Add(new SqlParameter("@pk", SqlDbType.Int));

            oCmd.Parameters["@affID"].Value = affID;
            oCmd.Parameters["@statementCredit"].Value = credit;
            oCmd.Parameters["@statementDebit"].Value = debit;
            oCmd.Parameters["@orderID"].Value = orderID;
            oCmd.Parameters["@extOrderID"].Value = extOrderID;
            oCmd.Parameters["@actionID"].Value = actionID;
            oCmd.Parameters["@linkedPrefix"].Value = prefix;
            oCmd.Parameters["@cheque"].Value = cheque;
            oCmd.Parameters["@reason"].Value = reason;
            oCmd.Parameters["@transDate"].Value = dt;
            oCmd.Parameters["@pk"].Direction = ParameterDirection.Output;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                pk = (int)oCmd.Parameters["@pk"].Value;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
            return pk;
        }
        public static int affAddToStatementDistBuyingWithCurrency(int affID, decimal credit, decimal debit, int orderID, int extOrderID, int actionID, string prefix, string cheque, string reason, string currency, DateTime dt)
        {
            // Adds order detilas to an Affiliate or Distributors statement
            /* Used on:
             * maintenance/statementPayment.aspx.vb
             */
            int pk = 0;
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procAffiliateStatementInsert3", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@affID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@statementCredit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@statementDebit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@extOrderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@actionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5));
            oCmd.Parameters.Add(new SqlParameter("@cheque", SqlDbType.VarChar, 30));
            oCmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.VarChar, 2000));
            oCmd.Parameters.Add(new SqlParameter("@transDate", SqlDbType.DateTime, 2000));
            oCmd.Parameters.Add(new SqlParameter("@currency", SqlDbType.VarChar, 3));
            oCmd.Parameters.Add(new SqlParameter("@pk", SqlDbType.Int));

            oCmd.Parameters["@affID"].Value = affID;
            oCmd.Parameters["@statementCredit"].Value = credit;
            oCmd.Parameters["@statementDebit"].Value = debit;
            oCmd.Parameters["@orderID"].Value = orderID;
            oCmd.Parameters["@extOrderID"].Value = extOrderID;
            oCmd.Parameters["@actionID"].Value = actionID;
            oCmd.Parameters["@linkedPrefix"].Value = prefix;
            oCmd.Parameters["@cheque"].Value = cheque;
            oCmd.Parameters["@reason"].Value = reason;
            oCmd.Parameters["@transDate"].Value = dt;
            oCmd.Parameters["@currency"].Value = currency;
            oCmd.Parameters["@pk"].Direction = ParameterDirection.Output;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                pk = (int)oCmd.Parameters["@pk"].Value;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
            return pk;
        }
        public static void affStockUpdate(int affProductBuyingID, int affID, int orderID, int extOrderID, int qtyAdded, int qtyRemoved, int actionID, bool addReason, string reason)
        {
            // Adjusts Distributors stock
            /* Used on:
             * affiliates/scan.aspx.vb
             */
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procAffiliateEBDistributorStockInsert2", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@affProductBuyingID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@affID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyAdd", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@qtyRemove", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@actionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.VarChar));
            oCmd.Parameters.Add(new SqlParameter("@addReason", SqlDbType.Bit));
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@extOrderID", SqlDbType.Int));
            oCmd.Parameters["@affProductBuyingID"].Value = affProductBuyingID;
            oCmd.Parameters["@affID"].Value = affID;
            oCmd.Parameters["@qtyAdd"].Value = qtyAdded;
            oCmd.Parameters["@qtyRemove"].Value = qtyRemoved;
            oCmd.Parameters["@actionID"].Value = actionID;
            oCmd.Parameters["@reason"].Value = reason;
            oCmd.Parameters["@addReason"].Value = addReason;
            oCmd.Parameters["@orderID"].Value = orderID;
            oCmd.Parameters["@extOrderID"].Value = extOrderID;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }

        public static void AddToOrderLog(int orderid, string message, string user)
        {
            AddToOrderLog(orderid, message, user, false);
        }
        public static void AddToOrderLog(int orderid, string message, string user, bool customerVisible)
        {
            AddToOrderLog(orderid, message, user, customerVisible,"N/A");
        }
        public static void AddToOrderLog(int orderid, string message, string user, bool customerVisible,string contact)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procOrderLogInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@orderid", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@message", SqlDbType.VarChar, 4000));
            oCmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 30));
            oCmd.Parameters.Add(new SqlParameter("@customerVisible", SqlDbType.Bit));
            oCmd.Parameters.Add(new SqlParameter("@contact", SqlDbType.VarChar,50));
            oCmd.Parameters["@orderid"].Value = orderid;
            oCmd.Parameters["@message"].Value = message;
            oCmd.Parameters["@username"].Value = user;
            oCmd.Parameters["@customerVisible"].Value = customerVisible;
            oCmd.Parameters["@contact"].Value = contact;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }

        public static void addToSalesLedger(int id, int retID, int actionID, string countryCode, int orderPrefix, decimal credit, decimal debit, decimal creditVat, decimal debitVat)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procSalesLedgerInsert", oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@returnID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@actionID", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@countryCode", SqlDbType.VarChar, 5));
            oCmd.Parameters.Add(new SqlParameter("@orderPrefix", SqlDbType.Int));
            oCmd.Parameters.Add(new SqlParameter("@ledgerCredit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@ledgerDebit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@ledgerVatCredit", SqlDbType.Decimal));
            oCmd.Parameters.Add(new SqlParameter("@ledgerVatDebit", SqlDbType.Decimal));
            oCmd.Parameters["@orderID"].Value = id;
            oCmd.Parameters["@returnID"].Value = retID;
            oCmd.Parameters["@actionID"].Value = actionID;
            oCmd.Parameters["@countryCode"].Value = countryCode;
            oCmd.Parameters["@orderPrefix"].Value = orderPrefix;
            oCmd.Parameters["@ledgerCredit"].Value = credit;
            oCmd.Parameters["@ledgerDebit"].Value = debit;
            oCmd.Parameters["@ledgerVatCredit"].Value = creditVat;
            oCmd.Parameters["@ledgerVatDebit"].Value = debitVat;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }
        public string getGoogleRef(string cc)
        {
            string result = "UA-4553866-1";
            if (cc.ToLower() == "us") result = "UA-4553866-2";
            if (cc.ToLower() == "ie") result = "UA-4553866-3";
            if (cc.ToLower() == "fr") result = "UA-4553866-4";
            if (cc.ToLower() == "de") result = "UA-4553866-5";
            if (cc.ToLower() == "es") result = "UA-4553866-6";
            if (cc.ToLower() == "be") result = "UA-4553866-7";
            if (cc.ToLower() == "dk") result = "UA-4553866-9";
            if (cc.ToLower() == "nl") result = "UA-4553866-10";
            if (cc.ToLower() == "ca") result = "UA-4553866-11";
            return result;
        }
        public string getCurrencySignByCurrencyCode(string code)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand("procCurrencyByCodeSelect", oConn);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string result="";
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.Parameters.Add(new SqlParameter("@currency", SqlDbType.VarChar, 3));
            oCmd.Parameters["@currency"].Value = code;
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                da = new SqlDataAdapter(oCmd);
                da.Fill(ds);
                if(ds.Tables[0].Rows.Count>0)result=ds.Tables[0].Rows[0]["currencySign"].ToString();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                ds.Dispose();
                da.Dispose();
                oCmd.Dispose();
                oConn.Dispose();
            }
            return result;
        }
        public string getOrderType(string ot)
        {
            string result="";
            switch (ot)
            {
                case ("cc"):
                    result = "Credit Card";
                    break;
                case ("affaccount"):
                    result = "Affiliate Account";
                    break;
                case ("affcc"):
                    result = "Affilite Credit Card";
                    break;
                case ("distcc"):
                    result = "Distributor Credit Card";
                    break;
                case ("distaccount"):
                    result = "Distributor Account";
                    break;
                case ("distaccoun"):
                    result = "Distributor Account";
                    break;
                case ("cheque"):
                    result = "Cheque";
                    break;
                case ("phone"):
                    result = "Phone";
                    break;
            }
            return result;
        }
        public static DataTable getDataTable(string[] param, string[] paramValue, SqlDbType[] paramType, int[] paramSize, string storedProcedure)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand(storedProcedure, oConn);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            oCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < param.Length; i++)
            {
                if (paramType[i] == SqlDbType.VarChar)
                    oCmd.Parameters.Add(new SqlParameter(param[i], paramType[i], paramSize[i]));
                else
                    oCmd.Parameters.Add(new SqlParameter(param[i], paramType[i]));
                oCmd.Parameters[param[i]].Value = paramValue[i];
            }
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                da = new SqlDataAdapter(oCmd);
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds.Dispose();
                da.Dispose();
                oCmd.Dispose();
                oConn.Dispose();
            }
            return dt;
        }
        public static void executeNonQuery(string[] param, string[] paramValue, SqlDbType[] paramType, int[] paramSize, string storedProcedure)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand(storedProcedure, oConn);
            oCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < param.Length; i++)
            {
                if (paramType[i] == SqlDbType.VarChar)
                    oCmd.Parameters.Add(new SqlParameter(param[i], paramType[i], paramSize[i]));
                else
                    oCmd.Parameters.Add(new SqlParameter(param[i], paramType[i]));
                oCmd.Parameters[param[i]].Value = paramValue[i];
            }
            try
            {
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }
        public static void executeSQLStatement(string sql)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand();
            try
            {
                string cmd = sql;
                oCmd = new SqlCommand(cmd, oConn);
                oCmd.CommandType = CommandType.Text;
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //throw back.
                throw e;
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }
        public static DataTable getSQLStatement(string sql)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                string cmd = sql;
                oCmd = new SqlCommand(cmd, oConn);
                oCmd.CommandType = CommandType.Text;
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                da = new SqlDataAdapter(oCmd);
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                //throw back.
                throw e;
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
            return new DataTable();
        }
        public static void addToAffiliateLog(int affID, string msg, bool affVisible, string contact, string username)
        {
            try
            {
                string[] param = new string[] { "@affID", "@msg", "@affVisible", "@contact", "@username" };
                string[] paramValue = new string[] { affID.ToString(), msg, affVisible.ToString(), contact, username};
                SqlDbType[] paramType = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.VarChar };
                int[] paramSize = new int[] { 0, -1, 0, 50, 20 };
                executeNonQuery(param, paramValue, paramType, paramSize, "procAffiliateContactLogInsert");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
            }
        }

        public static void sendEmail(string to, string cc, string bcc, string subject, string from, string fromName, string body)
        {
            string[] singleAdd = new string[1];
            singleAdd[0] = to;
            sendEmail(singleAdd, cc, bcc, subject, from, fromName, body);
        }
        public static void sendEmail(string[] to, string cc, string bcc, string subject, string from, string fromName, string body)
        {
            AlternateView plainView;
            AlternateView htmlView;
            MailMessage mail = new MailMessage();
            mail.To.Add(to[0]);
            if (cc != "") mail.CC.Add(cc);
            if (bcc != "") mail.Bcc.Add(bcc);
            mail.Subject = subject;
            mail.From = new MailAddress(from,fromName);
            mail.IsBodyHtml = true;
            char t = (char)10;
            plainView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
            htmlView = AlternateView.CreateAlternateViewFromString(body.Replace(t.ToString(), "<br>"), null, "text/html");
            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);
            SmtpClient client = new SmtpClient();
            client.Send(mail);
            mail.Dispose();
        }
    public static void sendContactEmail(string to, string cc, string bcc, string subject, string from, string fromName, string body)
    {
        AlternateView plainView;
        AlternateView htmlView;
        MailMessage mail = new MailMessage();
        mail.To.Add(to);
        if (cc != "") mail.CC.Add(cc);
        if (bcc != "") mail.Bcc.Add(bcc);
        mail.Subject = subject;
        mail.From = new MailAddress(from, fromName);
        mail.IsBodyHtml = true;
        char t = (char)10;
        plainView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
        htmlView = AlternateView.CreateAlternateViewFromString(body.Replace(t.ToString(), "<br>"), null, "text/html");
        mail.AlternateViews.Add(plainView);
        mail.AlternateViews.Add(htmlView);
        SmtpClient client = new SmtpClient();
        client.Host = "smtpout.secureserver.net";
        client.EnableSsl = true;
        NetworkCredential NetworkCred = new NetworkCredential("noreply@emotionalbliss.co.uk", "No370456@emotionalbliss");
        client.UseDefaultCredentials = false;
        client.Credentials = NetworkCred;
        client.Port = 3535;
        client.Send(mail);
        mail.Dispose();
    }
    public static int sendSQLEmail(string to, string cc, string bcc, string subject, string from, string fromName, string body, _emailType emailType)
        {
            //sendEmail(to,cc,bcc,subject,from,fromName,body);
            //return 0;

            int result = 0;
            string sEmailTextType = "";
            switch (emailType)
            {
                case (_emailType.emailText):
                    sEmailTextType = "TEXT";
                    break;
                case (_emailType.emailHtml):
                    sEmailTextType = "HTML";
                    break;
            };
            DataTable dt = new DataTable();
            try
            {
                string[] param = new string[] { "@sp_profile_name", "@sp_recipients", "@sp_copy_recipients", "@sp_blind_copy_recipients", "@sp_body", "@sp_subject", "@sp_body_format" };
                string[] paramValue = new string[] { ConfigurationManager.AppSettings["databaseEmailProfile"], to, cc, bcc, body, subject, sEmailTextType };
                SqlDbType[] paramType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                int[] paramSize = new int[] { 100, -1, -1, -1, -1, -1, 20 };
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["isDev"]))
                {
                    paramValue[1] = "scott@emotionalbliss.com";
                }
                dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procMSDBSendEmail");
                if (dt.Rows.Count > 0) result = Convert.ToInt32(dt.Rows[0]["mailitem_id"]);
            }
            catch (Exception e)
            {
                addError("siteInclude.cs", "sendSQLEmail(); " + e.ToString());
                throw e;
            }
            finally
            {
                dt.Dispose();
            }
            return result;
        }
        public static int getGVRowByHeader(GridView gv, string headertext)
        {
            int result = 0;
            int index = 0;
            DataControlFieldCell fieldCell;
            if (gv.Rows.Count > 0)
            {
                foreach (TableCell cell in gv.Rows[0].Cells)
                {
                    fieldCell = (DataControlFieldCell)cell;
                    if (fieldCell.ContainingField.HeaderText.ToLower().Equals(headertext.ToLower()))
                    {
                        result = index;
                    }
                    index++;
                }
            }
            if (result == 0) throw (new Exception("Column header not found ('" + headertext + "')"));
            return result;
        }
        public static int getDVRowByHeader(DetailsView dv, string headertext)
        {
            int result = 99;
            int index = 0;
            //DataControlFieldCell fieldCell;
            if (dv.Rows.Count > 0)
            {
                foreach (DetailsViewRow row in dv.Rows)
                {
                    if (row.Cells[0].Text.ToLower().Equals(headertext.ToLower())) result = index;
                    index++;
                }
            }
            if (result == 99) throw (new Exception("Row header not found ('" + headertext + "')"));
            return result;
        }
        public static void setDropdownTextByID(string txt, string value, DropDownList drp)
        {
            foreach (ListItem item in drp.Items)
                if (item.Value == value)
                    item.Text = trimCrap(txt);            
        }
        public static void addItemToDropdown(DropDownList drp)
        {
            SortedList ht = new SortedList();
            if (drp.Items.Count > 0)
            {
                //Setup the Sorted List.
                foreach (ListItem li in drp.Items)
                {
                    ht.Add(li.Text + "@" + li.Value, li.Value);
                }
                //Clear the dropdown and re-add new values, starting with the Select... option.
                drp.Items.Clear();
                drp.Items.Add(new ListItem("Select...", ""));
                foreach (DictionaryEntry de in ht)
                {
                    drp.Items.Add(new ListItem(de.Key.ToString().Substring(0, de.Key.ToString().IndexOf("@")), de.Value.ToString()));
                }
            }
        }
        public static void addItemToDropdown(DropDownList drp, string txt, string val)
        {
            addItemToDropdown(drp, txt, val, true);
        }
        public static void addItemToDropdown(DropDownList drp, string txt, string val, bool ascending)
        {
            SortedList ht = new SortedList();
            if (drp.Items.Count > 0)
            {
                //Setup the Sorted List.
                foreach (ListItem li in drp.Items)
                {
                    ht.Add(li.Text + "@" + li.Value, li.Value);
                }
                //Clear the dropdown and re-add new values, starting with the Select... option.
                drp.Items.Clear();
                drp.Items.Add(new ListItem(txt, val));
                if (ascending)
                    foreach (DictionaryEntry de in ht)
                    {
                        drp.Items.Add(new ListItem(de.Key.ToString().Substring(0, de.Key.ToString().IndexOf("@")), de.Value.ToString()));
                    }
                else
                {
                    string[,] a = new string[2, Convert.ToInt16(ht.Keys.Count)];
                    int i = 0;
                    foreach (DictionaryEntry de in ht)
                    {
                        a[0, i] = de.Key.ToString().Substring(0, de.Key.ToString().IndexOf("@"));
                        a[1, i++] = de.Value.ToString();
                        //drp.Items.Add(new ListItem(de.Key.ToString().Substring(0, de.Key.ToString().IndexOf("@")), de.Value.ToString()));
                    }
                    for (int iLoop = (a.Length / 2) - 1; iLoop >= 0; iLoop--)
                        drp.Items.Add(new ListItem(a[0, iLoop], a[1, iLoop]));
                }
            }
        }
        public static void addItemToDropdownNoSort(DropDownList drp, string txt, string val)
        {
            ListDictionary l = new ListDictionary();
            foreach (ListItem li in drp.Items)
                l.Add(li.Text + "@" + li.Value, li.Value);
            drp.Items.Clear();
            drp.Items.Add(new ListItem(txt, val));
            foreach (DictionaryEntry de in l)
                drp.Items.Add(new ListItem(de.Key.ToString().Substring(0, de.Key.ToString().IndexOf("@")), de.Value.ToString()));
        }
        public static string getCurrencyCodeByCountryCode(string cc)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] param = new string[] { "@countryCode" };
                string[] paramValue = new string[] { cc };
                SqlDbType[] paramType = new SqlDbType[] { SqlDbType.VarChar };
                int[] paramSize = new int[] { 5 };
                dt = getDataTable(param, paramValue, paramType, paramSize, "procCurrencyByCounrtyCodeSelect2");
                if (dt.Rows.Count>0){
                    return (string)dt.Rows[0]["currencySign"];
                }
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                dt.Dispose();
            }
        }
        public static string getSagePayURL(string connectTo, string strType)
        {
            string result = "";
            switch (strType.ToLower())
            {
                case ("abort"):
                    result = ".sagepay.com/gateway/service/abort.vsp";
                    break;
                case ("authorise"):
                    //result = "https://ukvps.protx.com/vspgateway/service/authorise.vsp";
                    break;
                case ("cancel"):
                    result = ".sagepay.com/gateway/service/cancel.vsp";
                    break;
                case ("purchase"):
                    result = ".sagepay.com/gateway/service/vspdirect-register.vsp";
                    break;
                case ("refund"):
                    result = ".sagepay.com/gateway/service/directrefund.vsp";
                    break;
                case ("release"):
                    result = ".sagepay.com/gateway/service/release.vsp";
                    break;
                case ("repeat"):
                    result = ".sagepay.com/gateway/service/repeat.vsp";
                    break;
                case ("void"):
                    result = ".sagepay.com/gateway/service/void.vsp";
                    break;
                case ("3dcallback"):
                    result = ".sagepay.com/gateway/service/direct3dcallback.vsp";
                    break;
                case ("showpost"):
                    //result = "https://ukvpstest.protx.com/showpost/showpost.asp";
                    break;
                case ("complete"):
                    result = ".sagepay.com/gateway/service/complete.vsp";
                    break;
            }
            return "https://" + connectTo.ToLower() + result;
        }
        public static void updateReceiptField(int orderID, string field, string val, bool includeQuotes)
        {
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand oCmd = new SqlCommand();
            try
            {
                string cmd = "UPDATE shopOrder SET " + field + "=";
                if (includeQuotes) cmd += "'";
                cmd += val;
                if (includeQuotes) cmd += "'";
                cmd += " WHERE id=" + orderID.ToString();
                oCmd = new SqlCommand(cmd, oConn);
                oCmd.CommandType = CommandType.Text;
                debug(cmd);
                if (oCmd.Connection.State == 0) oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //throw back.
                throw e;
            }
            finally
            {
                oCmd.Dispose();
                oConn.Dispose();
            }
        }
        public static string getTrackerURL(string tracker, string countryCode)
        {
            //
            DataTable dt = new DataTable();
            string result="";
            try
            {
                string[] param = new string[] { "@tracker" };
                string[] paramValue = new string[] { tracker };
                SqlDbType[] paramType = new SqlDbType[] { SqlDbType.VarChar };
                int[] paramSize = new int[] { 30 };
                dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procCourierByTrackerSelect");
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["url"].ToString();
                    result = result.Replace("@tracker", tracker);
                    result = result.Replace("@countrycode", countryCode);
                    result = result.Replace("@postcode", dt.Rows[0]["postcode"].ToString().Trim().Replace(" ", "+"));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                dt.Dispose();
            }
            return result;
        }
        public static string trimCrap(string html)
        {
            return Regex.Replace(html, "<(.|\n)*?>", String.Empty);
        }
    }
