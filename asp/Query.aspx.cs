using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Text;

public partial class asp_Query : System.Web.UI.Page
{
    string sqlConnection = ConfigurationManager.ConnectionStrings["Pomps_ReportsConnectionString"].ConnectionString;
    string cs = ConfigurationManager.ConnectionStrings["SQLMaddenCoConnectionString"].ConnectionString;
    string dataTable, textFields, valueFields, criteriaValue, columnField, columnValue, endYear, endMonth;
    StringBuilder QueryCmd = new StringBuilder();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            clearScreen();
            string username = Request.QueryString.Get("Username").ToString();
            lblUserID.Text = GetUserID(username);
            ddDataTable.DataBind();
        }
        this.RegisterPostBackControl();
    }
    protected void ddTableFields_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddStartYear.SelectedValue == "0" || ddStartMonth.SelectedValue == "0")
        {
            lblError.ForeColor = Color.Red;
            lblError.Text = "Please enter a Year and Month range";
        }
        else
        {
            if (ddEndYear.SelectedValue == "0") { endYear = ddStartYear.SelectedValue; } else { endYear = ddEndYear.SelectedValue; }
            if (ddEndMonth.SelectedValue == "0") { endMonth = ddStartMonth.SelectedValue; } else { endMonth = ddEndMonth.SelectedValue; }
            FieldData(ddDataTable.SelectedValue, ddTableFields.SelectedValue, endYear, endMonth);

            //ReadFieldData(ddTableFields.SelectedValue);
        }
    }
    
    protected void btnContinue_Click(object sender, EventArgs e)
    {
        switch (lblInstructions.Text)
        {
            case ("Select Text Fields"):
                if (ddDataTable.SelectedValue == "")
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Data table and Text fields must be selected before selecting value fields";
                }
                else
                {
                    if (lbSelectedTableFields.Items.Count > 0)
                    {
                        lblError.Text = "";
                        lnkBtnTextFields.Enabled = false;
                        textFields = SelectedTextFields(lbSelectedTableFields);
                        QueryCmd.Append("Select " + textFields + ",");
                        queryCmd.Text = QueryCmd.ToString();
                        lbDataTableFields.Rows = 1;
                        lbSelectedTableFields.Rows = 1;
                        ddTableFields.Visible = false;
                        lbDataTableFields.Visible = false;
                        lbSelectedTableFields.Visible = false;
                        imgBtnDownArrow.Visible = false;
                        imgBtnLeftArrow.Visible = false;
                        imgBtnRightArrow.Visible = false;
                        imgBtnUpArrow.Visible = false;
                        btnAddPar.Visible = false;
                        btnContinue.Visible = false;
                        tblFieldSelection.Visible = false;
                    }
                    else
                    {
                        lblError.ForeColor = Color.Red;
                        lblError.Text = "Select at least one Text Field Row";
                    }
                }
                break;
            case ("Select Value Fields"):
                if (ddDataTable.SelectedValue == "" || lbSelectedGroupFields.Items.Count < 1)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Data table and Text fields must be selected before selecting parameters criteria.";
                }
                else
                {
                    if (Label5.Text == "Pivot Query" && lbSelectedTableFields.Items.Count > 1)
                    {
                        lblError.ForeColor = Color.Red;
                        lblError.Text = "For a Pivot Query you may select only one value field";
                        lblInstructions.Text = "Select Value Fields";
                        lbDataTableFields.Items.Clear();
                        lbSelectedTableFields.Items.Clear();
                        DisplayFields(ddDataTable.SelectedValue, "Numeric", "Numeric");
                        tblFieldSelection.Visible = true;
                        lbDataTableFields.Rows = 10;
                        lbSelectedTableFields.Rows = 10;
                        lbDataTableFields.Visible = true;
                        lbSelectedTableFields.Visible = true;
                        imgBtnDownArrow.Visible = true;
                        imgBtnLeftArrow.Visible = true;
                        imgBtnRightArrow.Visible = true;
                        imgBtnUpArrow.Visible = true;
                        btnContinue.Visible = true;

                    }
                    else
                    {
                        btnAddPar.Visible = false;
                        btnContinue.Visible = false;
                        lnkBtnValueFields.Enabled = false;
                        lblError.Text = "";
                        valueFields = SelectedValueFields(lbSelectedTableFields);
                        QueryCmd.Append(queryCmd.Text + valueFields + " from " + ddDataTable.SelectedValue);
                        queryCmd.Text = QueryCmd.ToString();
                        lbDataTableFields.Rows = 1;
                        lbSelectedTableFields.Rows = 1;
                        ddTableFields.Visible = false;
                        lbDataTableFields.Visible = false;
                        lbSelectedTableFields.Visible = false;
                        imgBtnDownArrow.Visible = false;
                        imgBtnLeftArrow.Visible = false;
                        imgBtnRightArrow.Visible = false;
                        imgBtnUpArrow.Visible = false;
                        tblFieldSelection.Visible = false;
                    }

                }
                break;
            case ("Criteria Selection"):
                btnAddPar.Visible = false;
                btnContinue.Visible = false;
                ddTableFields.Visible = false;
                lblError.Text = "";
                lbDataTableFields.Rows = 1;
                lbSelectedTableFields.Rows = 1;
                lbDataTableFields.Visible = false;
                lbSelectedTableFields.Visible = false;
                lnkBtnParameters.Enabled = false;
                imgBtnDownArrow.Visible = false;
                imgBtnLeftArrow.Visible = false;
                imgBtnRightArrow.Visible = false;
                imgBtnUpArrow.Visible = false;
                tblFieldSelection.Visible = false;
                break;
            default:
                break;
        }
        tblNewQuery.Enabled = true;
        tblSelectQuery.Enabled = true;
    }
    protected void btnQuerySelection_Click(object sender, EventArgs e)
    {
        if (Label1.Text == "List Query" && Label2.Text == "Pivot Query")
        {
            if (RadioButtonListQuery.Checked == true && RadioButtonPivotQuery.Checked == true)
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "You must select only one option";
                RadioButtonListQuery.Checked = false;
                RadioButtonPivotQuery.Checked = false;
            }
            else if (RadioButtonListQuery.Checked == true && RadioButtonPivotQuery.Checked == true) lblError.Text = "Please select your Query Option below";
            else
            {
                lblInstructions.Text = "Enter your query information above, all fields must be selected.";
                if (RadioButtonListQuery.Checked == true && RadioButtonPivotQuery.Checked == false)
                {
                    Label5.Text = "List Query";
                    lblError.Text = "";
                    Label3.Text = "Sort By";
                    Label4.Text = "";
                    ddColumnValue.Items.Add("");
                    ddColumnValue.Items.Add("DESC");
                    ddMyQueries.Width = 1;
                    ddMyQueries.Visible = false;
                    txtQueryName.Width = 350;
                    txtQueryName.Visible = true;
                    tblNewQuery.Visible = true;
                    tblSelectQuery.Visible = true;
                    tblSelectQueryOption.Visible = false;
                    RadioButtonNewQuery.Checked = false;
                    RadioButtonSavedQuery.Checked = false;
                    btnCmd.Visible = true;
                    btnProcess.Visible = true;
                }
                else if (RadioButtonListQuery.Checked == false && RadioButtonPivotQuery.Checked == true)
                {
                    Label5.Text = "Pivot Query";
                    lblError.Text = "";
                    Label3.Text = "Column Field";
                    Label4.Text = "Column Value";
                    ddMyQueries.Width = 1;
                    ddMyQueries.Visible = false;
                    txtQueryName.Width = 350;
                    txtQueryName.Visible = true;
                    tblNewQuery.Visible = true;
                    tblSelectQuery.Visible = true;
                    tblSelectQueryOption.Visible = false;
                    RadioButtonNewQuery.Checked = false;
                    RadioButtonSavedQuery.Checked = false;
                    btnCmd.Visible = true;
                    btnProcess.Visible = true;
                }

            }
        }
        else
        {
            if (RadioButtonNewQuery.Checked == true && RadioButtonSavedQuery.Checked == true)
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "You must select only one option";
                RadioButtonNewQuery.Checked = false;
                RadioButtonSavedQuery.Checked = false;
            }
            else if (RadioButtonNewQuery.Checked == false && RadioButtonSavedQuery.Checked == false)
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "Please select your Query Option below";
            }
            else
            {
                if (RadioButtonNewQuery.Checked == true)
                {
                    Label1.Text = "List Query";
                    Label2.Text = "Pivot Query";
                    Label5.Text = "Select a Query Type";
                    lblError.Text = "";
                    lblInstructions.Text = "";
                    ddMyQueries.Visible = false; ;
                    txtQueryName.Visible = false;
                    RadioButtonSavedQuery.Visible = false;
                    RadioButtonListQuery.Visible = true;
                    RadioButtonNewQuery.Visible = false;
                    RadioButtonPivotQuery.Visible = true;

                }
                if (RadioButtonSavedQuery.Checked == true)
                {
                    Label5.Text = "";
                    lblInstructions.Text = "Enter your information above and press Next => to run the query";
                    lblError.Text = "";
                    txtQueryName.Visible = false;
                    ddMyQueries.DataBind();
                    ddMyQueries.Visible = true;
                    tblNewQuery.Visible = true;
                    tblNewQuery.Enabled = false;
                    tblSelectQuery.Visible = true;
                    tblSelectQueryOption.Visible = false;
                    RadioButtonNewQuery.Checked = false;
                    RadioButtonSavedQuery.Checked = false;
                    btnCmd.Visible = true;
                    btnProcess.Visible = true;
                }
            }
        }
    }

    protected void btnCmd_Click(object sender, EventArgs e)
    {
        if (ddEndYear.SelectedValue == "0") endYear = ddStartYear.SelectedValue;
        if (ddEndMonth.SelectedValue == "0") endMonth = ddStartMonth.SelectedValue;
        switch (lblInstructions.Text)
        {
            case ("Criteria Selection"):
                if (Label5.Text == "Pivot Query" && (ddColumnField.SelectedValue == "" || ddColumnValue.SelectedValue == ""))
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "A Pivot Query requires one column field and one column value field";
                }
                else
                {
                    lnkBtnParameters.Enabled = false;
                    if (lblParameterValues.Text != "")
                    {
                        string par = TableParameters(ddDataTable.SelectedValue);
                        QueryCmd.Append(queryCmd.Text + " where " + lblParameterValues.Text);
                        if (par != "")
                        {
                            //queryCmd.Text = QueryCmd.ToString() + " and " + par;
                            QueryCmd.Append(" and " + par + " group by " + lblTextFields.Text);
                            queryCmd.Text = QueryCmd.ToString();
                        }
                        else
                        {
                            QueryCmd.Append(" group by " + lblTextFields.Text);
                            queryCmd.Text = QueryCmd.ToString();
                        }
                        if (Label5.Text == "Pivot Query")
                        {
                            string filters = lblParameterValues.Text + " and " + par;
                            string columnValueData = ColumnValueData(ddColumnField.SelectedValue, ddDataTable.SelectedValue,filters, ddStartYear.SelectedValue, endYear, ddStartMonth.SelectedValue, endYear);
                            queryCmd.Text = "Select * from (" + QueryCmd.ToString() + ") s Pivot(sum(" + ddColumnValue.SelectedValue + ") for " + ddColumnField.SelectedValue + " in(" + columnValueData + ")) pvt";
                        }
                        else
                        {
                            if (ddColumnField.SelectedValue == "")
                            {
                                queryCmd.Text = QueryCmd.ToString();
                            }
                            else
                            {
                                QueryCmd.Append(" order by " + ddColumnField.SelectedValue + " " + ddColumnValue.SelectedValue);
                                queryCmd.Text = QueryCmd.ToString();
                            }
                        }
                        lblError.Text = "";
                        ddTableFields.Visible = false;
                        lbDataTableFields.Rows = 1;
                        lbSelectedTableFields.Rows = 1;
                        lbDataTableFields.Visible = false;
                        lbSelectedTableFields.Visible = false;
                        imgBtnDownArrow.Visible = false;
                        imgBtnLeftArrow.Visible = false;
                        imgBtnRightArrow.Visible = false;
                        imgBtnUpArrow.Visible = false;
                        btnAddPar.Visible = false;
                        btnCmd.Text = "Process";
                        btnCmd.Visible = true;
                        btnProcess.Text = "Reset";
                        btnProcess.Visible = true;
                        txtQueryName.Visible = true;
                        tblNewQuery.Enabled = false;
                        tblSelectQuery.Enabled = false;
                        lblInstructions.Text = "Click Process to run Query";
                    }
                    else
                    {
                        lblError.Text = "You must enter at least one selection criteria";
                        lblInstructions.Text = "Criteria Selection";
                    }
                }
                break;
            case ("Click Process to run Query"):
                if (txtQueryName.Text != String.Empty || txtQueryName.Text != " ")
                {
                    lblInstructions.Text = "Save your Query?";
                    lblError.Text = "";
                    btnCmd.Text = "Save Query";
                    imgExcel.Visible = true;
                    if (ddEndYear.SelectedValue == "0") { endYear = ddStartYear.SelectedValue; } else { endYear = ddEndYear.SelectedValue; }
                    if (ddEndMonth.SelectedValue == "0") { endMonth = ddStartMonth.SelectedValue; } else { endMonth = ddEndMonth.SelectedValue; }
                    FillGridview(queryCmd.Text, lblUserID.Text, endYear, endMonth);
                }
                else
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Please enter a name for your Query";
                }
                break;
            case ("Enter your information above and press Next => to run the query"):

                if (ddStartMonth.SelectedValue.ToString() == "0" || ddStartYear.SelectedValue.ToString() == "0")
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Year and Month range must be selected";
                }
                else
                {
                    lblError.Text = "";
                    btnCmd.Enabled = false;
                    imgExcel.Visible = true;
                    lblInstructions.Text = ddMyQueries.SelectedItem.Text + " Year " + ddStartYear.SelectedItem.Text;
                    if (ddEndYear.SelectedValue != "0") lblInstructions.Text = lblInstructions.Text + " To " + endYear;
                    lblInstructions.Text = lblInstructions.Text + " Month " + ddStartMonth.SelectedItem.Text;
                    if (ddEndMonth.SelectedValue != "0") lblInstructions.Text = lblInstructions.Text + " To " + ddEndMonth.SelectedItem.Text;
                    if (ddEndYear.SelectedValue == "0") { endYear = ddStartYear.SelectedValue; } else { endYear = ddEndYear.SelectedValue; }
                    if (ddEndMonth.SelectedValue == "0") { endMonth = ddStartMonth.SelectedValue; } else { endMonth = ddEndMonth.SelectedValue; }
                    FillGridview(ddMyQueries.SelectedValue, lblUserID.Text, endYear, endMonth);
                    //DisposeControls();
                }
                break;
            case ("Save your Query?"):
                GridView1.Visible = false;
                imgExcel.Visible = false;
                SaveQuery(txtQueryName.Text, queryCmd.Text);
                lblInstructions.Text = "Your Query has been saved";
                btnCmd.Text = "Continue";
                btnProcess.Visible = false;
                break;
            case ("Your Query has been saved"):
                //DisposeControls();
                clearScreen();
                break;
        }
    }
    protected void btnAddPar_Click(object sender, EventArgs e)
    {
        if (lbSelectedTableFields.Items.Count > 0)
        {
            lblError.Text = "";
            string parameterValue = ddTableFields.SelectedValue + " in " + SelectedParameterValue(lbSelectedTableFields);
            if (lblParameterValues.Text == String.Empty)
            {
                lblParameterValues.Text = parameterValue;
            }
            else
            {
                lblParameterValues.Text = lblParameterValues.Text + " and " + parameterValue;
            }
            lbDataTableFields.Items.Clear();
            lbSelectedTableFields.Items.Clear();
            btnCmd.Visible = true;
        }
        else
        {
            lblError.ForeColor = Color.Red;
            lblError.Text = "You must enter at least one selection criteria";
            lblInstructions.Text = "Criteria Selection";
        }
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        //DisposeControls();
        clearScreen();
    }
    protected void imgBtnUpArrow_Click(object sender, ImageClickEventArgs e)
    {
        MoveItem(-1);
    }

    protected void imgBtnRightArrow_Click(object sender, ImageClickEventArgs e)
    {
        if (lbDataTableFields.SelectedIndex == -1)
        {
            lblInstructions.Text = "Please select an item";
        }
        else
        {
            while (lbDataTableFields.SelectedIndex >= 0)
            {
                if (lbDataTableFields.SelectedItem.Selected)
                {
                    lbSelectedTableFields.Items.Add(lbDataTableFields.SelectedItem);
                    lbSelectedTableFields.DataValueField = lbDataTableFields.DataValueField;
                    lbDataTableFields.Items.RemoveAt(lbDataTableFields.SelectedIndex);
                }
            }

        }
    }

    protected void imgBtnLeftArrow_Click(object sender, ImageClickEventArgs e)
    {
        /*if (lbSelectedTableFields.SelectedIndex == 1)
        {
            lblInstructions.Text = "Please select an item";
        }
        else
        {*/
        while (lbSelectedTableFields.SelectedIndex >= 0)
        {
            if (lbSelectedTableFields.SelectedItem.Selected)
            {
                lbDataTableFields.Items.Add(lbSelectedTableFields.SelectedItem);
                lbDataTableFields.DataValueField = lbSelectedTableFields.DataValueField;
                lbSelectedTableFields.Items.RemoveAt(lbSelectedTableFields.SelectedIndex);
            }
        }
        //}
    }

    protected void imgBtnDownArrow_Click(object sender, ImageClickEventArgs e)
    {
        MoveItem(1);
    }


    protected void imgExcel_Click(object sender, ImageClickEventArgs e)
    {
        string queryName = txtQueryName.Text + " Year " + ddStartYear.SelectedValue.ToString();
        if (ddEndYear.SelectedValue != "0") queryName = queryName + " To " + ddEndYear.SelectedItem.Text;
        queryName = queryName + " Month " + ddStartMonth.SelectedItem.Text;
        if (ddEndMonth.SelectedValue != "0") queryName = queryName + " To " + ddEndMonth.SelectedItem.Text;
        ExportToExcel(queryName);
    }
    protected void lnkBtnTextFields_Click(object sender, EventArgs e)
    {
        if (ddDataTable.SelectedValue == "")
        {
            lblError.ForeColor = Color.Red;
            lblError.Text = "Data table must be selected before selecting text fields";
            ddDataTable.Enabled = true;
        }
        else
        {
            if (ddStartYear.SelectedValue == "0" && ddStartMonth.SelectedValue == "0")
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "Please enter Year and Month Range";
            }
            else
            {
                if (txtQueryName.Text == String.Empty)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Please enter a Name for your new Query";
                }
                else
                {
                    tblNewQuery.Enabled = false;
                    tblSelectQuery.Enabled = false;
                    dataTable = ddDataTable.SelectedValue;
                    queryCmd.Text = "select * from " + ddDataTable.SelectedValue;
                    lblInstructions.Text = "Select Text Fields";
                    lblError.Text = "";
                    lbDataTableFields.Rows = 10;
                    lbDataTableFields.Visible = true;
                    lbSelectedTableFields.Items.Clear();
                    lbSelectedTableFields.Rows = 10;
                    lbSelectedTableFields.Visible = true;
                    DisplayFields(dataTable, "Text", "Text");
                    tblFieldSelection.Visible = true;
                    imgBtnDownArrow.Visible = true;
                    imgBtnLeftArrow.Visible = true;
                    imgBtnRightArrow.Visible = true;
                    imgBtnUpArrow.Visible = true;
                    btnContinue.Visible = true;
                }
            }
        }

    }
    protected void lnkBtnValueFields_Click(object sender, EventArgs e)
    {
        if (ddDataTable.SelectedValue == "")
        {
            lblError.ForeColor = Color.Red;
            lblError.Text = "Data table and Text fields must be selected before selecting value fields";
        }
        else
        {
            if (lbSelectedTableFields.Items.Count > 0)
            {
                tblNewQuery.Enabled = false;
                tblSelectQuery.Enabled = false;
                lblInstructions.Text = "Select Value Fields";
                lblError.Text = "";
                lbDataTableFields.Items.Clear();
                lbDataTableFields.Rows = 10;
                lbDataTableFields.Visible = true;
                lbSelectedTableFields.Items.Clear();
                lbSelectedTableFields.Rows = 10;
                lbSelectedTableFields.Visible = true;
                DisplayFields(ddDataTable.SelectedValue, "Numeric", "Numeric");
                tblFieldSelection.Visible = true;
                imgBtnDownArrow.Visible = true;
                imgBtnLeftArrow.Visible = true;
                imgBtnRightArrow.Visible = true;
                imgBtnUpArrow.Visible = true;
                btnContinue.Visible = true;
            }
            else
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "Select at least one Text Field Row";
            }
        }
    }

    protected void lnkBtnParameters_Click(object sender, EventArgs e)
    {
        if (ddDataTable.SelectedValue == "" || lbSelectedGroupFields.Items.Count < 1)
        {
            lblError.ForeColor = Color.Red;
            lblError.Text = "Data table and Text fields must be selected before selecting parameters criteria.";
        }
        else
        {
            if (Label5.Text == "Pivot Query" && lbSelectedTableFields.Items.Count > 1)
            {
                tblNewQuery.Enabled = false;
                tblSelectQuery.Enabled = false;
                lblError.ForeColor = Color.Red;
                lblError.Text = "For a Pivot Query you may select only one value field";
                lblInstructions.Text = "Select Value Fields";
                lbDataTableFields.Rows = 10;
                lbDataTableFields.Visible = true;
                lbSelectedTableFields.Items.Clear();
                lbSelectedTableFields.Rows = 10;
                lbSelectedTableFields.Visible = true;
                DisplayFields(ddDataTable.SelectedValue, "Numeric", "Numeric");
                tblFieldSelection.Visible = true;
                imgBtnDownArrow.Visible = true;
                imgBtnLeftArrow.Visible = true;
                imgBtnRightArrow.Visible = true;
                imgBtnUpArrow.Visible = true;
                btnContinue.Visible = true;

            }
            else
            {
                btnAddPar.Visible = true;
                btnContinue.Visible = true;
                ddTableFields.Visible = true;
                imgBtnDownArrow.Visible = true;
                imgBtnLeftArrow.Visible = true;
                imgBtnRightArrow.Visible = true;
                imgBtnUpArrow.Visible = true;
                lbSelectedTableFields.Items.Clear();
                lbSelectedTableFields.Rows = 10;
                lbSelectedTableFields.Visible = true;
                lbDataTableFields.Items.Clear();
                lbDataTableFields.Rows = 10;
                lbDataTableFields.Visible = true;
                lblInstructions.Text = "Criteria Selection";
                tblNewQuery.Enabled = false;
                tblSelectQuery.Enabled = false;
                tblFieldSelection.Visible = true;

                DisplayFields(ddDataTable.SelectedValue, "Text", "Parameter");
            }

        }
    }
    private void ExportToExcel(string queryName)
    {
        try
        {
            Response.ClearContent();
            Response.ContentType = "application/excel";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("content-disposition", "attachment; filename=" + queryName + ".xls");

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");

            foreach (TableCell tableCell in GridView1.HeaderRow.Cells)
            {
                tableCell.Style["background-color"] = "seagreen";
            }

            foreach (GridViewRow gridViewRow in GridView1.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "#E3EAEB";
                }
            }

            foreach (GridViewRow gridViewRow in GridView1.Rows)
            {
                gridViewRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
                {
                    gridViewRowTableCell.Style["background-color"] = "white";
                }
            }

            Response.Write("");
            Response.Write(queryName + " " + lblParameterValues.Text);
            GridView1.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    private void FillGridview(string myQuery, string userID, string endYear, string endMonth)
    {
        SqlConnection cn = new SqlConnection(sqlConnection);
        cn.Open();
        try
        {
            using (SqlDataAdapter c = new SqlDataAdapter(myQuery, cn))
            {
                var gridviewTable = new DataTable();
                c.SelectCommand.Parameters.AddWithValue("@UserID", userID);
                c.SelectCommand.Parameters.AddWithValue("@StartYear", ddStartYear.SelectedValue);
                c.SelectCommand.Parameters.AddWithValue("@EndYear", endYear);
                c.SelectCommand.Parameters.AddWithValue("@StartMonth", ddStartMonth.SelectedValue);
                c.SelectCommand.Parameters.AddWithValue("@EndMonth", endMonth);
                c.SelectCommand.CommandTimeout = 0;
                c.Fill(gridviewTable);
                GridView1.DataSource = gridviewTable;
                GridView1.DataBind();
                cn.Close();
                //ExportToExcel();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private string ColumnValueData(string columnData, string dataTable, string parameters, string startYear, string endYear, string startMonth, string endMonth)
    {
        string columnValueData;
        string myQuery = @"select top 500 " + columnData + " as columnData from " + dataTable + " where " + parameters + " group by " + columnData + " order by " + columnData;
        using (SqlConnection cn = new SqlConnection(sqlConnection))
            try
            {
                cn.Open();
                SqlCommand execMyQuery = new SqlCommand(myQuery, cn);
                execMyQuery.Parameters.AddWithValue("@UserID", lblUserID.Text);
                execMyQuery.Parameters.AddWithValue("@StartYear", startYear);
                execMyQuery.Parameters.AddWithValue("@EndYear", endYear);
                execMyQuery.Parameters.AddWithValue("@StartMonth", startMonth);
                execMyQuery.Parameters.AddWithValue("@EndMonth", endMonth);
                using (SqlDataReader read = execMyQuery.ExecuteReader())
                {
                    var myList = new List<string>();
                    while (read.Read())
                    {
                        myList.Add("[" + read["columnData"].ToString() + "]");
                    }
                    var dataRows = myList.ToArray();
                    columnValueData = (string.Join(",", dataRows));
                    return columnValueData;
                    cn.Close();
                }

            }
            catch (Exception ex)
            {
                columnValueData = ex.Message;
                return columnValueData;
            }
    }
    private string SelectedGroupFields(ListBox selectedFields)
    {
        string selFields;
        try
        {
            var selTextList = new List<string>();
            int i = 0;
            foreach (ListItem li in selectedFields.Items)
            {
                selTextList.Add(li.Value.ToString().Trim());
                i++;
            }
            if (i == 0) { selTextList.Add("Please select at least one item"); }
            var selectedItemsRows = selTextList.ToArray();
            selFields = (string.Join(",", selectedItemsRows));
        }
        catch (Exception ex)
        {
            selFields = (ex.Message);
        }

        return selFields;
    }

    private string SelectedTextFields(ListBox selectedFields)
    {
        string selFields;
        try
        {
            var selTextList = new List<string>();
            var selGroupList = new List<string>();
            int i = 0;
            foreach (ListItem li in selectedFields.Items)
            {
                selTextList.Add(li.Value.ToString().Trim());
                selGroupList.Add(li.Value.ToString());
                i++;
            }
            if (i == 0) { selTextList.Add("Please select at least one item"); }
            var selectedItemsRows = selTextList.ToArray();
            var selectedGroupRows = selGroupList.ToArray();
            selFields = (string.Join(",", selectedItemsRows));
            lblTextFields.Text = selFields;
            lbSelectedGroupFields.DataSource = selectedGroupRows;
            lbSelectedGroupFields.DataBind();
            ddColumnField.DataSource = selectedGroupRows;
            ddColumnField.DataBind();
        }
        catch (Exception ex)
        {
            selFields = (ex.Message);
        }

        return selFields;
    }

    private string SelectedValueFields(ListBox selectedFields)
    {
        string selFields;
        try
        {
            var selValueList = new List<string>();
            var selValueList2 = new List<string>();
            int a = 0;
            if (Label5.Text == "Pivot Query")
            {
                foreach (ListItem la in selectedFields.Items)
                {
                    //selValueList.Add("Format(sum(" + la.Value.ToString().Trim() + "),'#,#') as " + la.Text);
                    selValueList.Add("sum(" + la.Value.ToString().Trim() + ") as " + la.Text);
                    selValueList2.Add(la.Value.ToString());
                    a++;
                }
                if (a == 0) { selValueList.Add("Please select at least one item"); }
                var selectedItemsRows = selValueList.ToArray();
                var selectedValueRows = selValueList2.ToArray();
                selFields = (string.Join(",", selectedItemsRows));
                lbSelectedValueFields.DataSource = selectedValueRows;
                lbSelectedValueFields.DataBind();
                ddColumnValue.DataSource = selectedValueRows;
                ddColumnValue.DataBind();
            }
            else
            {

                foreach (ListItem la in selectedFields.Items)
                {
                    selValueList.Add("sum(" + la.Value.ToString().Trim() + ") as " + la.Text);
                    selValueList2.Add(la.Value.ToString());
                    ddColumnField.Items.Add(la.Value.ToString());
                    a++;
                }
                if (a == 0) { selValueList.Add("Please select at least one item"); }
                var selectedItemsRows = selValueList.ToArray();
                var selectedValueRows = selValueList2.ToArray();
                selFields = (string.Join(",", selectedItemsRows));
                lbSelectedValueFields.DataSource = selectedValueRows;
                lbSelectedValueFields.DataBind();
            }
        }
        catch (Exception ex)
        {
            selFields = (ex.Message);
        }

        return selFields;
    }
    private string SelectedParameterValue(ListBox selectedFields)
    {
        string parValue;
        try
        {
            var selParValueList = new List<string>();
            int i = 0;
            foreach (ListItem li in selectedFields.Items)
            {
                //if (li.Selected)
                //{
                selParValueList.Add("'" + li.Value.ToString().Trim() + "'");
                i++;
                //}
            }
            if (i == 0)
                selParValueList.Add("Please select at least one item");
            var selectedItemsRows = selParValueList.ToArray();
            parValue = ("(" + string.Join(",", selectedItemsRows) + ")");
        }
        catch (Exception ex)
        {
            parValue = (ex.Message);
        }

        return parValue;
    }

    private void DisplayFieldData(DataTable dt, string parameters, string sortOrder)
    {
        DataRow[] foundRows;
        foundRows = dt.Select(parameters, sortOrder);
        for (int i = 0; i < foundRows.Length; i++) lbDataTableFields.Items.Add("dataTableField");
    }
    public void MoveItem(int direction)
    {
        // Checking selected item
        if (lbSelectedTableFields.SelectedItem == null || lbSelectedTableFields.SelectedIndex < 0)
            return; // No selected item - nothing to do

        // Calculate new index using move direction
        int newIndex = lbSelectedTableFields.SelectedIndex + direction;

        // Checking bounds of the range
        if (newIndex < 0 || newIndex >= lbSelectedTableFields.Items.Count)
            return; // Index out of range - nothing to do

        string selected = lbSelectedTableFields.SelectedItem.ToString();

        // Removing removable element
        lbSelectedTableFields.Items.Remove(lbSelectedTableFields.SelectedItem);
        // Insert it in new position
        lbSelectedTableFields.Items.Insert(newIndex, selected);
        // Restore selection
        //lbSelectedTableFields.SetSelected(newIndex, true);
        lbSelectedTableFields.SelectedIndex = newIndex;
    }
    private void SaveQuery(string queryName, string queryCommand)
    {
        SqlConnection sql = new SqlConnection(sqlConnection);
        sql.Open();
        try
        {
            SqlCommand cmd = new SqlCommand("insert into Pomps_Reports.dbo.Database_User_Query (UserID,QueryName,QueryCommand,QueryCreatedDate) values (@UserID,@QueryName,@QueryCommand,sysdatetime())", sql);
            cmd.Parameters.AddWithValue("@UserID", lblUserID.Text);
            cmd.Parameters.AddWithValue("@QueryName", queryName);
            cmd.Parameters.AddWithValue("@QueryCommand", queryCommand);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    private string GetUserID(string username)
    {
        string UserID;
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT UserID FROM Pomps_Reports.dbo.Database_User WHERE UserLogin = @UserLogin";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserLogin", username);

                    if (cmd.ExecuteScalar() != null)
                    {
                        UserID = cmd.ExecuteScalar().ToString();
                        connection.Close();
                    }
                    else
                    {
                        UserID = "";
                        connection.Close();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return UserID;
        }
    }
    private void FieldData(string dataTable,string dataTableField,string endYear,string endMonth)
    {
        string query, parameters;
        parameters = TableParameters(dataTable);

        if (lblParameterValues.Text != "")
        {
            query = "select distinct " + dataTableField + " as dataTableField from " + dataTable + " where " + lblParameterValues.Text + " and " + parameters;
        }
        else
        {
            query = "select distinct " + dataTableField + " as dataTableField from " + dataTable + " where " + parameters;
        }
        //lblError.Text = query;
        
        SqlConnection cn = new SqlConnection(sqlConnection);
        cn.Open();
        using (SqlDataAdapter da = new SqlDataAdapter(query, cn))
        {
            try
            {
                da.SelectCommand.CommandTimeout = 0;
                da.SelectCommand.Parameters.AddWithValue("@UserID", lblUserID.Text);
                da.SelectCommand.Parameters.AddWithValue("@StartYear", ddStartYear.SelectedValue);
                da.SelectCommand.Parameters.AddWithValue("@EndYear", endYear);
                da.SelectCommand.Parameters.AddWithValue("@StartMonth", ddStartMonth.SelectedValue);
                da.SelectCommand.Parameters.AddWithValue("@EndMonth", endMonth);
                da.Fill(dt);

                lbDataTableFields.DataSource = dt;
                lbDataTableFields.DataTextField = "dataTableField";
                lbDataTableFields.DataValueField = "dataTableField";
                lbDataTableFields.DataBind();

                cn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }

    private void DisplayFields(string dataTable, string tableFieldType, string type)
    {
        string queryString;
        lbDataTableFields.Items.Clear();


        queryString = @"SELECT TableFieldID,RTrim(TableFieldID) as TableFieldName FROM Pomps_Reports.dbo.QueryTableFields qtf INNER JOIN Pomps_Reports.dbo.QueryTables qt
ON qtf.QueryTableID = qt.QueryTableID WHERE TableID= @TableName and TableFieldActive = 1 and (RTRIM(TableFieldType) = @TableFieldType or 'All' in(@TableFieldType)) ORDER BY TableFieldName";
        
        try
        {
            SqlConnection cn = new SqlConnection(sqlConnection);
            cn.Open();
            using (SqlDataAdapter d = new SqlDataAdapter(queryString, cn))
            {
                // fill a data table
                var s = new DataTable();
                d.SelectCommand.Parameters.AddWithValue("@TableName", dataTable);
                d.SelectCommand.Parameters.AddWithValue("@TableFieldType", tableFieldType);
                d.Fill(s);

                // Bind the table to the list box
                if (type == "Parameter")
                {
                    ddTableFields.DataTextField = "TableFieldID";
                    ddTableFields.DataValueField = "TableFieldID";
                    ddTableFields.DataSource = s;
                    ddTableFields.DataBind();
                    
                }
                else
                {
                    lbDataTableFields.DataTextField = "TableFieldID";
                    lbDataTableFields.DataValueField = "TableFieldID";
                    lbDataTableFields.DataSource = s;
                    lbDataTableFields.DataBind();
                }
                cn.Close();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string TableParameters(string dataTable)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string par;
            try
            {
                string query = @"SELECT TableParameters FROM dbo.QueryTables WHERE TableID = @TableID and TableActive = 1";
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TableID", dataTable);
                if (cmd.ExecuteScalar() != null)
                {
                    par = cmd.ExecuteScalar().ToString();
                    //return par;
                }
                else
                {
                    par = "";
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                par = ex.Message;
            }
            return par;
        }
    }
    private void clearScreen()
    {
        btnAddPar.Text = "Add Parameter";
        btnAddPar.Visible = false;
        btnAddPar.Enabled = true;
        btnCmd.Text = "Next =>";
        btnCmd.Visible = false;
        btnCmd.Enabled = true;
        btnProcess.Text = "Reset";
        btnProcess.Visible = false;
        btnProcess.Enabled = true;
        btnContinue.Visible = false;
        ddColumnField.Items.Clear();
        ddColumnValue.Items.Clear();
        ddStartYear.SelectedValue = "0";
        ddEndYear.SelectedValue = "0";
        ddStartMonth.SelectedValue = "0";
        ddEndMonth.SelectedValue = "0";
        ddTableFields.Visible = false;
        ddDataTable.Enabled = true;
        ddDataTable.SelectedValue = "";
        ddDataTable.Visible = true;
        ddMyQueries.SelectedValue = "";
        GridView1.DataSource = "";
        GridView1.DataBind();
        imgBtnDownArrow.Visible = false;
        imgBtnLeftArrow.Visible = false;
        imgBtnRightArrow.Visible = false;
        imgBtnUpArrow.Visible = false;
        imgExcel.Visible = false;
        Label1.Text = "Saved Query";
        Label2.Text = "New Query";
        Label3.Text = "";
        Label4.Text = "";
        Label5.Text = "Select your Query Option";
        lblInstructions.Text = "";
        lblParameterValues.Text = "";
        lblTextFields.Text = "";
        lblError.Text = "";
        lblUserID.Visible = false;
        lblTextFields.Visible = false;
        lbDataTableFields.Items.Clear();
        lbDataTableFields.Rows = 1;
        lbDataTableFields.Visible = false;
        lbSelectedTableFields.Items.Clear();
        lbSelectedTableFields.Rows = 1;
        lbSelectedTableFields.Visible = false;
        lbSelectedGroupFields.Items.Clear();
        lbSelectedGroupFields.Rows = 1;
        lbSelectedValueFields.Items.Clear();
        lbSelectedValueFields.Rows = 1;
        lnkBtnTextFields.Enabled = true;
        lnkBtnValueFields.Enabled = true;
        lnkBtnParameters.Enabled = true;
        queryCmd.Visible = false;
        queryCmd.Text = "";
        RadioButtonSavedQuery.Visible = true;
        RadioButtonSavedQuery.Checked = false;
        RadioButtonListQuery.Visible = false;
        RadioButtonListQuery.Checked = false;
        RadioButtonNewQuery.Visible = true;
        RadioButtonNewQuery.Checked = false;
        RadioButtonPivotQuery.Visible = false;
        RadioButtonPivotQuery.Checked = false;
        txtQueryName.Text = String.Empty;
        txtQueryName.Enabled = true;
        tblSelectQueryOption.Visible = true;
        tblSelectQuery.Visible = false;
        tblSelectQuery.Enabled = true;
        tblNewQuery.Visible = false;
        tblNewQuery.Enabled = true;
        tblFieldSelection.Visible = false;
    }

    private void DisposeControls()
    {
        try
        {
            ddStartYear.Dispose();
            ddEndYear.Dispose();
            ddStartMonth.Dispose();
            ddEndMonth.Dispose();
            ddMyQueries.Dispose();
            ddDataTable.Dispose();
            ddColumnField.Dispose();
            ddColumnValue.Dispose();
            ddTableFields.Dispose();
            lbDataTableFields.Dispose();
            lbSelectedGroupFields.Dispose();
            lbSelectedTableFields.Dispose();
            lbSelectedValueFields.Dispose();
            RadioButtonListQuery.Dispose();
            RadioButtonNewQuery.Dispose();
            RadioButtonPivotQuery.Dispose();
            RadioButtonSavedQuery.Dispose();
            tblFieldSelection.Dispose();
            tblNewQuery.Dispose();
            tblSelectQuery.Dispose();
            tblSelectQueryOption.Dispose();
            txtQueryName.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void RegisterPostBackControl()
    {
        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnCmd);
        //ScriptManager.GetCurrent(this).RegisterPostBackControl(ddDataTable);
        ScriptManager.GetCurrent(this).RegisterPostBackControl(imgExcel);
        //ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkBtnParameters);
        ScriptManager.GetCurrent(this).RegisterPostBackControl(ddTableFields);
    }

    public override void Dispose()
    {
            base.Dispose();
        //this.Page.Dispose();
    }
    
    public override void VerifyRenderingInServerForm(Control control)
    {
    }

}