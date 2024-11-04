<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewUser.aspx.cs" Inherits="WebApplication1.ViewUseraspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-KyZXEAg3QhqLMpG8r+8fhAXLRGf25/FSjt4qlEap0DR0IMfE8Z6Lbc1D8ey5vTBz" crossorigin="anonymous">
    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">
    <!-- Font Awesome Icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <!-- Custom CSS -->
    <link rel="stylesheet" href="./CSS/ViewUser_Style.css" />
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="container">
            <div class="d-flex justify-content-between align-items-center mb-3">
                 <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                        <ContentTemplate>
                <h1>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </h1>
                            </ContentTemplate>
                    </asp:UpdatePanel>
                <div class="d-flex align-items-center">
                    <!-- Modal Button for New User with Icon -->
                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                        <ContentTemplate>
                            <button type="button" class="btn btn-primary btn-sm bg-red ms-3" onclick="openSched();">
                            <i class="bi bi-plus-circle"></i>New User
                            </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <script type="text/javascript">
                        function openSched() {
                            $('#AddUser').modal('show');
                        }

                    </script>
                    <!-- Search Bar -->
                    <div style="position: relative; width: 250px; margin-left: 20px;">
                        <i class="fas fa-search" style="position: absolute; top: 50%; left: 10px; transform: translateY(-50%);"></i>
                        <input type="text" class="form-control" id="searchBar" placeholder="Search">
                    </div>
                </div>
            </div>
            <!-- User Grid Table -->
            <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                        Height="323px" Width="973px" DataKeyNames="UserID" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="UserName" />
                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="UserPassword" HeaderText="Password" />
                            <asp:BoundField DataField="Faculty" HeaderText="Faculty" />
                            <asp:BoundField DataField="UserLevel" HeaderText="User Level" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="ShowModal" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandName="RemoveUser" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-danger" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--edit user--%>
            <div class="modal fade" id="editUserModal" tabindex="-1" role="dialog" aria-labelledby="editUserModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="editUserModalLabel">Edit User</h5>
                            <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <%--this is for userinfo ID--%>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
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
                                    </div>
                                    <div class="form-group">
                                        <label for="txtUserPassword">Password</label>
                                        <asp:TextBox ID="txtUserPassword" runat="server" CssClass="form-control " Enabled="false" />
                                    </div>
                                    <div class="form-group">
                                        <label for="ddlUserLevel">User Level</label>
                                       
                                        <asp:DropDownList ID="ddlUserLevel" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                <ContentTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnUpdate_Click" Visible="false" />
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" onclick="ResetModal()">Close</button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <%--for add user--%>
            <div class="modal fade" id="AddUser" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="AddModal">Add User</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                <ContentTemplate>
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold; font-size: 16px;" class="form-label ms-3" for="form3Example1cg">User Level:</label>
                                        <asp:DropDownList ID="userlvl" runat="server">
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <div class="mt-2 ms-3">
                                            <p style="font-size: 14px; color: #6c757d;">
                                                <strong>3</strong>: Requester<br>
                                                <strong>2</strong>: Approver<br>
                                                <strong>1</strong>: Super User
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example1cg">Faculty</label>
                                        <asp:DropDownList ID="RFacultyDL" runat="server" class="form-label ms-3"></asp:DropDownList>
                                         <asp:Panel ID="Panel2" runat="server" Style="display: inline;">
                                            <i class="fas fa-plus-circle ms-2" style="cursor: pointer;" data-bs-toggle="modal" data-bs-target="#addFacultyModal"></i>
                                        </asp:Panel>
                                    </div>
                                    <!-- Username Field -->
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example1cg">Username</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Complete This Field" ControlToValidate="UsernameID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="UsernameID" runat="server" class="form-control full-width" Style="background-color: #ECECEC; font-size: 14px;" type="text" placeholder="Enter Username" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>
                                    <!-- First Name Field -->
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example3cg">First Name</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Provide FirstName" ControlToValidate="FirstNameID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="FirstNameID" runat="server" class="form-control full-width" Style="background-color: #ECECEC; font-size: 14px;" type="text" placeholder="Enter First Name" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>
                                    <!-- Last Name Field -->
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example4cg">Last Name</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Provide LastName" ControlToValidate="LastNameID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="LastNameID" runat="server" class="form-control full-width" Style="background-color: #ECECEC; font-size: 14px;" type="text" placeholder="Enter Last Name" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>
                                    <!-- Email Field -->
                                    <div class="form-outline mb-4">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example4cdg">Email</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Email is Required" ControlToValidate="EmailID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="EmailID" runat="server" class="form-control full-width" Style="background-color: #ECECEC; font-size: 14px;" type="text" placeholder="Enter Email" ValidationGroup="valgrp3"></asp:TextBox>
                                    </div>
                                    <!-- Password Field -->
                                    <div class="form-outline mb-4 position-relative">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example5cdg">Password</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Password is Required" ControlToValidate="PasswordID" ForeColor="#CC0000" ValidationGroup="valgrp3"></asp:RequiredFieldValidator>
                                        <div class="input-group">
                                            <asp:TextBox ID="PasswordID" runat="server" class="form-control full-width" Style="background-color: #ECECEC; font-size: 14px;" type="password" placeholder="Enter Password" ValidationGroup="valgrp3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- Confirm Password Field -->
                                    <div class="form-outline mb-4 position-relative">
                                        <label style="font-weight: bold; font-size: 14px;" class="form-label ms-3" for="form3Example5cdg">Confirm Password</label>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                            ControlToValidate="PasswordID"
                                            ControlToCompare="ConfirmPasswordID"
                                            Operator="Equal"
                                            ErrorMessage="Passwords do not match."
                                            ValidationGroup="valgrp3" ForeColor="#CC0000"></asp:CompareValidator>
                                        <div class="input-group">
                                            <asp:TextBox ID="ConfirmPasswordID" runat="server" class="full-width form-control" Style="background-color: #ECECEC; font-size: 14px;" type="password" placeholder="Confirm Password" ValidationGroup="valgrp3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- Submit Button -->
                                    <div class="d-flex justify-content-center">
                                        <asp:Button ID="CreateAccBtn" runat="server" Text="Create Account" class="btn btn-primary btn-block btn-sm full-width bg-red" OnClick="CreateAccBtn_Click" ValidationGroup="valgrp3" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
             <!-- Add Faculty -->
            <div class="modal fade" id="addFacultyModal" tabindex="-1" aria-labelledby="addFacultyModalLabel" aria-hidden="true">
                <!-- Modal content -->
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Add New Faculty</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtNewFacultyName" runat="server" CssClass="form-control" Placeholder="Enter Faculty"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label1facult" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddFaculty" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddFaculty" runat="server" Text="Add Faculty" CssClass="btn btn-primary" OnClick="btnAddFaculty_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>