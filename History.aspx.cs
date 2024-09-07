using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class History : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindRoomRequestHistory();
            }

        }

        private void BindRoomRequestHistory()
        {
            //string query = "SELECT HistoryID, RequestID, Email,  RequestDateTime, ApprovalDateTime, Status, UpdatedBy, Remarks FROM RoomRequestHistory ORDER BY RequestDateTime DESC";

            string selectedStatus = ddlStatusFilter.SelectedValue;

            string query = "SELECT * FROM RoomRequestHistory";
            if (selectedStatus != "All")
            {
                query += " WHERE Status = @Status";
            }

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    if (selectedStatus != "All")
                    {
                        cmd.Parameters.AddWithValue("@Status", selectedStatus);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvRoomRequestHistory.DataSource = dt;
                    gvRoomRequestHistory.DataBind();
                }
            }
        }

        protected void gvRoomRequestHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoomRequestHistory.PageIndex = e.NewPageIndex;
            BindRoomRequestHistory();  
        }

        protected void gvRoomRequestHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Calculate the row number considering pagination
                int rowIndex = e.Row.RowIndex + 1 + (gvRoomRequestHistory.PageIndex * gvRoomRequestHistory.PageSize);
                e.Row.Cells[0].Text = rowIndex.ToString();
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRoomRequestHistory();
        }
    }
}