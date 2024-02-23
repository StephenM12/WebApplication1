<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="WebApplication1.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="./CSS/ResetPassword_Style.css" />
    <title></title>
</head>
<body>
    <div class="reset-password-container">
        <h2 class="reset-password-header">RESET PASSWORD</h2>
        <div class="form-group">
            <input type="text" placeholder="Username" />
        </div>
        <div class="form-group">
            <input type="password" placeholder="New Password" />
        </div>
        <div class="form-group">
            <input type="password" placeholder="Confirm Password" />
        </div>
        <div class="form-group otp-group">
            <input type="text" placeholder="OTP" />
            <button class="btn btn-secondary">Send OTP</button>
        </div>
        <div class="button-group">
            <button class="btn btn-secondary">Close</button>
            <button class="btn">Reset Password</button>
        </div>
    </div>

    <script>
        // You can add JavaScript here if you need to handle form submission or other interactions.
    </script>

</body>
</html>
