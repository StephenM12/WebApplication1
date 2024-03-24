<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Profile_Style.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Function to toggle password visibility
            function togglePasswordVisibility(inputFieldId, eyeIconId) {
                const passwordField = document.getElementById(inputFieldId);
                const eyeIcon = document.getElementById(eyeIconId);

                // Initially hide the password
                passwordField.setAttribute('type', 'password');

                eyeIcon.addEventListener('click', function () {
                    const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
                    passwordField.setAttribute('type', type);
                    // Change the SVG icon based on the current state
                    if (type === 'text') {
                        eyeIcon.innerHTML = `
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                            <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z"/>
                            <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z"/>
                        </svg>`;
                    } else {
                        eyeIcon.innerHTML = `
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                            <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                            <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                        </svg>`;
                    }
                });
            }

            // Call the function for each password field when the page is loaded
            togglePasswordVisibility('<%= PCurrentPassTB.ClientID %>', 'showCurrentPass');
        togglePasswordVisibility('<%= PNewPassTB.ClientID %>', 'showPassword');
        togglePasswordVisibility('<%= PConfirmTB.ClientID %>', 'toggleConfirmPassword');
    });
    </script>



    <form id="form1" runat="server">

        <!-- Account details card-->
        <div class="card mb-4 w-100">
            <div class="card-header" style="font-weight: bold;">Account Information</div>
            <div class="card-body" style="font-weight: bold;">

                <!-- Form Group (username)-->
                <div class="mb-3">
                    <label class="small mb-1" for="inputUsername">Name</label>
                    <asp:TextBox ID="PNameTB" runat="server" CssClass="full-width form-control"></asp:TextBox>
                </div>
                <!-- Form Group (first name)-->
                <div class="mb-3">
                    <label class="small mb-1" for="inputFirstName">Username</label>
                    <asp:TextBox ID="PUsernameTB" runat="server" CssClass="full-width form-control"></asp:TextBox>
                </div>
                <!-- Form Group (organization name)-->
                <div class="mb-3">
                    <label class="small mb-1" for="inputOrgName">Email</label>
                    <asp:TextBox ID="PEmailTB" runat="server" CssClass="full-width form-control"></asp:TextBox>
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
                                </div>

                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="Button1" runat="server" Text="Close" CssClass="bg-color btn btn-primary bg-color" />
                                <asp:Button ID="Button2" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>


        <!--Change Password-->
        <div class="card mb-4 w-100">
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
                                <div class="input-group-text show-password-icon" id="showCurrentPass">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                    </svg>
                                </div>
                            </div>
                        </div>


                        <!-- Enter New Password-->
                        <div class="form-outline mb-4 position-relative">
                            <label style="font-weight: bold;" class="form-label ms-3" for="PNewPassTB">New Password</label>
                            <div class="input-group">
                                <asp:TextBox ID="PNewPassTB" runat="server" class="width form-control" type="text" placeholder="Enter New Password"></asp:TextBox>
                                <div class="input-group-text show-password-icon" id="showPassword">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                    </svg>
                                </div>
                            </div>
                        </div>

                        <!-- Confirm New Password -->
                        <div class="form-outline mb-4 position-relative">
                            <label style="font-weight: bold;" class="form-label ms-3" for="PConfirmTB">Confirm New Password</label>
                            <div class="input-group">
                                <asp:TextBox ID="PConfirmTB" runat="server" class="width form-control" type="text" placeholder="Confirm New Password"></asp:TextBox>
                                <div class="input-group-text show-password-icon" id="toggleConfirmPassword">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                    </svg>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="PCloseBtn" runat="server" Text="Close" CssClass="bg-color btn btn-primary bg-color" />
                            <asp:Button ID="PSaveChangesBtn" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" />
                        </div>
                    </div>
                </div>
            </div>
    </form>
</asp:Content>



