using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;



namespace WebApplication1
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ResetPassBtn_Click(object sender, EventArgs e)
        {
            //code here:
            string newpass = RNewPassTB.Text;

            
            int userId = user_Identity.userID;
            bool chngPass = changePassword.changePass(userId, newpass);

            if (chngPass == true) { Response.Redirect("LogIn.aspx"); }
            else { Response.Write("Failed to change the password"); }


            
        }

        protected void CloseBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("LogIn.aspx");
        }

        protected void RPBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("LogIn.aspx");
        }
    }
}