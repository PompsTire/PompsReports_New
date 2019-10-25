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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string username = Request.QueryString.Get("Username").ToString();
            lblUserID.Text = RetrieveUserAccess(username);
            clearScreen();
        }
    }
    protected void btnCancelQuery_Click(object sender, EventArgs e)
    {
        clearScreen();
    }
    protected void btnAddQueryFilter_Click(object sender, EventArgs e)
    {
        string queryFilters = lblQueryFilters.Text + " " + selectQueryFilters();
        lblQueryFilters.Text = queryFilters;
        ddAddQueryFilters.Visible = true;
        ddQueryFiltersFactor.SelectedValue = "";
        lbQueryFiltersValue.Items.Clear();
        lbQueryFiltersValue.Rows = 1;
        lbQueryFiltersValue.Visible = false;
    }
    protected void ddQueryFilters_TextChanged(object sender, EventArgs e)
    {
        lbQueryFiltersValue.Items.Clear();
        lbQueryFiltersValue.Rows = 1;
        lbQueryFiltersValue.Visible = false;
        generateQueryFilters(ddQueryFilters.SelectedValue.ToString(), ddDataTable.SelectedValue.ToString());
    }
    protected void ddQueryFiltersFactor_TextChanged(object sender, EventArgs e)
    {
        if (ddQueryFiltersFactor.SelectedItem.Text == "Equals" || ddQueryFiltersFactor.SelectedItem.Text == "Not Equals")
        {
            txtQueryFiltersValue.Text = "";
            txtQueryFiltersValue.Visible = false;
            lbQueryFiltersValue.Rows = 20;
            lbQueryFiltersValue.Visible = true;
        }
        else
        {
            lbQueryFiltersValue.Items.Clear();
            lbQueryFiltersValue.Rows = 1;
            lbQueryFiltersValue.Visible = false;
            txtQueryFiltersValue.Text = "";
            txtQueryFiltersValue.Visible = true;
        }
    }
    protected void btnGenerateQuery_Click(object sender, EventArgs e)
    {
        StringBuilder query = new StringBuilder();

        switch (lblMessage.Text)
        {
            case ("Select table"):
                if (ddQueryOption.SelectedValue == "det_qry")
                {
                    generateQueryFields(ddDataTable.SelectedValue, "All");
                    lblMessage.Text = "Select query fields";
                }
                else
                {
                    generateQueryFields(ddDataTable.SelectedValue, "text");
                    lblMessage.Text = "Select query text fields";
                }
                lbQueryFields.Rows = 20;
                ddDataTable.Visible = false;
                lbQueryFields.Visible = true;
                btnGenerateQuery.Text = "Continue";
                break;
            case ("Select query fields"):
                lblTxtFields.Text = selectQueryTextFields().ToString();
                query.Append("select " + lblTxtFields.Text + " from " + ddDataTable.SelectedValue);
                lblQuery.Text = query.ToString();
                lbQueryFields.Visible = false;
                lbQueryFields.Items.Clear();
                lbQueryFields.Rows = 1;
                ddQueryFilters.SelectedValue = "";
                ddQueryFilters.DataBind();
                ddQueryFilters.Visible = true;
                ddQueryFiltersFactor.Visible = true;
                btnAddQueryFilter.Visible = true;
                btnGenerateQuery.Text = "Continue";
                lblMessage.Text = "Select query filters";
                break;
            case ("Select query filters"):
                if (lblQueryFilters.Text == "")
                {
                    query.Append(lblQuery.Text);
                }
                else
                {
                    query.Append(lblQuery.Text + " Where " + lblQueryFilters.Text + " Group by " + lblTxtFields.Text);
                }
                lblQueryFilters.Visible = false;
                lblMessage.ForeColor = Color.Purple;
                lblMessage.Text = "Your Query: ";
                lblQuery.ForeColor = Color.Purple;
                lblQuery.Text = query.ToString();
                lbQueryFields.Visible = false;
                lbQueryFields.Items.Clear();
                lbQueryFields.Rows = 1;
                ddQueryFilters.SelectedValue = "";
                ddQueryFilters.DataBind();
                ddQueryFilters.Visible = false;
                ddQueryFiltersFactor.Visible = false;
                ddAddQueryFilters.Visible = false;
                lbQueryFiltersValue.Items.Clear();
                lbQueryFiltersValue.Visible = false;
                txtQueryFiltersValue.Text = "";
                txtQueryFiltersValue.Visible = false;
                btnAddQueryFilter.Visible = false;
                btnGenerateQuery.Text = "Process";
                break;
            case ("Your Query: "):
                query.Append(lblQuery.Text);
                string sqlQuery = query.ToString();
                generateQueryTable(sqlQuery);
                btnGenerateQuery.Text = "Print";
                break;
            case ("Print"):
                break;
            default:
                clearScreen();
                break;
        }

        lblQuery.Text = query.ToString();
    }

    private void generateQueryTable(string query)
    {
        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
            {
                try
                {
                    DataTable dTable = new DataTable();
                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(dTable);
                        GridView1.DataSource = dTable;
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = ex.Message;
                }
            }
            con.Close();
        }

    }

    private void generateQueryFields(string dataTable, string fieldType)
    {
        string sqlQuery;
        sqlQuery = "select rtrim(tbl_Field_Name) as field_Name from PompsTire_ERP.dbo.systemTableFields as tf inner join PompsTire_ERP.dbo.systemTables as t on tf.tbl_ID = t.tblID where tbl_SQL_Name = @dataTable and tbl_Field_Type = case when @fieldType = 'All' then tbl_Field_Type else @fieldType end";

        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@dataTable", dataTable);
                    cmd.Parameters.AddWithValue("@fieldType", fieldType);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lbQueryFields.Items.Add(reader["field_Name"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = (ex.Message);
                }
            }
            con.Close();
        }

    }

    private string selectQueryTextFields()
    {
        string queryRows;
        try
        {
            var myList = new List<string>();
            foreach (ListItem li in this.lbQueryFields.Items)
            {
                if (li.Selected)
                {
                    myList.Add(li.Text.ToString());
                }
            }
            var rows = myList.ToArray();
            queryRows = (string.Join(",", rows));
        }
        catch (Exception ex)
        {
            queryRows = (ex.Message);
        }

        return queryRows;
    }

    private void generateQueryFilters(string queryField, string dataTable)
    {
        string filtersQuery = "select distinct " + queryField + " as queryField from " + dataTable + " group by " + queryField;
        string queryFilter;
        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(filtersQuery, con))
            {
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryFilter = (reader["queryField"].ToString());
                            lbQueryFiltersValue.Items.Add(queryFilter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = ex.Message;
                }
            }
            con.Close();
        }
    }

    private string selectQueryFilters()
    {
        string queryFilter;
        try
        {
            var myFilters = new List<string>();

            foreach (ListItem f in this.lbQueryFiltersValue.Items)
            {
                if (f.Selected)
                {
                    myFilters.Add(f.Text.ToString());
                }
            }
            var par = myFilters.ToArray();
            if (ddAddQueryFilters.Visible == true)
            {
                if (txtQueryFiltersValue.Text != "")
                {
                    queryFilter = ddAddQueryFilters.SelectedValue + " " + ddQueryFilters.SelectedValue + " " + ddQueryFiltersFactor.SelectedValue + " " + " ('" + txtQueryFiltersValue.Text + "')";
                }
                else
                {
                    queryFilter = ddAddQueryFilters.SelectedValue + " " + ddQueryFilters.SelectedValue + " " + ddQueryFiltersFactor.SelectedValue + " " + " ('" + string.Join("','", par) + "')";
                }
            }
            else
            {
                if (txtQueryFiltersValue.Text != "")
                {
                    queryFilter = ddQueryFilters.SelectedValue + " " + ddQueryFiltersFactor.SelectedValue + " " + " ('" + txtQueryFiltersValue.Text + "')";
                }
                else
                {
                    queryFilter = ddQueryFilters.SelectedValue + " " + ddQueryFiltersFactor.SelectedValue + " " + " ('" + string.Join("','", par) + "')";
                }
            }
            return queryFilter;
        }
        catch (Exception ex)
        {
            lblError.ForeColor = Color.Red;
            lblError.Text = ex.Message;
            queryFilter = ex.Message;
            return queryFilter;
        }
    }

    private void clearScreen()
    {
        lblQuery.Text = "";
        lblMessage.Text = "Select table";
        lblQueryFilters.Text = "";
        btnGenerateQuery.Text = "Continue";
        ddDataTable.Visible = true;
        ddDataTable.SelectedValue = "";
        lbQueryFields.Visible = false;
        lbQueryFields.Items.Clear();
        lbQueryFields.Rows = 1;
        ddQueryFilters.SelectedValue = "";
        ddQueryFilters.Visible = false;
        ddQueryFiltersFactor.Visible = false;
        ddAddQueryFilters.Visible = false;
        lbQueryFiltersValue.Rows = 1;
        lbQueryFiltersValue.Visible = false;
        txtQueryFiltersValue.Text = "";
        txtQueryFiltersValue.Visible = false;
        btnAddQueryFilter.Visible = false;
    }
    protected string RetrieveUserAccess(string username)
    {
        using (SqlConnection connection = new SqlConnection(sqlConnection))
        {
            string query = @"SELECT UserID FROM dbo.Database_User WHERE UserLogin = @UserLogin";

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                try
                {
                    string userID;
                    cmd.Parameters.AddWithValue("@UserLogin", username);

                    if (cmd.ExecuteScalar() != null)
                    {
                        userID = cmd.ExecuteScalar().ToString();
                        connection.Close();
                    }
                    else
                    {
                        userID = "";
                        connection.Close();
                    }

                    return userID;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

    }
}