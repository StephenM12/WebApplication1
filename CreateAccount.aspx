<%@ Page Title="" Language="C#" MasterPageFile="~/login_master.Master" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="WebApplication1.create_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./CSS/CreateAccount_Style.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
   <script>
       // JavaScript code goes here
       // Define your JavaScript code directly within the <script> tags
       // This script will be executed when the browser encounters it
       document.addEventListener('DOMContentLoaded', function () {
           // For Password Field
           const togglePassword = document.getElementById('showPassword');
           const passwordField = document.getElementById('<%= PasswordID.ClientID %>');

        togglePassword.addEventListener('click', function () {
            const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordField.setAttribute('type', type);
            // Change the SVG icon based on the current state
            if (type === 'text') {
                togglePassword.innerHTML = `
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                        <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z"/>
                        <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z"/>
                    </svg>`;
            } else {
                togglePassword.innerHTML = `
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                    </svg>`;
            }
        });

        // For Confirm Password Field
        const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
        const confirmPasswordField = document.getElementById('<%= ConfirmPasswordID.ClientID %>');

        toggleConfirmPassword.addEventListener('click', function () {
            const type = confirmPasswordField.getAttribute('type') === 'password' ? 'text' : 'password';
            confirmPasswordField.setAttribute('type', type);
            // Change the SVG icon based on the current state
            if (type === 'text') {
                toggleConfirmPassword.innerHTML = `
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                        <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z"/>
                        <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z"/>
                    </svg>`;
            } else {
                toggleConfirmPassword.innerHTML = `
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                    </svg>`;
            }
        });
    });
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="vh-100 bg-image"
        style="background-image: url('https://mdbcdn.b-cdn.net/img/Photos/new-templates/search-box/img4.webp'); background-repeat: no-repeat; background-size: 100%;">
        <div class="mask d-flex align-items-center h-100 gradient-custom-3">
            <div class="container h-100">
                <div class="row d-flex justify-content-center align-items-center h-100">
                    <div class="col-12 col-md-9 col-lg-7 col-xl-6">
                        <div class="card" style="border-radius: 15px;">
                            <div class="card-body p-5" style="background-color: #D9D9D9;">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <p class="fs-3 text-center text-uppercase" style="color: #6358DC; font-weight: bold;">Create Account</p>
                                    </div>

                                    <div class="col-sm-6 text-end">
                                        <a href="LogIn.aspx" class="btn-close" style="font-size: 43px; color: black; font-weight: bold;">
                                            <i class="fa fa-trash-o" aria-hidden="true"></i>
                                        </a>
                                    </div>
                                </div>

                                <form>
                                    <div class="custom-line"></div>
                                    <!--ID's for Create Account TextBoxes-->
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Username</label>
                                        <asp:TextBox ID="UsernameID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Username"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example3cg">First Name</label>
                                        <asp:TextBox ID="FirstNameID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter First Name"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example4cg">Last Name</label>
                                        <asp:TextBox ID="LastNameID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Last Name"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example4cdg">Email</label>
                                        <asp:TextBox ID="EmailID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Email"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4 position-relative">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example5cdg">Password</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="PasswordID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="password" placeholder="Enter Password"></asp:TextBox>
                                            <div class="input-group-text show-password-icon" id="showPassword">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                                    <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                                    <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                                </svg>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-outline mb-4 position-relative">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example5cdg">Confirm Password</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="ConfirmPasswordID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="password" placeholder="Confirm Password"></asp:TextBox>
                                            <div class="input-group-text show-password-icon" id="toggleConfirmPassword">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                                    <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                                    <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                                </svg>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <!--ID for Create Account Button-->
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="CreateAccBtn" runat="server" Text="Create Account" OnClick="CreateAccBtn_Click" class="btn btn-primary btn-block btn-lg full-width bg-purple" />
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>





