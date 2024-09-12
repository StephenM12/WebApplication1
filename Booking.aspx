<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="WebApplication1.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <form id="form1" runat="server">

        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                    Height="323px" Width="973px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand" DataKeyNames="RequestID">
                    <Columns>
                        <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
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