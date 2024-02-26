using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//sql connection
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LogInBtn_Click(object sender, EventArgs e)
        {
            //Response.Redirect("CMS.aspx");
            SqlConnection storename = new SqlConnection("Server=tcp:bagongserver.database.windows.net,1433;Initial Catalog=bagongdb;Persist Security Info=False;User ID=Frankdb;Password=Frank12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            //variable that will hold login values (username and password)
            String username, password;
            username = UsernameTB.Text;
            password = PasswordTB.Text;

            try
            {
                String querry = "SELECT * FROM userLogin WHERE username= '" + UsernameTB.Text + "' AND password = '" + PasswordTB.Text + "'";

                SqlDataAdapter sda = new SqlDataAdapter(querry, storename);

                DataTable dtable = new DataTable();

                sda.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    username = UsernameTB.Text;
                    password = PasswordTB.Text;

                    Response.Redirect("CMS.aspx");


                }
                else
                {

                    

                }


            }
            catch
            {

                


            }
        }

        protected void CreateAccBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateAccount.aspx");
        }
    }
}