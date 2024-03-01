<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Style.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <form id="form1" runat="server">

        <!-- Account details card-->
        <div class="card mb-4">
            <div class="card-header" style="font-weight: bold;">Account Information</div>
            <div class="card-body" style="font-weight: bold;">

                <!-- Form Group (username)-->
                <div class="mb-3">
                    <label class="small mb-1" for="inputUsername">Name</label>
                    <asp:TextBox ID="PNameTB" runat="server" class="full-width form-control"></asp:TextBox>
                    <!-- Form Row-->
                    <div class="row gx-4 mb-3">
                        <!-- Form Group (first name)-->
                        <div class="col-md-6">
                            <label class="small mb-1" for="inputFirstName">Username</label>
                            <asp:TextBox ID="PUsernameTB" runat="server" class="full-width form-control"></asp:TextBox>
                        </div>
                    </div>
                    <!-- Form Row -->
                    <div class="row gx-3 mb-3">
                        <!-- Form Group (organization name)-->
                        <div class="col-md-6">
                            <label class="small mb-1" for="inputOrgName">Email</label>
                            <asp:TextBox ID="PEmailTB" runat="server" class="full-width form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Save changes button-->
                <asp:Button ID="UpdateProfileBtn" CssClass="bg-color btn btn-primary bg-color" runat="server" Text="Update Profile" />

            </div>
        </div>


        <!--Change Password-->
        <div class="card mb-4">
            <div class="card-header" style="font-weight: bold;">Change Password</div>
            <div class="card-body">
                <!-- Button trigger modal -->
                <asp:Button ID="PMOdalBtn" runat="server" Text="Update Password" CssClass="bg-color btn btn-primary bg-color" OnClientClick="openModal(); return false;" />

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



                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Current Password:</label>
                                    <asp:TextBox ID="PCurrentPassTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Current Password"></asp:TextBox>
                                </div>
                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">New Password:</label>
                                    <asp:TextBox ID="PNewPassTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter New Password"></asp:TextBox>
                                </div>
                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Confirm New Password:</label>
                                    <asp:TextBox ID="PConfirmTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Confirm New Password"></asp:TextBox>
                                </div>

                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="PCloseBtn" runat="server" Text="Close" CssClass="bg-color btn btn-primary bg-color" />
                                <asp:Button ID="PSaveChangesBtn" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>


</asp:Content>
