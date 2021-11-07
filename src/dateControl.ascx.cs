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

public partial class dateControl : System.Web.UI.UserControl
{
    private bool bSingleMonth = true;
    private bool errorFlagged = false;
    private string passStartDate = "StartDate";
    private string passEndDate = "EndDate";
    private string[] months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string pageName = getPageName();
            if (Convert.ToString(Session["EBDateControl_PageName"]) == pageName)
            {
                // Session matches, this DateControl's dates are still in the session. Set them.
                //del calStart.SelectedDate = Convert.ToDateTime(Session["EBDateControl_StartDateDay"] + "/" + Session["EBDateControl_StartDateMonth"] + "/" + Session["EBDateControl_StartDateYear"]);
                //del calEnd.SelectedDate = Convert.ToDateTime(Session["EBDateControl_EndDateDay"] + "/" + Session["EBDateControl_EndDateMonth"] + "/" + Session["EBDateControl_EndDateYear"]);
                txtStartDate.Text = Session["EBDateControl_StartDateDay"] + " " + months[Convert.ToInt16(Session["EBDateControl_StartDateMonth"])-1] + " " + Session["EBDateControl_StartDateYear"];
                txtEndDate.Text= Session["EBDateControl_EndDateDay"] + " " + months[Convert.ToInt16(Session["EBDateControl_EndDateMonth"])-1] + " " + Session["EBDateControl_EndDateYear"];
                //Maintain value from new controls
                //del calStart.VisibleDate = calStart.SelectedDate;
                //del calEnd.VisibleDate = calEnd.SelectedDate;
                //calStart.VisibleDate = calStart.SelectedDate;
                //calEnd.VisibleDate = calEnd.SelectedDate;
            }
            else
            {
                // Session does not match, or is not found. Use defaults.
                //del calStart.SelectedDate = Convert.ToDateTime("01/" + DateTime.Today.Month + "/" + DateTime.Today.Year);
                //del calEnd.SelectedDate = DateTime.Now;
                txtStartDate.Text = "1 " + months[DateTime.Today.Month-1] + " " + DateTime.Today.Year;
                txtEndDate.Text= DateTime.Today.Day + " " + months[DateTime.Today.Month - 1] + " " + DateTime.Today.Year;
                //del txtEndDate.Text = DateTime.Now.ToString();
            }

            lnkMonth.Text = months[DateTime.Now.Month - 1] + " " + DateTime.Now.Year.ToString();
            //if querystring has been set, then use values listed there instead
            //del if (Request.QueryString[passStartDate] != null) calStart.SelectedDate = Convert.ToDateTime(Request.QueryString[passStartDate]);
            //del if (Request.QueryString[passEndDate] != null) calEnd.SelectedDate = Convert.ToDateTime(Request.QueryString[passEndDate]);
            //del lnkStartDate.Text = calStart.SelectedDate.ToLongDateString();
            //del lnkEndDate.Text = calEnd.SelectedDate.ToLongDateString();
            if (Request.QueryString[passStartDate] != null) txtStartDate.Text = Convert.ToDateTime(Request.QueryString[passStartDate]).ToString();
            if (Request.QueryString[passEndDate] != null) txtEndDate.Text = Convert.ToDateTime(Request.QueryString[passEndDate]).ToString();
            //lnkStartDate.Text = calStart.SelectedDate.ToLongDateString();
            //lnkEndDate.Text = calEnd.SelectedDate.ToLongDateString();
        }
    }
    public event System.EventHandler DateChanged;
    protected virtual void onChange(object sender)
    {
        if (this.DateChanged != null)
            this.DateChanged(sender, new EventArgs());
    }
    public virtual bool hasErrorOccured
    {
        get { return errorFlagged; }
    }
    public virtual bool singleMonth
    {
        get { return bSingleMonth; }
        set { bSingleMonth = value; updateVisibility(); }
    }
    public virtual string StartDateQS
    {
        get { return passStartDate; }
        set { passStartDate = value; }
    }
    public virtual string EndDateQS
    {
        get { return passEndDate; }
        set { passEndDate = value; }
    }
    public virtual string StartDateText
    {
        get { return lnkStartDateText.Text; }
        set { lnkStartDateText.Text = value; }
    }
    public virtual string EndDateText
    {
        get { return lnkEndDateText.Text; }
        set { lnkEndDateText.Text = value; }
    }
    /*public virtual string getStartDateQS
    {
        get { return string.Format("{0:d}", calStart.SelectedDate).Replace("/","-"); }
    } */
    /*public virtual string getEndDateQS
    {
        get { return string.Format("{0:d}", calEnd.SelectedDate).Replace("/", "-"); }
    }
    public virtual string getDatesQS
    {
        get { return passStartDate + "=" + getStartDateQS + "&" + passEndDate + "=" + getEndDateQS; }
    }
    public virtual DateTime setStartDate
    {
        set { calStart.SelectedDate = value; lnkStartDate.Text = calStart.SelectedDate.ToLongDateString(); }
    }
    public virtual DateTime setEndDate
    {
        set { calEnd.SelectedDate = value; lnkEndDate.Text = calEnd.SelectedDate.ToLongDateString(); }
    }*/
    public virtual string Month
    {
        get { return lblMonthText.Text; }
        set { lblMonthText.Text = value; }
    }
    public virtual string getStartDate
    {
        //del get { return lnkStartDate.Text + " 0:00"; }
        get { return Convert.ToDateTime(txtStartDate.Text).ToString("dd/MM/yyyy") + " 0:00"; }
    }
    public virtual string getEndDate
    {
        //del get { return lnkEndDate.Text + " 23:59:59"; }
        get { return Convert.ToDateTime(txtEndDate.Text).ToString("dd/MM/yyyy") + " 23:59:59"; }
    }
    public virtual string getMonth
    {
        get { return lnkMonth.Text; }
    }
    protected void lnkStartDate_click(object sender, EventArgs e)
    {
        lnkStartDate.Visible = false;
        calStart.Visible = true;
    }
    protected void lnkEndDate_click(object sender, EventArgs e)
    {    
        lnkEndDate.Visible = false;
        calEnd.Visible = true;
    }
    protected void lnkMonth_click(object sender, EventArgs e)
    {
        calMonth.Visible = true;
    }
    /*protected void calStart_selectionChanged(object sender,EventArgs e)
    {
        //lnkStartDate.Text=calStart.SelectedDate.ToLongDateString();
        lnkStartDate.Visible = true;
        calStart.Visible = false;
        testDates();
        if (!errorFlagged)
            setSessionVariables();
        onChange(sender);
    }
    protected void calEnd_selectionChanged(object sender,EventArgs e)
    {
        //lnkEndDate.Text = calEnd.SelectedDate.ToLongDateString();
        lnkEndDate.Visible = true;
        calEnd.Visible = false;
        testDates();
        if (!errorFlagged)
            setSessionVariables();
        onChange(sender);
    }*/
    protected void txtStartDate_textChanged(object sender, EventArgs e)
    {
        try
        {

            testDates();
            if (!errorFlagged) setSessionVariables();
            onChange(sender);
        }
        catch (Exception ex)
        {
            siteInclude.addError("dateControl.ascx.vb", "txtStartDate_textChanged(); " + ex.ToString());
        }
    }
    protected void txtEndDate_textChanged(object sender, EventArgs e)
    {
        testDates();
        if (!errorFlagged) setSessionVariables();
        onChange(sender);
    }
    protected void calMonth_selectionChanged(object sender, EventArgs e)
    {
        lnkMonth.Text = months[calMonth.SelectedDate.Month - 1] + " " + calMonth.SelectedDate.Year.ToString();
        calMonth.Visible = false;
        onChange(sender);
    }
    protected void testDates()
    {
        /*DateTime sd = calStart.SelectedDate;
        DateTime ed = calEnd.SelectedDate;
        Int64 sdTicks = sd.Ticks;
        Int64 edTicks = ed.Ticks;
        if (sdTicks > edTicks)
        {
            //<!  THESE WERE ALREADY COMMENTED OUT
            //lblError.Text = "<font color='red'>Invalid date range.</font>";
            //errorFlagged=true;
            //Start date is greater than end date, rather than show an error, just knock a year off the start date
            //calStart.SelectedDate = sd.AddYears(-1);
        }
        else
        {
            lblError.Text = "";
            errorFlagged=false;
        }*/
    }
    private void updateVisibility()
    {
        if (bSingleMonth)
        {
            tblRowStartDate.Visible = false;
            tblRowEndDate.Visible = false;
            tblRowMonth.Visible = true;
        }
        else
        {
            tblRowStartDate.Visible = true;
            tblRowEndDate.Visible = true;
            tblRowMonth.Visible = false;
        }
    }
    private string getPageName()
    {
        string page = Request.ServerVariables["PATH_TRANSLATED"];
        string[] arr = page.Split('?');
        //return arr[0];
        return "static";
    }
    private void setSessionVariables()
    {
        /*
        Session["EBDateControl_PageName"] = getPageName();
        Session["EBDateControl_StartDateDay"] = calStart.SelectedDate.Day;
        Session["EBDateControl_StartDateMonth"] = calStart.SelectedDate.Month;
        Session["EBDateControl_StartDateYear"] = calStart.SelectedDate.Year;
        Session["EBDateControl_EndDateDay"] = calEnd.SelectedDate.Day;
        Session["EBDateControl_EndDateMonth"] = calEnd.SelectedDate.Month;
        Session["EBDateControl_EndDateYear"] = calEnd.SelectedDate.Year;
         * */
        Session["EBDateControl_PageName"] = getPageName();
        Session["EBDateControl_StartDateDay"] = Convert.ToDateTime(txtStartDate.Text).Day;
        Session["EBDateControl_StartDateMonth"] = Convert.ToDateTime(txtStartDate.Text).Month;
        Session["EBDateControl_StartDateYear"] = Convert.ToDateTime(txtStartDate.Text).Year;
        Session["EBDateControl_EndDateDay"] = Convert.ToDateTime(txtEndDate.Text).Day;
        Session["EBDateControl_EndDateMonth"] = Convert.ToDateTime(txtEndDate.Text).Month;
        Session["EBDateControl_EndDateYear"] = Convert.ToDateTime(txtEndDate.Text).Year;
    }
    protected void lnkYearPrevNav_click(object sender, EventArgs e)
    {
        /*
        Calendar cal;
        if (hidSelected.Value == "end")
            cal = calEnd;
        else
            cal = calStart;
        cal.SelectedDate = Convert.ToDateTime(cal.SelectedDate.Day + "/" + cal.SelectedDate.Month + "/" + Convert.ToString(Convert.ToInt16(cal.SelectedDate.Year)-1));
        testDates();
        if (!errorFlagged)
        {
            setSessionVariables();
            cal.VisibleDate = cal.SelectedDate;
            if (hidSelected.Value == "end")
                calEnd_selectionChanged(cal, null);
            else
                calStart_selectionChanged(cal, null);
        }
         */
    }
    protected void lnkYearNextNav_click(object sender, EventArgs e)
    {
        /*
        Calendar cal;
        if (hidSelected.Value == "end")
            cal = calEnd;
        else
            cal = calStart;
        cal.SelectedDate = Convert.ToDateTime(cal.SelectedDate.Day + "/" + cal.SelectedDate.Month + "/" + Convert.ToString(Convert.ToInt16(cal.SelectedDate.Year) + 1));
        testDates();
        if (!errorFlagged)
        {
            setSessionVariables();
            cal.VisibleDate = cal.SelectedDate;
            if (hidSelected.Value == "end")
                calEnd_selectionChanged(cal, null);
            else
                calStart_selectionChanged(cal, null);
        }
         */
    }
    protected void lnkMonthNav_click(object sender, EventArgs e)
    {
        /*
        LinkButton lnk = (LinkButton)sender;
        string month = lnk.CommandArgument;
        Calendar cal;
        if (hidSelected.Value == "end")
            cal = calEnd;
        else
            cal = calStart;
        cal.SelectedDate = Convert.ToDateTime(cal.SelectedDate.Day + "/" + month + "/" + Convert.ToString(cal.SelectedDate.Year));
        testDates();
        if (!errorFlagged)
        {
            setSessionVariables();
            cal.VisibleDate = cal.SelectedDate;
            if (hidSelected.Value == "end")
                calEnd_selectionChanged(cal, null);
            else
                calStart_selectionChanged(cal, null);
        }
         */
    }
    protected void lnkStartDateText_click(object sender, EventArgs e)
    {
        if (hidSelected.Value=="end")
        {
            //End date is already selected, therefore controls are already visible
            lnkEndDateText.Text = "End Date:";
            lnkStartDateText.Text = "<font color='red'>Start Date:</font>";
            hidSelected.Value = "start";
        }
        else
        {
            if (lnkStartDateText.Text == "Start Date:")
            {
                //Start date was not clicked either. Hilight it and show controls
                lnkStartDateText.Text = "<font color='red'>Start Date:</font>";
                tblControls.Visible = true;
                hidSelected.Value = "start";
            }
            else
            {
                //Start date was selected. De-select it and hide controls
                lnkStartDateText.Text = "Start Date:";
                tblControls.Visible = false;
                hidSelected.Value = "";
            }
        }
    }
    protected void lnkEndDateText_click(object sender, EventArgs e)
    {
        if (hidSelected.Value=="start")
        {
            //Start date is already selected, therefore controls are already visible
            lnkStartDateText.Text = "Start Date:";
            lnkEndDateText.Text = "<font color='red'>End Date:</font>";
            hidSelected.Value = "end";
        }
        else
        {
            if (lnkEndDateText.Text == "End Date:")
            {
                //Enddate was not clicked either. Hilight it and show controls
                lnkEndDateText.Text = "<font color='red'>End Date:</font>";
                tblControls.Visible = true;
                hidSelected.Value = "end";
            }
            else
            {
                //End date was selected. De-select it and hide controls
                lnkEndDateText.Text = "End Date:";
                tblControls.Visible = false;
                hidSelected.Value = "";
            }
        }
    }
}
