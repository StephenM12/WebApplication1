using System;
using System.Data.SqlClient;
using System.Web;
using WebApplication1.cs_files;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // Get the RequestID from the query string
            int requestId = Convert.ToInt32(context.Request.QueryString["RequestID"]);

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT FileName, ContentType, FileData FROM RoomRequest WHERE RequestID = @FileID", connection))
                {
                    cmd.Parameters.AddWithValue("@FileID", requestId);
                    //connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Get file data from the database
                            string fileName = reader["FileName"].ToString();
                            string contentType = reader["ContentType"].ToString();
                            byte[] fileData = (byte[])reader["FileData"];

                            // Send the file to the user
                            context.Response.Clear();
                            context.Response.ContentType = contentType;
                            context.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                            context.Response.BinaryWrite(fileData);
                            context.Response.Flush();
                            context.ApplicationInstance.CompleteRequest();  // Avoid thread termination issue
                        }
                        else
                        {
                            // Handle case where no file is found for the given RequestID
                            context.Response.StatusCode = 404;
                            context.Response.Write("File not found.");
                        }
                    }
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}