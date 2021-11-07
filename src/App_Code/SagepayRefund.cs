using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Net;
using System.Text;

/// <summary>
/// Summary description for SagepayRefund
/// </summary>
public class SagepayRefund
{
    private string _sResponse;
    private int _orderID;
    private int _paymentID;
    private bool _debug = false;
    public sagepayObject pay = new sagepayObject();
    public enum IDType { OrderID, PaymentID, SagepayID };

	public SagepayRefund()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public SagepayRefund(int orderID) { _orderID = orderID; }

    public static string getSagePayURL(string connectTo, string strType)
    {
        string result = "";
        switch (strType.ToLower())
        {
            case ("abort"):
                result = ".sagepay.com/gateway/service/abort.vsp";
                break;
            case ("authorise"):
                result = ".sagepay.com/gateway/service/authorise.vsp";
                break;
            case ("cancel"):
                result = ".sagepay.com/gateway/service/cancel.vsp";
                break;
            case ("purchase"):
                result = ".sagepay.com/gateway/service/vspdirect-register.vsp";
                break;
            case ("refund"):
                result = ".sagepay.com/gateway/service/refund.vsp";
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
    public void postData(string sConnectTo)
    {
        string strPost = "VPSProtocol=" + pay.VPSProtocol;
        switch (sConnectTo.ToLower())
        {
            case "complete":
                strPost += "&TxType=" + HttpUtility.UrlEncode(pay.TxType);
                strPost += "&VPSTxId=" + HttpUtility.UrlEncode(pay.VPSTxId);
                strPost += "&Amount=" + HttpUtility.UrlEncode(pay.Amount);
                strPost += "&Accept=" + HttpUtility.UrlEncode(pay.Accept);
                break;
            case "release":
                strPost += "&TxType=" + HttpUtility.UrlEncode(pay.TxType);
                strPost += "&Vendor=" + HttpUtility.UrlEncode(pay.Vendor);
                strPost += "&VendorTxCode=" + HttpUtility.UrlEncode(pay.VendorTxCode);
                strPost += "&VPSTxId=" + HttpUtility.UrlEncode(pay.VPSTxId);
                strPost += "&SecurityKey=" + HttpUtility.UrlEncode(pay.Securitykey);
                strPost += "&TxAuthNo=" + HttpUtility.UrlEncode(pay.TxAuthNo);
                strPost += "&ReleaseAmount=" + HttpUtility.UrlEncode(pay.ReleaseAmount);
                break;
            case "refund":
                strPost += "&TxType=" + HttpUtility.UrlEncode(pay.TxType);
                strPost += "&Vendor=" + HttpUtility.UrlEncode(pay.Vendor);
                strPost += "&VendorTxCode=" + HttpUtility.UrlEncode(pay.VendorTxCode);
                strPost += "&Amount=" + HttpUtility.UrlEncode(pay.Amount);
                strPost += "&Currency=" + HttpUtility.UrlEncode(pay.Currency);
                strPost += "&Description=" + HttpUtility.UrlEncode(pay.Description);
                strPost += "&RelatedVPSTxId=" + HttpUtility.UrlEncode(pay.RelatedVPSTxId);
                strPost += "&RelatedVendorTxCode=" + HttpUtility.UrlEncode(pay.RelatedVendorTxCode);
                strPost += "&RelatedSecurityKey=" + HttpUtility.UrlEncode(pay.RelatedSecuritykey);
                strPost += "&RelatedTxAuthNo=" + HttpUtility.UrlEncode(pay.RelatedTxAuthNo);
                break;
            default:
                strPost += "&TxType=" + HttpUtility.UrlEncode(pay.TxType);
                strPost += "&Vendor=" + HttpUtility.UrlEncode(pay.Vendor);
                strPost += "&VendorTxCode=" + HttpUtility.UrlEncode(pay.VendorTxCode);
                strPost += "&Amount=" + HttpUtility.UrlEncode(pay.Amount);
                strPost += "&Currency=" + HttpUtility.UrlEncode(pay.Currency);
                strPost += "&Description=" + HttpUtility.UrlEncode(pay.Description);
                strPost += "&CardHolder=" + HttpUtility.UrlEncode(pay.CardHolder);
                strPost += "&CardNumber=" + HttpUtility.UrlEncode(pay.CardNumber);
                strPost += "&StartDate=" + HttpUtility.UrlEncode(pay.StartDate);
                strPost += "&ExpiryDate=" + HttpUtility.UrlEncode(pay.ExpiryDate);
                strPost += "&IssueNumber=" + HttpUtility.UrlEncode(pay.IssueNumber);
                strPost += "&CV2=" + HttpUtility.UrlEncode(pay.CV2);
                strPost += "&CardType=" + HttpUtility.UrlEncode(pay.CardType);
                strPost += "&BillingFirstnames=" + HttpUtility.UrlEncode(pay.BillingFirstnames);
                strPost += "&BillingSurname=" + HttpUtility.UrlEncode(pay.BillingSurname);
                strPost += "&BillingAddress1=" + HttpUtility.UrlEncode(pay.BillingAddress1);
                strPost += "&BillingAddress2=" + HttpUtility.UrlEncode(pay.BillingAddress2);
                strPost += "&BillingCity=" + HttpUtility.UrlEncode(pay.BillingCity);
                strPost += "&BillingPostCode=" + HttpUtility.UrlEncode(pay.BillingPostCode);
                strPost += "&BillingCountry=" + HttpUtility.UrlEncode(pay.BillingCountry);
                strPost += "&BillingState=" + HttpUtility.UrlEncode(pay.BillingState);
                strPost += "&BillingPhone=" + HttpUtility.UrlEncode(pay.BillingPhone);
                strPost += "&DeliveryFirstnames=" + HttpUtility.UrlEncode(pay.DeliveryFirstnames);
                strPost += "&DeliverySurname=" + HttpUtility.UrlEncode(pay.DeliverySurname);
                strPost += "&DeliveryAddress1=" + HttpUtility.UrlEncode(pay.DeliveryAddress1);
                strPost += "&DeliveryAddress2=" + HttpUtility.UrlEncode(pay.DeliveryAddress2);
                strPost += "&DeliveryCity=" + HttpUtility.UrlEncode(pay.DeliveryCity);
                strPost += "&DeliveryPostCode=" + HttpUtility.UrlEncode(pay.DeliveryPostCode);
                strPost += "&DeliveryCountry=" + HttpUtility.UrlEncode(pay.DeliveryCountry);
                strPost += "&DeliveryState=" + HttpUtility.UrlEncode(pay.DeliveryState);
                strPost += "&DeliveryPhone=" + HttpUtility.UrlEncode(pay.DeliveryPhone);
                strPost += "&PayPalCallbackURL=" + HttpUtility.UrlEncode(pay.PayPalCallbackURL);
                strPost += "&CustomerEMail=" + HttpUtility.UrlEncode(pay.CustomerEMail);
                strPost += "&GiftAidPayment=" + HttpUtility.UrlEncode(pay.GiftAidPayment);
                strPost += "&ApplyAVSCV2=" + HttpUtility.UrlEncode(pay.ApplyAVSCV2);
                strPost += "&ClientIPAddress=" + HttpUtility.UrlEncode(pay.ClientIPAddress);
                strPost += "&Apply3DSecure=" + HttpUtility.UrlEncode(pay.Apply3DSecure);
                strPost += "&AccountType=" + HttpUtility.UrlEncode(pay.AccountType);
                strPost += "&MD=" + HttpUtility.UrlEncode(pay.MD);
                strPost += "&PARes=" + HttpUtility.UrlEncode(pay.PARes);
                break;
        };

        if (_debug) siteInclude.debug(strPost);
        UTF8Encoding objUTFEncode = new UTF8Encoding();
        Byte[] arrRequest;
        Stream objStreamReq;
        StreamReader objStreamRes;
        HttpWebRequest objHttpRequest;
        HttpWebResponse objHttpResponse;
        Uri objUri;
        if (pay.CardType.ToUpper() == "PAYPAL" || pay.TxType.ToUpper() == "COMPLETE")
            objUri = new Uri(getSagePayURL("live", sConnectTo)); //Paypal doesnt work on test servers yet 10/8/09
        else
            objUri = new Uri(getSagePayURL(pay.Server, sConnectTo));
        if (_debug) siteInclude.debug("URI=" + getSagePayURL(pay.Server, sConnectTo));
        objHttpRequest = (HttpWebRequest)WebRequest.Create(objUri);
        objHttpRequest.KeepAlive = false;
        objHttpRequest.Method = "POST";
        objHttpRequest.ContentType = "application/x-www-form-urlencoded";
        arrRequest = objUTFEncode.GetBytes(strPost);
        objHttpRequest.ContentLength = arrRequest.Length;
        try
        {
            objStreamReq = objHttpRequest.GetRequestStream();
            objStreamReq.Write(arrRequest, 0, arrRequest.Length);
            objStreamReq.Close();
        }
        catch (Exception e)
        {
            siteInclude.debug("sagesap.cs::postData; Error getting response");
            throw (e);
        }
        //Get response
        objHttpResponse = (HttpWebResponse)objHttpRequest.GetResponse();
        objStreamRes = new StreamReader(objHttpResponse.GetResponseStream(), Encoding.ASCII);
        _sResponse = objStreamRes.ReadToEnd();
        if (_debug) siteInclude.debug("response=" + _sResponse);
        objStreamRes.Close();
        switch (findField("Status").ToLower())
        {
            case ("3dauth"):
                //Store so aspx page can access values
                pay.ACSURL = findField("ACSURL");
                pay.PAReq = findField("PAReq");
                pay.MD = findField("MD");
                break;
            case ("ppredirect"):
                pay.PayPalRedirectURL = findField("PayPalRedirectURL");
                break;
        }
    }
    public void storeRefundResponse()
    {
        try
        {
            string[] param = new string[] { "@VPSProtocol", "@Status", "@StatusDetail", "@VendorTxCode", "@Amount", "@Currency", "@RelatedVPSTxId", "@RelatedVendorTxCode", "@RelatedSecurityKey", "@RelatedTxAuthNo", "@VPSTxId", "@TxAuthNo", "@orderID", "@TxType" };
            string[] paramValue = new string[] { findField("VPSProtocol"), findField("Status"), findField("StatusDetail"), pay.VendorTxCode, pay.Amount, pay.Currency, pay.RelatedVPSTxId, pay.RelatedVendorTxCode, pay.RelatedSecuritykey, pay.RelatedTxAuthNo, pay.VPSTxId, pay.TxAuthNo, _orderID.ToString(), pay.TxType };
            SqlDbType[] paramType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar };
            int[] paramSize = new int[] { 4, 15, 255, 40, 0, 3, 38, 50, 10, 50, 38, 50, 0, 15 };
            /*siteInclude.debug(param.Length.ToString());
            siteInclude.debug(paramValue.Length.ToString());
            siteInclude.debug(paramType.Length.ToString());
            siteInclude.debug(paramSize.Length.ToString());*/
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "dalProtxRefundInsert");
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public string findField(string fieldName)
    {
        //string[] arrItems=new string[1];
        string[] arrItems;
        string result;
        arrItems = _sResponse.Split((char)10);
        for (int iItem = 0; iItem < arrItems.Length; iItem++)
        {
            if (arrItems[iItem].IndexOf(fieldName + "=") == 0)
            {
                //result = arrItems[iItem].Split((char)'=');
                //return result[1].Trim();
                result = arrItems[iItem].Substring(arrItems[iItem].IndexOf('=') + 1);
                return result.Trim();
            }
        }
        return "";
    }
    public void dumpSnapshot()
    {
        pay.dumpSnapshot();
    }
}
[Serializable]
public class sagepayObject
{
    public sagepayObject()
    {

    }
    public void dumpSnapshot()
    {
        //Dumps all current values into the debug table
        siteInclude.debug("_VPSProtocol=" + _VPSProtocol);
        siteInclude.debug("_TxType =" + _TxType);
        siteInclude.debug("_TxAuthNo =" + _TxAuthNo);
        siteInclude.debug("_Vendor =" + _Vendor);
        siteInclude.debug("_VendorTxCode =" + _VendorTxCode);
        siteInclude.debug("_VPSTxId =" + _VPSTxId);
        siteInclude.debug("_RelatedVendorTxCode =" + _RelatedVendorTxCode);
        siteInclude.debug("_RelatedVPSTxId =" + _RelatedVPSTxId);
        siteInclude.debug("_RelatedSecurityKey =" + _RelatedSecurityKey);
        siteInclude.debug("_RelatedTxAuthNo =" + _RelatedTxAuthNo);
        siteInclude.debug("_SecurityKey =" + _SecurityKey);
        siteInclude.debug("_Amount =" + _Amount);
        siteInclude.debug("_ReleaseAmount =" + _ReleaseAmount);
        siteInclude.debug("_Currency =" + _Currency);
        siteInclude.debug("_Description =" + _Description);
        siteInclude.debug("_CardHolder =" + _CardHolder);
        siteInclude.debug("_CardNumber =" + _CardNumber);
        siteInclude.debug("_StartDate =" + _StartDate);
        siteInclude.debug("_ExpiryDate =" + _ExpiryDate);
        siteInclude.debug("_IssueNumber =" + _IssueNumber);
        siteInclude.debug("_CV2 =" + _CV2);
        siteInclude.debug("_CardType =" + _CardType);
        siteInclude.debug("_BillingSurname =" + _BillingSurname);
        siteInclude.debug("_BillingFirstnames =" + _BillingFirstnames);
        siteInclude.debug("_BillingAddress1 =" + _BillingAddress1);
        siteInclude.debug("_BillingAddress2 =" + _BillingAddress2);
        siteInclude.debug("_BillingCity =" + _BillingCity);
        siteInclude.debug("_BillingPostCode =" + _BillingPostCode);
        siteInclude.debug("_BillingCountry =" + _BillingCountry);
        siteInclude.debug("_BillingState =" + _BillingState);
        siteInclude.debug("_BillingPhone =" + _BillingPhone);
        siteInclude.debug("_DeliverySurname =" + _DeliverySurname);
        siteInclude.debug("_DeliveryFirstnames =" + _DeliveryFirstnames);
        siteInclude.debug("_DeliveryAddress1 =" + _DeliveryAddress1);
        siteInclude.debug("_DeliveryAddress2 =" + _DeliveryAddress2);
        siteInclude.debug("_DeliveryCity =" + _DeliveryCity);
        siteInclude.debug("_DeliveryPostCode =" + _DeliveryPostCode);
        siteInclude.debug("_DeliveryCountry =" + _DeliveryCountry);
        siteInclude.debug("_DeliveryState =" + _DeliveryState);
        siteInclude.debug("_DeliveryPhone =" + _DeliveryPhone);
        siteInclude.debug("_PayPalCallbackURL =" + _PayPalCallbackURL);
        siteInclude.debug("_PayPalRedirectURL =" + _PayPalRedirectURL);
        siteInclude.debug("_CustomerEMail  =" + _CustomerEMail);
        siteInclude.debug("_GiftAidPayment =" + _GiftAidPayment);
        siteInclude.debug("_Apply3DSecure=" + _Apply3DSecure);
        siteInclude.debug("_AccountType=" + _AccountType);
        siteInclude.debug("_Server=" + _Server);
        siteInclude.debug("_ACSURL=" + _ACSURL);
        siteInclude.debug("_PARes=" + _PARes);
        siteInclude.debug("_MD=" + _MD);
        siteInclude.debug("_Accept=" + _Accept);

    }
    //Members
    private string _VPSProtocol = ""; //Alph(4)
    private string _TxType = ""; //Alph(15)
    private string _TxAuthNo = ""; //Long Int
    private string _Vendor = ""; //Alph(15)
    private string _VendorTxCode = ""; //Aplh(40)
    private string _VPSTxId = ""; //Aplh(40)
    private string _RelatedVendorTxCode = ""; //Aplh(40)
    private string _RelatedVPSTxId = ""; //Aplh(38)
    private string _RelatedSecurityKey = ""; //Aplh(10)
    private string _RelatedTxAuthNo = ""; //Long Int
    private string _SecurityKey = ""; //Aplh(10)
    private string _Amount = ""; //Numeric (1.00 to 100,000.00)
    private string _ReleaseAmount = ""; //Numeric (1.00 to 100,000.00)
    private string _Currency = ""; //Alph(3)
    private string _Description = ""; //Alph(100)
    private string _CardHolder = ""; //Alph(50) - (Ignored for paypal)
    private string _CardNumber = ""; //Numeric(20) - (Ignored for paypal)
    private string _StartDate = ""; //Numeric(4) - OPTIONAL - (Ignored for paypal)
    private string _ExpiryDate = ""; //Numeric(4) - (Ignored for paypal)
    private string _IssueNumber = ""; //Numeric(2) - OPTIONAL - (Ignored for paypal)
    private string _CV2 = ""; //Numeric(4) - OPTIONAL - (Ignored for paypal)
    private string _CardType = ""; //Alph(15) “VISA”, ”MC”, ”DELTA”, “SOLO”, “MAESTRO”, “UKE”, “AMEX”, “DC”, “JCB”, “LASER”, “PAYPAL”
    private string _BillingSurname = ""; //Alph(20)
    private string _BillingFirstnames = ""; //Alph(20)
    private string _BillingAddress1 = ""; //Alph(100)
    private string _BillingAddress2 = ""; //Alph(100) - OPTIONAL
    private string _BillingCity = ""; //Alph(40)
    private string _BillingPostCode = ""; //Alph(10)
    private string _BillingCountry = ""; //Alph(2) ISO 3166-1 Countrycode
    private string _BillingState = ""; //Alph(2) - OPTIONAL
    private string _BillingPhone = ""; //Alph(20) - OPTIONAL
    private string _DeliverySurname = ""; //Alph(20)
    private string _DeliveryFirstnames = ""; //Alph(20)
    private string _DeliveryAddress1 = ""; //Alph(100)
    private string _DeliveryAddress2 = ""; //Alph(100) - OPTIONAL
    private string _DeliveryCity = ""; //Alph(40)
    private string _DeliveryPostCode = ""; //Alph(10)
    private string _DeliveryCountry = ""; //Alph(2) ISO 3166-1 Countrycode
    private string _DeliveryState = ""; //Alph(2) - OPTIONAL
    private string _DeliveryPhone = ""; //Alph(20) - OPTIONAL
    private string _PayPalCallbackURL = ""; //Alph(255) - OPTIONAL
    private string _PayPalRedirectURL = "";  //Alph(255)
    private string _CustomerEMail = ""; //Alph(255) - OPTIONAL
    private string _GiftAidPayment = ""; //Flag(0=No, 1=Yes) - OPTIONAL
    private string _ApplyAVSCV2 = ""; //Flag(0=Default/use rules, 1=Force check(If rules apply use them), 2=Force no check, 3=Force check(if rules apply ignore them) - OPTIONAL - (Ignored for paypal)
    private string _ClientIPAddress = ""; //Alph(15) - OPTIONAL
    private string _Apply3DSecure = ""; //Flag(0=Default/use rules), 1=Force(If rules apply use them), 2=Force no check, 3=Force check, Always obtain authcode irrespective of rules) - OPTIONAL - (Ignored for paypal)
    private string _AccountType = "E"; //Alph(1) E=Default/ecommerce, C=Merchant account, M=mail order/phone order - OPTIONAL - (Ignored for paypal)
    private string _Server = ""; //Alph(4)(LIVE or TEST)    
    private string _ACSURL = ""; //Cant be stored in db so temp store here so vb page can access it
    private string _PAReq = ""; //Cant be stored in db so temp store here so vb page can access it
    private string _PARes = ""; //Cant be stored in db so temp store here so vb page can access it
    private string _MD = ""; //Cant be stored in db so temp store here so vb page can access it
    private string _Accept = ""; //Alph(3) - PAYPAL ONLY - OPTIONAL

    //Accessors
    public string VPSProtocol
    {
        get { return _VPSProtocol; }
        set { _VPSProtocol = value; }
    }
    public string TxType
    {
        get { return _TxType; }
        set { _TxType = value; }
    }
    public string TxAuthNo
    {
        get { return _TxAuthNo; }
        set { _TxAuthNo = value; }
    }
    public string VPSTxId
    {
        get { return _VPSTxId; }
        set { _VPSTxId = value; }
    }
    public string Vendor
    {
        get { return _Vendor; }
        set { _Vendor = value; }
    }
    public string VendorTxCode
    {
        get { return _VendorTxCode; }
        set { _VendorTxCode = value; }
    }
    public string RelatedVendorTxCode
    {
        get { return _RelatedVendorTxCode; }
        set { _RelatedVendorTxCode = value; }
    }
    public string RelatedVPSTxId
    {
        get { return _RelatedVPSTxId; }
        set { _RelatedVPSTxId = value; }
    }
    public string RelatedSecuritykey
    {
        get { return _RelatedSecurityKey; }
        set { _RelatedSecurityKey = value; }
    }
    public string RelatedTxAuthNo
    {
        get { return _RelatedTxAuthNo; }
        set { _RelatedTxAuthNo = value; }
    }
    public string Securitykey
    {
        get { return _SecurityKey; }
        set { _SecurityKey = value; }
    }
    public string Amount
    {
        get { return String.Format("{0:0.##}", _Amount); }
        set { _Amount = value; }
    }
    public string ReleaseAmount
    {
        get { return String.Format("{0:0.##}", _ReleaseAmount); }
        set { _ReleaseAmount = value; }
    }
    public string Currency
    {
        get { return _Currency; }
        set { _Currency = value; }
    }
    public string Description
    {
        get { return _Description; }
        set { _Description = value; }
    }
    public string CardHolder
    {
        get { return _CardHolder; }
        set { _CardHolder = value; }
    }
    public string CardNumber
    {
        get { return _CardNumber; }
        set { _CardNumber = value; }
    }
    public string StartDate
    {
        get { return _StartDate; }
        set { _StartDate = value; }
    }
    public string ExpiryDate
    {
        get { return _ExpiryDate; }
        set { _ExpiryDate = value; }
    }
    public string IssueNumber
    {
        get { return _IssueNumber; }
        set { _IssueNumber = value; }
    }
    public string CV2
    {
        get { return _CV2; }
        set { _CV2 = value; }
    }
    public string CardType
    {
        get { return _CardType; }
        set { _CardType = value; }
    }
    public string BillingSurname
    {
        get { return _BillingSurname; }
        set { _BillingSurname = value; }
    }
    public string BillingFirstnames
    {
        get { return _BillingFirstnames; }
        set { _BillingFirstnames = value; }
    }
    public string BillingAddress1
    {
        get { return _BillingAddress1; }
        set { _BillingAddress1 = value; }
    }
    public string BillingAddress2
    {
        get { return _BillingAddress2; }
        set { _BillingAddress2 = value; }
    }
    public string BillingCity
    {
        get { return _BillingCity; }
        set { _BillingCity = value; }
    }
    public string BillingPostCode
    {
        get { return _BillingPostCode; }
        set { _BillingPostCode = value; }
    }
    public string BillingCountry
    {
        get { return _BillingCountry; }
        set { _BillingCountry = value; }
    }
    public string BillingState
    {
        get { return _BillingState; }
        set { _BillingState = value; }
    }
    public string BillingPhone
    {
        get { return _BillingPhone; }
        set { _BillingPhone = value; }
    }
    public string DeliverySurname
    {
        get { return _DeliverySurname; }
        set { _DeliverySurname = value; }
    }
    public string DeliveryFirstnames
    {
        get { return _DeliveryFirstnames; }
        set { _DeliveryFirstnames = value; }
    }
    public string DeliveryAddress1
    {
        get { return _DeliveryAddress1; }
        set { _DeliveryAddress1 = value; }
    }
    public string DeliveryAddress2
    {
        get { return _DeliveryAddress2; }
        set { _DeliveryAddress2 = value; }
    }
    public string DeliveryCity
    {
        get { return _DeliveryCity; }
        set { _DeliveryCity = value; }
    }
    public string DeliveryPostCode
    {
        get { return _DeliveryPostCode; }
        set { _DeliveryPostCode = value; }
    }
    public string DeliveryCountry
    {
        get { return _DeliveryCountry; }
        set { _DeliveryCountry = value; }
    }
    public string DeliveryState
    {
        get { return _DeliveryState; }
        set { _DeliveryState = value; }
    }
    public string DeliveryPhone
    {
        get { return _DeliveryPhone; }
        set { _DeliveryPhone = value; }
    }
    public string PayPalCallbackURL
    {
        get { return _PayPalCallbackURL; }
        set { _PayPalCallbackURL = value; }
    }
    public string PayPalRedirectURL
    {
        get { return _PayPalRedirectURL; }
        set { _PayPalRedirectURL = value; }
    }
    public string CustomerEMail
    {
        get { return _CustomerEMail; }
        set { _CustomerEMail = value; }
    }
    public string GiftAidPayment
    {
        get { return _GiftAidPayment; }
        set { _GiftAidPayment = value; }
    }
    public string ApplyAVSCV2
    {
        get { return _ApplyAVSCV2; }
        set { _ApplyAVSCV2 = value; }
    }
    public string ClientIPAddress
    {
        get { return _ClientIPAddress; }
        set { _ClientIPAddress = value; }
    }
    public string Apply3DSecure
    {
        get { return _Apply3DSecure; }
        set { _Apply3DSecure = value; }
    }
    public string AccountType
    {
        get { return _AccountType; }
        set { _AccountType = value; }
    }
    public string Server
    {
        get { return _Server; }
        set { _Server = value; }
    }
    public string ACSURL
    {
        get { return _ACSURL; }
        set { _ACSURL = value; }
    }
    public string PAReq
    {
        get { return _PAReq; }
        set { _PAReq = value; }
    }
    public string PARes
    {
        get { return _PARes; }
        set { _PARes = value; }
    }
    public string MD
    {
        get { return _MD; }
        set { _MD = value; }
    }
    public string Accept
    {
        get { return _Accept; }
        set { _Accept = value; }
    }
}
