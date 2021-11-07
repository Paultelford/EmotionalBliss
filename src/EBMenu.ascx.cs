using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class EBMenu : System.Web.UI.UserControl
{
    public string menuName;
    public string master;
    public int _maxItemsInBottomTable = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        string _menuType = menuName;
        string callingMasterPage = master;
        string pageName = Request.ServerVariables["SCRIPT_NAME"];
        char splitter = '/';
        string[] pageNameArr = pageName.Split(splitter);
        pageName=pageNameArr[pageNameArr.Length-1];
        pageName=Request.ServerVariables["PATH_INFO"];
        if ((string)Request.QueryString.ToString() != "") pageName += "?" + Request.QueryString;
        if (Request.QueryString["m"] != null) _menuType = Request.QueryString["m"];
        SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        SqlCommand oCmd = new SqlCommand("procSiteMenusByCountryCodeMenuSelect", oConn);
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        string topData = "";
        string bottomData = "";
        int rowCount = 0;
        int currentRow = 0;
        string html = "";
        string url;
        string menuType;
        string className = "";
        oCmd.CommandType = CommandType.StoredProcedure;
        oCmd.Parameters.Add(new SqlParameter("@countryCode", SqlDbType.VarChar, 5));
        oCmd.Parameters.Add(new SqlParameter("@menuType", SqlDbType.VarChar, 200));
        oCmd.Parameters["@countryCode"].Value = Session["EBLanguage"];
        oCmd.Parameters["@menuType"].Value = _menuType;
        try
        {
            if (oCmd.Connection.State == 0) oCmd.Connection.Open();
            da = new SqlDataAdapter(oCmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
                rowCount = ds.Tables[0].Rows.Count;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    url = (string)row["url"];
                    className = "white2";
                    menuType = (string)row["menuType"];
                    if ((bool)row["static"])
                    {
                        //If user is looking at static page, then change the className for that page so it stands out bold in the leftMenu
                        if (Request.QueryString["p"] != null)
                            if (url.ToLower().Substring(2) == (string)Request.QueryString["p"].ToLower()) className = "titlem";
                        url = "/static.aspx?p=" + url.Replace("/", "") + "&m=" + menuType;
                        url = url.Replace("~", "");                           
                    }
                    else
                    {
                        url = url.Replace("~", "");
                        if (pageName.ToLower().Substring(1) == url.ToLower().Substring(1)) className = "titlem"; //If this menu item is currently being displayed, then change the className so it shows as bold in the leftMenu
                    }
                    html = "<a href='" + url + "' class='sideNav'>" + (string)row["name"] + "</a><div id='DashedLineHorizontal'></div>";
                    topData += html;
                    if (false)
                    {
                        if (className == "titlem")
                            html = "<span class='" + className + "'>" + (string)row["name"] + "</span>";
                        else
                            html = "<a href='" + url + "' class='" + className + "'>" + (string)row["name"] + "</a>";

                        if ((rowCount - currentRow) - 1 != _maxItemsInBottomTable) html += "<br>";

                        if ((rowCount - currentRow++) <= _maxItemsInBottomTable)
                            bottomData += html;
                        else
                            topData += html;
                    }
                }
                
                litBottomMenu.Text = topData+bottomData;
                switch(callingMasterPage){
                    case "m_site":
                        try
                        {
                            //(Page.Master as ASP.m_site_master).passUpperTableData(topData);
                        }
                        catch (Exception ex)
                        {
                            //Page.Master.passUpperTableData(topData);
                        }

                        
                        break;
                    case "m_shop":
                        //(Page.Master as ASP.m_shop_master).passUpperTableData(topData);
                        break;
                    case "msite":
                        try
                        {
                            //(Page.Master as ASP.msite_master).passUpperTableData(topData);
                        }
                        catch (Exception ex)
                        {
                            //Page.Master.passUpperTableData(topData);
                            siteInclude.debug("msite:passUpperTableData::topData=" + topData.ToString());
                            siteInclude.debug("msite:passUpperTableData::" + ex.ToString());
                        }
                        break;
            }                
        }
        catch (Exception ex)
        {
            lblMenuError.Text = "<font color='red'>Error occured creating menu; " + ex.ToString() + "</font>";
        }
        finally
        {
            ds.Dispose();
            da.Dispose();
            oCmd.Dispose();
            oConn.Dispose();
        }
    }
}
