<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="WebApplication1.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">

        <!-- Include Bootstrap CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" />
    
    <!-- Custom CSS -->
    <style>
        .nowrap {
            white-space: nowrap;
        }
    </style>
    <div class="card">
            <div class="card-header">
                <h2>Booking Module</h2>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CourseCode" HeaderText="Course Code" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="SectionName" HeaderText="Section" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="InstructorName" HeaderText="Instructor Name" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="Faculty" HeaderText="Faculty" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="PurposeoftheRoom" HeaderText="Room Purpose" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="BuildingName" HeaderText="Building" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="RoomName" HeaderText="Room Number" ItemStyle-CssClass="nowrap" />
                           <%-- <asp:BoundField DataField="TimeSlot" HeaderText="Time Slot" ItemStyle-CssClass="nowrap" />--%>
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" ItemStyle-CssClass="nowrap" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" ItemStyle-CssClass="nowrap" />
                            <asp:TemplateField HeaderText="PDF File">
                                <ItemTemplate>
                                    <asp:Button ID="btnViewPDF" runat="server" Text="View PDF" CssClass="btn btn-primary" CommandName="ViewPDF" CommandArgument='<%# Eval("RequestID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-between">
                                        <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success mr-1" CommandName="Approve" CommandArgument='<%# Eval("RequestID") %>' />
                                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger" CommandName="Reject" CommandArgument='<%# Eval("RequestID") %>' />
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="150px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!-- File upload section -->
                <div class="form-group">
                    <asp:FileUpload ID="FileUploadPDF" runat="server" CssClass="form-control-file" />
                </div>
                <div class="form-group">
                    <asp:Button ID="btnUploadPDF" runat="server" Text="Upload PDF" CssClass="btn btn-primary" OnClick="btnUploadPDF_Click" />
                </div>
                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>

         <!-- Include Bootstrap JS and dependencies -->
        <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>

    </form>
    
</asp:Content>

