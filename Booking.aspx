<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="WebApplication1.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Booking_Style.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <form id="form1" runat="server">

        <asp:ScriptManager runat="server"></asp:ScriptManager>
         <!-- Loading overlay -->
        <div id="loadingOverlay" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%; background:rgba(255,255,255,0.8); z-index:9999;">
            <img src="\Images\Half circle.gif" alt="Loading..." style="position:absolute; top:50%; left:50%; transform:translate(-50%, -50%);">
        </div>
        <div class="content-wrapper">
            <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand" DataKeyNames="RequestID">
                    <Columns>
                         <asp:TemplateField HeaderText="No">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 + (GridView1.PageIndex * GridView1.PageSize) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="RequestedByEmail" HeaderText="RequestedByEmail" SortExpression="RequestedByEmail" />
                        <asp:BoundField DataField="Course" HeaderText="CourseCode" SortExpression="CourseCode" />
                        <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="Section" />
                        <asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />
                        <asp:BoundField DataField="Faculty" HeaderText="Faculty" SortExpression="Faculty" />
                        <asp:BoundField DataField="Building" HeaderText="Building" SortExpression="Building" />
                        <asp:TemplateField HeaderText="RoomNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomName" runat="server" Text='<%# Eval("Room") %>'></asp:Label>
                                <i class="fa-solid fa-triangle-exclamation" style='<%# CheckScheduleConflict(Container) %>'></i>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                        <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                        <asp:BoundField DataField="startTime" HeaderText="StartTime" SortExpression="StartTime" />
                        <asp:BoundField DataField="endTime" HeaderText="EndTime" SortExpression="EndTime" />
                        <asp:BoundField DataField="PurposeoftheRoom" HeaderText="PurposeoftheRoom" SortExpression="PurposeoftheRoom" />
                        <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:Image ID="imgFileType" runat="server" 
                                               ImageUrl='<%# GetFileIcon(Eval("ContentType").ToString()) %>' 
                                               AlternateText="File Icon" 
                                               Width="50px" Height="50px" 
                                               Visible='<%# !string.IsNullOrEmpty(Eval("FileName").ToString()) %>' />
                                    <br />
                                    <asp:LinkButton ID="lnkDownload" runat="server" 
                                                    PostBackUrl='<%# "~/DownloadFile.ashx?RequestID=" + Eval("RequestID") %>' 
                                                    Text='<%# Eval("FileName") %>' 
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("FileName").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnDeploy" runat="server" Text="Deploy" CommandName="Deploy" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnReject" runat="server" Text="Reject" CommandName="Reject" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-secondary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <!-- reject remarks modal -->
          <div class="modal fade" id="rejectModal" tabindex="-1" aria-labelledby="rejectModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                         <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                            <ContentTemplate>
                                    <asp:HiddenField ID="hiddenRequestId" runat="server" />
                                    <asp:HiddenField ID="HiddenRequester_Email" runat="server" />
                                    </ContentTemplate>
                        </asp:UpdatePanel>

                        <h5 class="modal-title" id="rejectModalLabel">Reject Request</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>Do you want to provide a remark for this rejection?</p>
                        <asp:TextBox ID="remarkText" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3"></asp:TextBox>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnSubmitRemark" runat="server" Text="Reject" OnClick="btnSubmitRemark_Click" CssClass="btn btn-danger" />

                    </div>
                </div>
            </div>
        </div>
        <!-- Confirm Add Modal -->
        <div class="modal fade" id="confirmADD" tabindex="-1" aria-labelledby="confirmADDLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="confirmADDLabel">Room Not Registered</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        The selected room is not registered in the system. Would you like to add it now?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="btnConfirmAddRoom">Add Room</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Add Room Modal -->
        <div class="modal fade" id="addRoomModal" tabindex="-1" aria-labelledby="addRoomModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="addRoomModalLabel">Add Room</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label for="roomName">Room Name</label>

                                    <asp:Label ID="lblRoom" runat="server" CssClass="alert alert-success" Visible="false"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtRoomName" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <label for="building">Select Building</label>
                                    <asp:DropDownList ID="ddlBuildings" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                <asp:Button ID="Button1" runat="server" Text="Add Room" CssClass="btn btn-primary" OnClick="btnConfirmAddRoom_Click"></asp:Button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <script>
            // Show the loading GIF when form is being submitted or button clicked
            $("form").on("submit", function () {
                $("#loadingOverlay").fadeIn();
            });

            // Hide the loading GIF after the page has fully loaded
            $(window).on("load", function () {
                $("#loadingOverlay").fadeOut(); // Hide loading gif
            });


            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                // Hide loading gif after partial postbacks
                $("#loadingOverlay").fadeOut();
            });
            function showConfirmAddModal() {
                var myModal = new bootstrap.Modal(document.getElementById('confirmADD'));
                myModal.show();
            }

            function showAddRoomModal() {
                var myModal = new bootstrap.Modal(document.getElementById('addRoomModal'));
                myModal.show();
            }

            document.addEventListener('DOMContentLoaded', function () {
                var confirmButton = document.getElementById('btnConfirmAddRoom');
                if (confirmButton) {
                    confirmButton.addEventListener('click', function () {
                        // Hide the confirm modal
                        var confirmModal = bootstrap.Modal.getInstance(document.getElementById('confirmADD'));
                        if (confirmModal) {
                            confirmModal.hide();
                        }

                        // Use a small delay to ensure modal hides before showing the next one
                        setTimeout(function () {
                            // Show the add room modal
                            showAddRoomModal();
                        }, 300); // 300 ms delay, adjust as needed
                    });
                }
            });
        </script>
    </form>
</asp:Content>