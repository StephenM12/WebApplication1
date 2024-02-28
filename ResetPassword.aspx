<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="WebApplication1.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="./CSS/ResetPassword_Style.css" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
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


                  <asp:Button ID="RPBtn" runat="server" Text="Reset Password" OnClick="RPBtn_Click" class="bg-purple btn btn-primary full-width bg-purple" />
            </div>
        </div>


    </form>













    <script>
        // You can add JavaScript here if you need to handle form submission or other interactions.
    </script>

</body>
</html>
