using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
 
public partial class MasterPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringWriter _writer = new StringWriter();
        HttpContext.Current.Server.Execute("Views/FetchCustomer.aspx", _writer);
        string html = _writer.ToString();
    }
}