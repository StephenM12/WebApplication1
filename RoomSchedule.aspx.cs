using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public partial class RoomSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGridView();

        }

        ///Code to connect db to gridview
        private void BindGridView()
        {

            try
            {
                SqlConnection storename = new SqlConnection("Server=tcp:bagongserver.database.windows.net,1433;Initial Catalog=bagongdb;Persist Security Info=False;User ID=Frankdb;Password=Frank12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                storename.Open();
                SqlCommand select = new SqlCommand("SELECT * FROM schedtbl3", storename);
                SqlDataAdapter adap = new SqlDataAdapter(select);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds.Tables[0];

                    GridView1.DataBind();

                    


                }
            }
            catch (Exception ex)
            {
                throw;

            }


        }
    }
}