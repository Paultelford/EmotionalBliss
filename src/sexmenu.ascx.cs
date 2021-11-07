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

public partial class sexmenu : System.Web.UI.UserControl
{
    public string menuName;
    protected void Page_Load(object sender, EventArgs e)
    {
        string _menuType = menuName;
        if (Request.QueryString["m"] !=null) _menuType = Request.QueryString["m"];
        SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        SqlCommand oCmd = new SqlCommand("procSiteMenusByCountryCodeSelect", oConn);
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        HyperLink lnk;
        TableRow tRow;
        TableCell tCell;
        string url;
        string menuType;
        oCmd.CommandType = CommandType.StoredProcedure;
        oCmd.Parameters.Add(new SqlParameter("@countryCode", SqlDbType.VarChar, 5));
        oCmd.Parameters.Add(new SqlParameter("@menuType", SqlDbType.VarChar, 200));
        oCmd.Parameters.Add(new SqlParameter("@showInEditor", SqlDbType.VarChar, 1));
        oCmd.Parameters["@countryCode"].Value = Session["EBLanguage"];
        oCmd.Parameters["@menuType"].Value = _menuType;
        oCmd.Parameters["@showInEditor"].Value = "%";
        try
        {
            if (oCmd.Connection.State == 0) oCmd.Connection.Open();
            da = new SqlDataAdapter(oCmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    if ((bool)row["showOnMenu"])
                    {
                        url = (string)row["url"];
                        menuType = (string)row["menuType"];
                        lnk = new HyperLink();
                        tRow = new TableRow();
                        tCell = new TableCell();
                        tCell.Font.Bold = false;
                        //tCell.BorderStyle=
                        lnk.Text = row["name"].ToString();
                        lnk.NavigateUrl = row["url"].ToString();
                        if ((bool)row["static"]) lnk.NavigateUrl = "~/static.aspx?p=" + url.Replace("~/","") + "&m=" + menuType;
                        lnk.ForeColor = System.Drawing.ColorTranslator.FromHtml("gray");
                        lnk.Font.Name = "arial,tahoma";
                        lnk.Font.Size = 10;
                        lnk.Attributes.Add("style", "a:hover{color:red;};");
                        tCell.Controls.Add(lnk);
                        tCell.VerticalAlign = VerticalAlign.Bottom;
                        tRow.Cells.Add(tCell);
                        tblSexologistsMenu.Rows.Add(tRow);
                        //Underline
                        tRow = new TableRow();
                        tRow.Height = 1;
                        tRow.BackColor = System.Drawing.ColorTranslator.FromHtml("lightblue");
                        tCell = new TableCell();
                        tRow.Cells.Add(tCell);
                        tblSexologistsMenu.Rows.Add(tRow);
                    }
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
