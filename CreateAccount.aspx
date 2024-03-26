<%@ Page Title="" Language="C#" MasterPageFile="~/login_master.Master" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="WebApplication1.create_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./CSS/CreateAccount_Style.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Complete This Field" ControlToValidate="UsernameID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="UsernameID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Username" ControlToValidate="UsernameID" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example3cg">First Name</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Pls Provide FirstName" ControlToValidate="FirstNameID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="FirstNameID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter First Name" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example4cg">Last Name</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Pls Provide LastName" ControlToValidate="LastNameID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="LastNameID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Last Name" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example4cdg">Email</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Email is Required" ControlToValidate="EmailID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="EmailID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Email" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>

                                    <div class="form-outline mb-4 position-relative">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example5cdg">Password</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Password is Required" ControlToValidate="PasswordID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <div class="input-group">
                                            <asp:TextBox ID="PasswordID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="password" placeholder="Enter Password" ValidationGroup="valgrp3" ></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-outline mb-4 position-relative">
                                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example5cdg">Confirm Password</label>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                ControlToValidate="PasswordID"
                                                ControlToCompare="ConfirmPasswordID"
                                                Operator="Equal"
                                                ErrorMessage="Passwords do not match."
                                                ValidationGroup="valgrp3" ForeColor="#CC0000"></asp:CompareValidator>
                                        
                                        <div class="input-group">
                                            
                                            <asp:TextBox ID="ConfirmPasswordID" runat="server" class="form-control" Style="background-color: #ECECEC;" type="password" placeholder="Confirm Password" ValidationGroup="valgrp3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br />
                                    <!--ID for Create Account Button-->
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="CreateAccBtn" runat="server" Text="Create Account" OnClick="CreateAccBtn_Click" class="btn btn-primary btn-block btn-lg full-width bg-purple" ValidationGroup="valgrp3" />
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