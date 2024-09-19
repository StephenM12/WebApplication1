using System;
using System.Web.UI;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                //page ids:
                //home = liHome
                //image display = liImageDisp
                //room sched = liRoomSched
                //booking = liBooking
                //request signature = liReqApp
                //history = liHistory
                //profile = liProfile
                //room request = liRoomReq

                int userlevel = user_Identity.user_level;
                switch (userlevel) 
                {
                    case 1:
                        //Response.Write("Welcome IFO");//remove later
                        liRoomReq.Visible = false;
                        liReqApp.Visible = false;
                        liRoomReq.Visible = false;

                        break;
                    case 2:
                        //Response.Write("Welcome user_Approval ");//Request Approval 
                        liImageDisp.Visible = false;
                        liRoomReq.Visible = false;
                        liBooking.Visible = false;
                        //liHistory.Visible = false;
                        liAddUser.Visible = false;


                        break;
                    case 3:
                        //Response.Write("Welcome user_Requester "); //Request Form
                        liImageDisp.Visible = false;
                        liBooking.Visible = false;
                        liReqApp.Visible = false;
                        //liHistory.Visible = false;
                        liAddUser.Visible = false;




                        break;

                }
                




            }
        }

        protected void SignOut(object sender, EventArgs e)
        {
            Response.Redirect("LogIn.aspx");
        }
    }
}