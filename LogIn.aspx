<%@ Page Title="" Language="C#" MasterPageFile="~/login_master.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs"
    Inherits="WebApplication1.login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./CSS/LogIn_Style.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container py-5 h-100">
            <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col-xl-11">
                    <div class="card rounded-3 text-black">
                        <div class="row g-0 gx-1 gy-1">
                            <div class="col-lg-6 bg-purple"
                                style="border-top-right-radius: 35px; border-bottom-right-radius: 35px;">
                                <div class="card-body p-md-5 mx-md-4">
                                    <div class="card-body-container p-4">
                                        <div class="text-center">
                                            <img src="images/logo.png" style="width: 185px;" alt="logo">
                                            <h4 class="mt-1 mb-5 pb-1" style="color: #6358DC; font-weight: bold;">E-Paper Content Management System
                                            </h4>
                                        </div>
                                         <!--Input Username--> 
                                        <form>
                                            <!--Input Username--> 
                                            <div class="form-outline mb-4">
                                                <label style="font-weight: bold;" class="form-label ms-3"
                                                    for="form2Example11">
                                                    Username</label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a user name" ControlToValidate="UsernameTB" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                                                <div class="input-group">
                                                    <div class="input-group-text">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16"
                                                            height="16" fill="currentColor"
                                                            class="bi bi-person-fill" viewBox="0 0 16 16">
                                                            <path
                                                                d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6" />
                                                        </svg>
                                                    </div>
                                                    <!--ID for Username TextBox-->
                                                    <asp:TextBox ID="UsernameTB" runat="server" class="form-control" type="text" placeholder="Enter Username"></asp:TextBox>
                                                </div>
                                            </div>
                                            <!--Input Password-->
                                            <div class="form-outline mb-2">
                                                <label style="font-weight: bold;" class="form-label ms-3"
                                                    for="PasswordTB">
                                                    Password</label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter a password" ControlToValidate="PasswordTB" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                                    
                                                <div class="input-group">
                                                    <div class="input-group-text">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16"
                                                            height="16" fill="currentColor" class="bi bi-lock-fill"
                                                            viewBox="0 0 16 16">
                                                            <path
                                                                d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2m3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2" />
                                                        </svg>
                                                    </div>
                                                    <!--ID for Password TextBox-->
                                                    <asp:TextBox ID="PasswordTB" runat="server" class="form-control" type="text" placeholder="Enter Password"></asp:TextBox>
                                                    <div class="input-group-text">
                                                        <svg id="togglePassword" xmlns="http://www.w3.org/2000/svg"
                                                            width="16" height="16" fill="currentColor"
                                                            class="bi bi-eye-fill" viewBox="0 0 16 16">
                                                            <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                                            <path
                                                                d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                                                        </svg>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- Redirect to Forgot Password-->
                                            <div class="row mb-3">
                                                <p class="text-end">
                                                    <a href="ForgotPassword.aspx"
                                                        style="color: #3B1867; font-weight: bold; text-decoration: none;">Forgot Password?</a>
                                                     <%--<span>Forgot Password?</span>--%>
                                                </p>
                                            </div>
                                            <div class="text-center pt-1 mb-5 pb-1">
                                                <!--ID for LogIn Button-->
                                                <asp:Button ID="LogInBtn" runat="server" Text="Log In" OnClick="LogInBtn_Click" class="btn btn-primary btn-block fa-lg full-width bg-purple" />
                                            </div>
                                            <div class="custom-line">
                                            </div>
                                            <div class="d-flex align-items-center justify-content-center pb-4">
                                                <!--ID for Create Account Button-->
                                                <asp:Button ID="CreateAccBtn" runat="server" Text="Create Account" OnClick="CreateAccBtn_Click" class="btn btn-primary btn-block fa-lg full-width bg-purple" />
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                            <!--Side Image-->
                            <div class="col-lg-6 d-md-none d-xl-block align-items-center justify-content-center">
                                <img src="images/Capture.png" class="img-fluid" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
