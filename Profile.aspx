<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Profile_Style.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <!-- Account details card-->
        <div class="card mb-4 w-50">
            <div class="card-header" style="font-weight: bold;">Account Information</div>
            <div class="card-body" style="font-weight: bold;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                    <ContentTemplate>
                        <!-- Form Group (username)-->
                        <div class="mb-3">
                            <label class="small mb-1" for="inputUsername">Name</label>
                            <asp:TextBox ID="PNameTB" runat="server" CssClass="width form-control"></asp:TextBox>
                        </div>
                        <!-- Form Group (first name)-->
                        <div class="mb-3">
                            <label class="small mb-1" for="inputFirstName">Username</label>
                            <asp:TextBox ID="PUsernameTB" runat="server" CssClass="width form-control"></asp:TextBox>
                        </div>
                        <!-- Form Group (organization name)-->
                        <div class="mb-3">
                            <label class="small mb-1" for="inputOrgName">Email</label>
                            <asp:TextBox ID="PEmailTB" runat="server" CssClass="width form-control"></asp:TextBox>
                        </div>
                        <%-- <div class="mb-3">
                            <label class="small mb-1" for="inputPassword">Password</label>
                            <asp:TextBox ID="PPasswordTB" runat="server" TextMode="Password" CssClass="width form-control"></asp:TextBox>
                        </div>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--Update Profile Modal Button-->
                <asp:Button ID="UpdateProfileBtn" runat="server" Text="Update Profile" CssClass="button" OnClientClick="openProfile(); return false;" />
            </div>
        </div>

        <!-- Password update card -->
        <div class="card mb-4 w-50">
            <div class="card-header" style="font-weight: bold;">Update Password</div>
            <div class="card-body" style="font-weight: bold;">
                <asp:Button ID="UpdatePasswordBtn" runat="server" Text="Change Password" CssClass="button" OnClientClick="openPasswordModal(); return false;" />
            </div>
        </div>

        <!-- Password Update Modal -->
        <div class="modal fade" id="passwordModal" tabindex="-1" aria-labelledby="passwordModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="passwordModalLabel" style="font-weight: bold;">Change Password</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server" ID="UpdatePanelPassword">
                            <ContentTemplate>
                                <!-- Current Password -->
                                <div class="mb-3">
                                    <label for="currentPassword" class="small mb-1">Current Password</label>
                                    <asp:TextBox ID="CurrentPasswordTB" runat="server" TextMode="Password" CssClass="form-control" />
                                </div>
                                <!-- New Password -->
                                <div class="mb-3">
                                    <label for="newPassword" class="small mb-1">New Password</label>
                                    <asp:TextBox ID="NewPasswordTB" runat="server" TextMode="Password" CssClass="form-control" />
                                </div>
                                <!-- Confirm New Password -->
                                <div class="mb-3">
                                    <label for="confirmPassword" class="small mb-1">Confirm New Password</label>
                                    <asp:TextBox ID="ConfirmPasswordTB" runat="server" TextMode="Password" CssClass="form-control" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="SavePasswordBtn" runat="server" Text="Save" CssClass="btn btn-success" OnClick="SavePasswordBtn_Click" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="profileModal" tabindex="-1" aria-labelledby="profileModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" style="font-weight: bold;" id="profeileModalLabel">Update Profile</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>

                            <div class="form-group">
                                <label for="txtUserName">UserName</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control " Enabled="false" />
                            </div>
                            <div class="form-group">
                                <label for="txtFirstName">First Name</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control " Enabled="false" />
                            </div>
                            <div class="form-group">
                                <label for="txtLastName">Last Name</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control " Enabled="false" />
                            </div>
                            <div class="form-group">
                                <label for="txtEmail">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control " Enabled="false" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtEmail"
                                    ErrorMessage="Please enter a valid email address"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                </asp:RegularExpressionValidator>
                            </div>
                            <%--  <div class="form-group">
                                            <label for="txtPassword">Password</label>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Enabled="false" />
                                        </div>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnUpdate_Click" Visible="false" />
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" onclick="ResetModalFields()">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        function openProfile() {
            $('#profileModal').modal('show');
        }

        function openPasswordModal() {
            $('#passwordModal').modal('show');
        }
    </script>

    
</asp:Content>