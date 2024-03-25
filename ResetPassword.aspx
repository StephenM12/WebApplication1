<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="WebApplication1.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />
    <link rel="stylesheet" href="./CSS/ForgotPassword_Style.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous" defer=""></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <title></title>
</head>
<body>

    <div class="container d-flex flex-column">
        <div class="row align-items-center justify-content-center
      min-vh-100">
            <div class="col-12 col-md-8 col-lg-4">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="col-sm-11 text-end">
                            <a href="LogIn.aspx" class="btn-close" style="font-size: 30px; color: black; font-weight: bold;">
                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                            </a>

                        </div>
                        <div class="mb-3">
                            <div class="row mb-0">
                                <p class="text-start fs-4">
                                    <a class="d-flex align-items-center justify-content-center pb-1" style="color: #3B1867; font-weight: bold; text-decoration: none;">Reset Password</a>
                                </p>
                            </div>

                        </div>
                        <form id="form1" runat="server">

                            <!-- Enter New Password-->
                            <div class="form-outline mb-3 position-relative">
                                <label style="font-weight: bold;" class="form-label ms-3" for="RNewPassTB">New Password</label>
                                <div class="input-group">
                                    <asp:TextBox ID="RNewPassTB" runat="server" class="form-control" type="password" placeholder="Enter New Password" ValidationGroup="valgrp1"></asp:TextBox>
                                    <div class="input-group-text show-password-icon" id="showPassword">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                            <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                            <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                        </svg>
                                    </div>
                                </div>
                            </div>

                            <!-- Confirm New Password -->

                            <div class="form-outline mb-3 position-relative">
                                <label style="font-weight: bold;" class="form-label ms-3" for="RConfirmPassTB">Confirm New Password</label>
                                <div class="input-group">
                                    <asp:TextBox ID="RConfirmPassTB" runat="server" class="form-control" type="password" placeholder="Confirm New Password" ValidationGroup="valgrp1"></asp:TextBox>
                                    <div class="input-group-text show-password-icon" id="toggleConfirmPassword">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                            <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                            <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                        </svg>
                                    </div>
                                </div>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="RConfirmPassTB" ControlToCompare="RNewPassTB" Operator="Equal" ErrorMessage="Passwords do not match" ForeColor="#CC0000"></asp:CompareValidator>
                            </div>

                            <!--ID for Reset Password Button-->
                            <asp:Button ID="ResetPassBtn" runat="server" Text="Reset Password" OnClick="ResetPassBtn_Click" class="bg-purple btn btn-primary full-width bg-purple"  ValidationGroup="valgrp1"/>

                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // For New Password Field
            const togglePassword = document.getElementById('showPassword');
            const newPasswordField = document.getElementById('<%= RNewPassTB.ClientID %>');

            togglePassword.addEventListener('click', function (event) {
                if (event.target === togglePassword) {
                    const type = newPasswordField.getAttribute('type') === 'password' ? 'text' : 'password';
                    newPasswordField.setAttribute('type', type);
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
                }
            });

            // For Confirm Password Field
            const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
            const confirmPasswordField = document.getElementById('<%= RConfirmPassTB.ClientID %>');

            toggleConfirmPassword.addEventListener('click', function (event) {
                if (event.target === toggleConfirmPassword) {
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
                }
            });
        });


    </script>

</body>
</html>



