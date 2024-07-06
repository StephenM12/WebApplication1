<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Profile_Style.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <form id="form1" runat="server">

        <!-- Account details card-->
        <div class="card mb-4 w-50">
            <div class="card-header" style="font-weight: bold;">Account Information</div>
            <div class="card-body" style="font-weight: bold;">

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

                <!--Update Profile Modal Button-->
                <asp:Button ID="UpdateProfileBtn" runat="server" Text="Update Profile" CssClass="bg-color btn btn-primary bg-color" OnClientClick="openProfile(); return false;" />

                <script type="text/javascript">
                    function openProfile() {
                        $('#profileModal').modal('show');
                    }
                </script>

                <div class="modal fade" id="profileModal" tabindex="-1" aria-labelledby="profileModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" style="font-weight: bold;" id="profeileModalLabel">Update Profile</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">

                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Change Email Address:</label>
                                    <asp:TextBox ID="ProfileEmail" runat="server" class="width form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter New Email Address"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="EmailValidator" runat="server"
                                        ControlToValidate="ProfileEmail"
                                        ErrorMessage="Please enter a valid email address"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                    </asp:RegularExpressionValidator>
                                </div>

                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="Button1" runat="server" Text="Close" CssClass="bg-color btn btn-primary bg-color" />
                                <asp:Button ID="Button2" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" OnClick="save_Email_Changes" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>


        <!--Change Password-->
        <div class="card mb-4 w-50">
            <div class="card-header" style="font-weight: bold;">Change Password</div>
            <div class="card-body">
                <!-- Button trigger modal -->
                <asp:Button ID="PMOdalBtn" runat="server" Text="Update Password" CssClass="bg-color btn btn-primary bg-color" OnClientClick="openModal(); return false;" />
            </div>

        </div>


        <script type="text/javascript">
            function openModal() {
                $('#exampleModal').modal('show');
            }
        </script>

        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" style="font-weight: bold;" id="exampleModalLabel">Update Password</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">


                        <!-- Enter Current Password-->
                        <div class="form-outline mb-4 position-relative">
                            <label style="font-weight: bold;" class="form-label ms-3" for="PCurrentPassTB">Current Password</label>
                            <div class="input-group">
                                <asp:TextBox ID="PCurrentPassTB" runat="server" class="width form-control" type="text" placeholder="Enter Current Password"></asp:TextBox>

                            </div>
                        </div>


                        <!-- Enter New Password-->
                        <div class="form-outline mb-4 position-relative">
                            <label style="font-weight: bold;" class="form-label ms-3" for="PNewPassTB">New Password</label>
                            <div class="input-group">
                                <asp:TextBox ID="PNewPassTB" runat="server" class="width form-control" type="text" placeholder="Enter New Password"></asp:TextBox>

                            </div>
                        </div>

                        <!-- Confirm New Password -->
                        <div class="form-outline mb-4 position-relative">
                            <label style="font-weight: bold;" class="form-label ms-3" for="PConfirmTB">Confirm New Password</label>
                            <div class="input-group">
                                <asp:TextBox ID="PConfirmTB" runat="server" class="width form-control" type="text" placeholder="Confirm New Password"></asp:TextBox>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="PCloseBtn" runat="server" Text="Close" CssClass="bg-color btn btn-primary bg-color" />
                            <asp:Button ID="PSaveChangesBtn" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" OnClick="save_password_Changes" />
                        </div>

                        <script>
                            $(document).ready(function () {
                                $('#<%= PSaveChangesBtn.ClientID %>').click(function () {
                                    $('#PasswordSecurity').modal('show');
                                });
                            });
                        </script>


                        <!-- Modal -->
                        <div class="modal fade" id="PasswordSecurity" tabindex="-1" role="dialog" aria-labelledby="PasswordSecurityLabel" aria-hidden="true">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="PasswordSecurityLabel">Modal title</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        ...
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary">Save changes</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
