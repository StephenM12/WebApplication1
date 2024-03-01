<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="WebApplication1.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />
    <link rel="stylesheet" href="./CSS/ForgotPassword_Style.css" />
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

                        </div>
                        <div class="mb-3">
                            <div class="row mb-0">
                                <p class="text-start fs-4">
                                    <a class="d-flex align-items-center justify-content-center pb-1" style="color: #3B1867; font-weight: bold; text-decoration: none;">Reset Password</a>
                                </p>
                            </div>

                        </div>
                        <form id="form1" runat="server">

                                <div class="mb-3">
                                    <!--ID for Email Text Box-->
                                    <asp:TextBox ID="RNewPassTB" runat="server" class="form-control mb-3" type="text" placeholder="Enter New Password"></asp:TextBox>
                                    <div class="d-flex align-items-center justify-content-center pb-1">
                                        </div>
                                    


                                    <div class="mb-3">
                                        <!--ID for Email Text Box-->
                                        <asp:TextBox ID="RConfirmPassTB" runat="server" class="form-control mb-3" type="text" placeholder="Confirm New Password"></asp:TextBox>
                                        <div class="d-flex align-items-center justify-content-center pb-1">
                                        </div>

                                        <div class="mb-3">
                                            <!--ID for Email Text Box-->
                                            <asp:TextBox ID="TextBox3" runat="server" class="form-control mb-3" type="text" placeholder="Enter Verification Code"></asp:TextBox>
                                            <div class="d-flex align-items-center justify-content-center pb-1">
                                            </div>


                                            <!--ID for Reset Password Button-->
                                            <asp:Button ID="ResetPassBtn" runat="server" Text="Reset Password" OnClick="ResetPassBtn_Click" class="bg-purple btn btn-primary full-width bg-purple" />
                                        </div>
                                    </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <%--<form id="form1" runat="server">
        <div class="reset-password-container">
            <h2 class="reset-password-header">RESET PASSWORD</h2>
            <div class="form-group">
            </div>
            <div class="form-group">
                <asp:TextBox ID="TextBox1" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="New Password"></asp:TextBox>

            </div>
            <div class="form-group">
                <asp:TextBox ID="TextBox2" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Confirm New Password"></asp:TextBox>

            </div>
            <div class="form-group otp-group">
                <asp:TextBox ID="TextBox3" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Verification Code"></asp:TextBox>

                <button class="btn btn-secondary">Send Verification Code</button>
            </div>
            <div class="button-group">
                  <asp:Button ID="CloseBtn" runat="server" Text="Close" OnClick="CloseBtn_Click" class="bg-purple btn btn-primary full-width bg-purple" />


                  <asp:Button ID="RPBtn" runat="servText="R Passwor








d" OnClick="RPBtn_Click" class="bg-purple btn btn-primary full-width bg-purple" />
            </div>
        </div>--%>


    <%--</form>--%>




    <script>
        // You can add JavaScript here if you need to handle form submission or other interactions.
    </script>

</body>
</html>
