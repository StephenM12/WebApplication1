using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text.pdf.draw;
using WebApplication1.cs_files;
using System.Data.SqlClient;
using System.Data;

namespace RoomRequestForm
{
    public partial class RequestSignatureForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindScheduleData(sender,e);


            }

        }

        protected void BindScheduleData(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    //connection.Open();
                    string query = @"
                    SELECT 
                        RR.RequestID, 
                        RR.email, 
                        C.CourseCode, 
                        S.SectionName, 
                        I.InstructorName, 
                        RR.Faculty, 
                        RR.PurposeoftheRoom, 
                        B.BuildingName, 
                        R.RoomName, 
                        RR.StartDate, 
                        RR.EndDate, 
                        RR.startTime, 
                        RR.endTime, 
                        RR.pdfData, 
                        RR.DateSigned, 
                        RR.status
                    FROM RoomRequest RR
                    LEFT JOIN Courses C ON RR.CourseID = C.CourseID
                    LEFT JOIN Sections S ON RR.SectionID = S.SectionID
                    LEFT JOIN Instructors I ON RR.InstructorID = I.InstructorID
                    LEFT JOIN Buildings B ON RR.BuildingID = B.BuildingID
                    LEFT JOIN Rooms R ON RR.RoomID = R.RoomID";


                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                    else
                    {
                        // No data found, display a message or handle accordingly
                        GridView1.EmptyDataText = "No data available";
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the error
                    Response.Write("Error: " + ex.Message);

                }
            }





        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

           

        }
        //For exporting to ng pdf
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=RoomRequestForm.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Create a new PDF document
            using (Document pdfDoc = new Document(PageSize.A4))
            {
                try
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();

                    //Create a table with 2 columns to hold the logo and text
                    PdfPTable table = new PdfPTable(2);
                    table.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 1f, 2f }; // Adjust column widths as needed
                    table.SetWidths(widths);

                    //Add logo
                    string logoPath = Server.MapPath("~/Images/MMCL.png");
                    if (File.Exists(logoPath))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.ScaleToFit(130f, 130f); // Adjust size as needed
                        PdfPCell logoCell = new PdfPCell(logo);
                        logoCell.Border = Rectangle.NO_BORDER;
                        table.AddCell(logoCell);
                    }
                    else
                    {
                        table.AddCell("");
                    }

                    //Add static text
                    PdfPCell textCell = new PdfPCell();
                    textCell.Border = Rectangle.NO_BORDER;
                    textCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    textCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    textCell.AddElement(new Paragraph("Permit to Use Mapua MCL Facilities and Items | PTX"));
                    textCell.AddElement(new Paragraph("For Co-curricular, Extra curricular and Rentals"));
                    textCell.AddElement(new Paragraph("FORM OVPA-002A"));
                    textCell.AddElement(new Paragraph("Revision No: 03"));
                    textCell.AddElement(new Paragraph("Revision Date: 1/03/2014"));
                    table.AddCell(textCell);
                    pdfDoc.Add(table);

                    //Add a line separator before the form fields
                    LineSeparator line = new LineSeparator(1, 100, BaseColor.BLACK, Element.ALIGN_CENTER, -2);
                    pdfDoc.Add(new Chunk(line));

                    //Add a newline after the line separator
                    pdfDoc.Add(Chunk.NEWLINE);

                    //Add form fields with line breaks between each field
                    //pdfDoc.Add(new Paragraph("Email: " + email.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Course Code: " + RCourseCodeTB.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Section: " + RSectionTB.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Professor/Instructor: " + RProfTB.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Room Number: " + RRoomNumberTB.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Selected Building: " + SelectBuildingDL.SelectedItem.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Faculty: " + RFacultyDL.SelectedItem.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Selected Date: " + SelectDateTB.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Selected End Date: " + EndDateTB.Text));
                    //pdfDoc.Add(Chunk.NEWLINE);
                    //pdfDoc.Add(new Paragraph("Selected Time: " + RTimeDL.SelectedItem.Text));

                    //Add a line separator after Select Time
                    pdfDoc.Add(new Chunk(line));

                    //Add a newline after the line separator
                    pdfDoc.Add(Chunk.NEWLINE);
                    pdfDoc.Add(Chunk.NEWLINE);
                    pdfDoc.Add(Chunk.NEWLINE);

                    //Create a paragraph for the signature

                    Paragraph signatureParagraph = new Paragraph();
                    signatureParagraph.Alignment = Element.ALIGN_RIGHT; // Aligns the content to the right

                    //Add signature image
                    string signaturePath = Server.MapPath("~/Images/Signature.png"); // Path to your signature image
                    if (File.Exists(signaturePath))
                    {
                        iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(signaturePath);
                        signatureImage.ScaleToFit(200f, 150f); // Adjust size as needed
                        signatureParagraph.Add(new Chunk(signatureImage, 25, -30, true));
                        pdfDoc.Add(signatureParagraph);
                    }

                    //Add signature line
                    Paragraph signatureLineParagraph = new Paragraph("________________________");
                    signatureLineParagraph.Alignment = Element.ALIGN_RIGHT; // Aligns the content to the right
                    pdfDoc.Add(signatureLineParagraph);

                    //Add signature label
                    Paragraph signatureLabelParagraph = new Paragraph("Signature Over Printed Name");
                    signatureLabelParagraph.Alignment = Element.ALIGN_RIGHT; // Aligns the content to the right
                    pdfDoc.Add(signatureLabelParagraph);

                    //Close the document
                    pdfDoc.Close();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    Response.End();
                }
            }

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e) 
        { 
        
        
        
        }
    }
}