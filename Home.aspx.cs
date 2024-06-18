using System;

using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userFname = user_Identity.user_FName;
            Label1.Text = "Welcome " + userFname;
        }
    }
}