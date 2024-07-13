using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
	public partial class ImageDisplay : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void VRizalBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("VRizal.aspx");
        }
        protected void VEinsteinBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("VEinstein.aspx");
        }
        protected void VETYCBBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("VETYCB.aspx");
        }
    }
}