using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class pager : System.Web.UI.UserControl
{
    public bool showTextBox=false;
    public int tableWidth=0;
    public int pageTotal = 0;
    private Int16 pageSize = 0;
    private string eventRaised = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        tblPager.Visible = true; 
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPager.Visible = true; 
        if(tableWidth>0)tblPager.Width = tableWidth;
        txtGoto.Visible = showTextBox;
    }
    public void registerControl(GridView gv)
    {
        switch (eventRaised){
            case "prev":
                gv.PageIndex--;
                lblPageCurrent.Text = Convert.ToString(gv.PageIndex + 1);
                Session["lywPagerIndex"] = Convert.ToString(gv.PageIndex);
                break;        
            case "next":
                gv.PageIndex++;
                lblPageCurrent.Text = Convert.ToString(gv.PageIndex + 1);
                Session["lywPagerIndex"] = Convert.ToString(gv.PageIndex);
                break;
            case "user":
                try
                {
                    if (isNumeric(txtGoto.Text))
                    {
                        int pg = Convert.ToInt32(txtGoto.Text);
                        if (pg >= 1 && pg <= Convert.ToInt32(lblPageTotal.Text))
                            gv.PageIndex = pg - 1;
                    }
                }
                catch (Exception ex) {
                    lblPagerError.Text = ex.ToString();
                }               
                break;
            case "pagesize":
                gv.PageSize = pageSize;
                Session["PagerPageSize"] = pageSize.ToString();
                break;
            default:
                if (Session["PagerPageSize"] != null)
                {
                    gv.PageSize = Convert.ToInt16(Session["PagerPageSize"]);
                    //DropDownList p = (DropDownList)gv.FindControl("drpPageSize");
                    try { drpPageSize.SelectedValue = Session["PagerPageSize"].ToString(); }
                    catch (Exception e) { }
                }
                break;
        }
        eventRaised = "";
    }
    public void registerData(GridView gv)
    {
        //Set pageCurrent
        lblPageCurrent.Text = Convert.ToString(gv.PageIndex + 1); 
        //Set pageTotal and buttons
        lblPageTotal.Text = Convert.ToString(gv.PageCount);
        lnkPrev.Enabled = lblPageCurrent.Text != "1";
        lnkNext.Enabled = lblPageCurrent.Text != lblPageTotal.Text;
        txtGoto.Text = "";
        //Set contorls visibility
        tblPager.Visible = gv.Visible;
        if (gv.Rows.Count == 0) tblPager.Visible = false;
    }
    protected void lnkPrev_click(object sender, EventArgs e)
    {
        eventRaised = "prev";
    }
    protected void lnkNext_click(object sender, EventArgs e)
    {
        eventRaised = "next";
    }
    protected void txtGoto_textChanged(object sender, EventArgs e)
    {
        eventRaised = "user";
    }
    protected void drpPageSize_selectedIndexChanged(object sender, EventArgs e)
    {
        pageSize=Int16.Parse(drpPageSize.SelectedValue);
        eventRaised = "pagesize";
    }
    protected bool isNumeric(string s)
    {
        bool result=true;
        try
        {
            int x = int.Parse(s);
        }
        catch (Exception ex)
        {
            result = false;
        }
        return result;
    }

    /* Pager sample functions
    Protected SUb Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        gvXXXX.DataBind()
    End Sub 
    Protected Sub gv_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvXXXX.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvXXXX.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvXXXX.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub
    * 
    Control Usage
    <%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
    <eb:Pager id="pager1" runat="server" tableWidth="800" showTextBox="true"></eb:Pager>
    Add the following to the <asp:GridView> control. AllowPaging="true" PageIndex="20" PagerSettings-Visible="false"
    * tableWidth is 100% by default.
    & showTextBox is False by default.
    */
}

