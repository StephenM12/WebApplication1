using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebApplication1.cs_files;
namespace WebApplication1
{
    public partial class Verification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            
        }
        protected void VOTPBtn_Click(object sender, EventArgs e)
        {
            // Concatenate the values of the four textboxes into a single string
            String concatenatedString = TextBox1.Text + TextBox2.Text + TextBox3.Text + TextBox4.Text;

            bool isPincorrect = pin_Valid_check.IsPinValid(concatenatedString);

            if (isPincorrect==true) { Response.Redirect("ResetPassword.aspx"); }
            else { Response.Write("Invalid"); }
           

        }
    }
}