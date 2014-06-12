using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class FetchCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string st = ConfigurationManager.ConnectionStrings["old"].ConnectionString;
        var id = Request.QueryString["Tag"];
        if (id != null)
        {
            using (SqlConnection con = new SqlConnection(st))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Top 1000000 * FROM [dbo].[ProdDollar_TagAggregation] where Tag='" +id+"'", con))
                {
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Response.Write("<p>");
                            Response.Write("<strong> Tag:" + rdr["Tag"].ToString() + "</strong><br />");
                            Response.Write("<strong> NetworkID:" + rdr["NetworkID"].ToString() + "</strong><br />");
                            Response.Write("<strong> FarmID:" + rdr["FarmID"].ToString() + "</strong><br />");
                            Response.Write("<strong> Date:" + rdr["Date"].ToString() + "</strong><br />");
                            Response.Write("</p>");

                        }
                    }
                }
            }
        }
        else
        {
        }
    }
}