using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using Newtonsoft.Json;
using System.IO;

public partial class FetchCustomer : System.Web.UI.Page
{
    public string sjson;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("hi");
        /*string st = ConfigurationManager.ConnectionStrings["old"].ConnectionString;
        var id = Request.QueryString["Tag"];
        if (true)
        {
            using (SqlConnection con = new SqlConnection(st))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Top 10 Date,NumberOfHits FROM [dbo].[ProdDollar_TagAggregation]", con))
                {
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    /*if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Response.Write("{");
                            Response.Write("'Tag:'" + rdr["Tag"].ToString() + ",");
                            Response.Write("'Hits:'" + Convert.ToInt32(rdr["NumberOfHits"]));
                            Response.Write("}");
                        }
                    }
                    // Create a DataTable object to hold all the data returned by the query.
                    DataTable dataTable = new DataTable("Test table");

                    // Use the DataTable.Load(SqlDataReader) function to put the results of the query into a DataTable.
                    dataTable.Load(rdr);

                    sjson ;
                    Response.Write("hi");
                }
            }
        }
        else
        {
        } */
    }
}