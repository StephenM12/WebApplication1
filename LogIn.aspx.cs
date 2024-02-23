using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

protected void LogInBtn_Click(object sender, EventArgs e)
{
    Response.Redirect("CMS.aspx");
}

protected void CreateAccBtn_Click(object sender, EventArgs e)
{
    Response.Redirect("CreateAccount.aspx");
}
    }
}