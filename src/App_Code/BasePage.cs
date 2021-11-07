using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;


/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : Page
{
    private const string m_DefaultCulture = "en";
    protected string m_metaDescription = "";
    protected string m_metaKeywords = "";
    protected override void InitializeCulture()
    {
        string culture = Convert.ToString(Session["EBLanguage"]);
        if (Request.QueryString["lang"]!=null)
        {
            culture = Request.QueryString["lang"];
        }
        if (!string.IsNullOrEmpty(culture))
        {
            if (culture == "ie") culture = "en";
            if (culture == "gb") culture = "en";
            if (culture == "us") culture = "en";
            if (culture == "se") culture = "sv";
            if (culture == "au")
            {
                culture = "en";
                Session["EBLanguage"] = "gb";
            }
            //Culture = culture;
            if (!string.IsNullOrEmpty(Request.QueryString["lang"]))
            {
                culture = Request.QueryString["lang"];
                if (culture == "ie") culture = "en";
                if (culture == "gb") culture = "en";
                if (culture == "us") culture = "en";
                if (culture == "se") culture = "sv";
                if (culture == "au")
                {
                    culture = "en";
                    Session["EBLanguage"] = "gb";
                }
                Session["EBLanguage"] = Request.QueryString["lang"];
            }
        }
        else
        {
            culture = m_DefaultCulture;
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        base.InitializeCulture();
    }
	public BasePage()
	{
        m_metaDescription = "Buy Emotional Bliss vibrators and intimate massagers here including the Womolia, Jasmine, Femblossom, Chandra and Isis. Unleash your inner erotic spirit!";
        m_metaKeywords = "Emotional Bliss, vibrators,intimate massagers, Womolia, Jasmine, Femblossom, Chandra, Isis, Julia Cole";
	}
    public string getDBResouceString(string name)
    {
        //Needed to support the old mis-spelling for the old site
        return getDBResourceString(name);
    }
    public string getDBResourceString(string name)
    {
        string pageName = "0";
        if (Request.QueryString["p"] != null)
            pageName = Request.QueryString["p"];
        else
            pageName = Page.GetType().Name;
        return getDBResourceString(name, pageName);
    }
    public string getDBResourceString(string name,string page)
    {
        string language=(string)Session["EBLanguage"];
        string result = "";
        SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        SqlCommand oCmd = new SqlCommand("procDBResourcesByNameSelect", oConn);
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        oCmd.CommandType = CommandType.StoredProcedure;
        oCmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar,500));
        oCmd.Parameters.Add(new SqlParameter("@countryCode", SqlDbType.VarChar,5));
        oCmd.Parameters.Add(new SqlParameter("@pagename", SqlDbType.VarChar, 500));
        oCmd.Parameters["@name"].Value = name;
        oCmd.Parameters["@countryCode"].Value = language;
        oCmd.Parameters["@pagename"].Value = page;
        try
        {
            if(oCmd.Connection.State == 0) oCmd.Connection.Open();
            da = new SqlDataAdapter(oCmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if(ds.Tables[0].Rows[0]["type"].ToString().ToLower()=="text")
                    result = ds.Tables[0].Rows[0]["value"].ToString().Replace("<p>", "").Replace("</p>", "");
                else
                    result = ds.Tables[0].Rows[0]["value"].ToString();
                if (result == "")
                {
                    //MetaData entry found in DB, but its Empty. Use default data
                    if (name.ToLower() == "metadescription") result = getMetaResourceString("metaDescription", (string)Session["EBLanguage"]);
                    if (name.ToLower() == "metakeywords") result = getMetaResourceString("metaKeywords", (string)Session["EBLanguage"]);
                }
            }else{
                //No MetaData found, use default
                if (name.ToLower() == "metadescription") result = getMetaResourceString("metaDescription", (string)Session["EBLanguage"]);
                if (name.ToLower() == "metakeywords") result = getMetaResourceString("metaKeywords", (string)Session["EBLanguage"]);
            }
        }
        catch (Exception ex)
        {
            //Not much we can do about it really, return the error message for now
            result = ex.ToString();
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
    private string getMetaResourceString(string name, string language)
    {
        SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        SqlCommand oCmd = new SqlCommand("procDBResourcesByNameSelect", oConn);
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        string result="";
        oCmd.CommandType = CommandType.StoredProcedure;
        oCmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar,500));
        oCmd.Parameters.Add(new SqlParameter("@countryCode", SqlDbType.VarChar,5));
        oCmd.Parameters.Add(new SqlParameter("@pagename", SqlDbType.VarChar, 500));
        oCmd.Parameters["@name"].Value = name;
        oCmd.Parameters["@countryCode"].Value = language;
        oCmd.Parameters["@pagename"].Value = "defaultValue";
        try
        {
            if (oCmd.Connection.State == 0) oCmd.Connection.Open();
            da = new SqlDataAdapter(oCmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0) 
            {
                result = (string)ds.Tables[0].Rows[0]["value"];
                if (result == "")
                {
                    //MetaData entry found in DB, but its Empty. Use default english text
                    if (name.ToLower() == "metadescription") result = m_metaDescription;
                    if (name.ToLower() == "metakeywords") result = m_metaKeywords;
                }
            }else{
                //No MetaData found, use default english text
                if (name.ToLower() == "metadescription") result = m_metaDescription;
                if (name.ToLower() == "metakeywords") result = m_metaKeywords;
            }
        }
        catch (Exception ex)
        {
            //Not much we can do about it really, return the error message for now
            result = ex.ToString();
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
    private string getLower(object o)
    {
        return o.ToString();
    }
 }
