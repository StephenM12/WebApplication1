﻿using System;
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
                int userlevel = user_Identity.user_level;
                switch (userlevel) 
                {
                    case 1:
                        Response.Write("Welcome admin");//remove later
                        break;
                    case 2:
                        Response.Write("Welcome user 1");//remove later
                        liReqsig.Visible = false;
                        break;
                    case 3:
                        Response.Write("Welcome user 2");//remove later
                        liBooking.Visible = false;
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