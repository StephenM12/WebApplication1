<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Style.css" />
     <form id="form1" runat="server">

    <!-- Account details card-->
    <div class="card mb-4">
        <div class="card-header">Account Information</div>
        <div class="card-body">
            
                <!-- Form Group (username)-->
                <div class="mb-3">
                    <label class="small mb-1" for="inputUsername">Name</label>
                    <asp:TextBox ID="TextBox3" runat="server" class=" full-width form-control"></asp:TextBox>
                <!-- Form Row-->
                <div class="row gx-3 mb-3">
                    <!-- Form Group (first name)-->
                    <div class="col-md-6">
                        <label class="small mb-1" for="inputFirstName">Username</label>
                        <input class="form-control" id="inputFirstName" type="text" placeholder="" value="">
                    </div>
                </div>
                <!-- Form Row        -->
                <div class="row gx-3 mb-3">
                    <!-- Form Group (organization name)-->
                    <div class="col-md-6">
                        <label class="small mb-1" for="inputOrgName">Email</label>
                        <input class="form-control" id="inputOrgName" type="text" placeholder="" value="">
                    </div>

                </div>
                <!-- Save changes button-->
                <button class="btn btn-primary" type="button">Update Profile</button>
            
        </div>
    </div>


    <!--Change Password-->
    <div class="card mb-4">
        <div class="card-header">Change Password</div>
        <div class="card-body">
            <!-- Button trigger modal -->
            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                Update Password
            </button>

            <!-- Modal -->
            <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Update Password</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">


                           
                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Current Password:</label>
                                    <asp:TextBox ID="CourseCodeTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Current Password"></asp:TextBox>
                                </div>
                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">New Password:</label>
                                    <asp:TextBox ID="TextBox1" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter New Password"></asp:TextBox>
                                </div>
                                <div class="form-outline mb-4">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Confirm New Password:</label>
                                    <asp:TextBox ID="TextBox2" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Confirm New Password"></asp:TextBox>
                                </div>
                           
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <button type="button" class="btn btn-primary">Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </form>


</asp:Content>
