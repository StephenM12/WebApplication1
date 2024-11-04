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
                                    <a class="d-flex align-items-center justify-content-center pb-1" style="color: #002855; font-weight: bold; text-decoration: none;">Reset Password</a>
                                </p>
                            </div>

                        </div>
                        <form id="form1" runat="server">

                            <!-- Enter New Password-->
                            <div class="form-outline mb-3 position-relative">
                                <label style="font-weight: bold;" class="form-label ms-3" for="RNewPassTB">New Password</label>
                                <div class="input-group">
                                    <asp:TextBox ID="RNewPassTB" runat="server" class="form-control" type="password" placeholder="Enter New Password" ValidationGroup="valgrp2"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Confirm New Password -->

                            <div class="form-outline mb-3 position-relative">
                                <label style="font-weight: bold;" class="form-label ms-3" for="RConfirmPassTB">Confirm New Password</label>
                                <div class="input-group">
                                    <asp:TextBox ID="RConfirmPassTB" runat="server" class="form-control" type="password" placeholder="Confirm New Password" ValidationGroup="valgrp2"></asp:TextBox>
                                </div>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="RNewPassTB" ControlToCompare="RConfirmPassTB" Operator="Equal" ErrorMessage="Passwords do not match" ForeColor="#CC0000" ValidationGroup="valgrp2"></asp:CompareValidator>
                            </div>

                            <!--ID for Reset Password Button-->
                            <asp:Button ID="ResetPassBtn" runat="server" Text="Reset Password" OnClick="ResetPassBtn_Click" class="bg-purple btn btn-primary full-width bg-red" ValidationGroup="valgrp2" />

                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

