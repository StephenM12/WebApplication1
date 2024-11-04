<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="WebApplication1.Verification" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />
    <link rel="stylesheet" href="./CSS/Verification_Style.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous" defer=""></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Verification</title>

    <style>
        .hyperlink-button {
            background: none;
            border: none;
            color: blue;
            text-decoration: underline;
            cursor: pointer;
            padding: 0;
            margin: 0;
            font-size: inherit;
        }
    </style>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" integrity="sha512-SzlrxWUlpfuzQ+pcUCosxcglQRNAq/DZjVsC0lE40xsADsfeQoEypE+enwcOiGjk/bSuGGKHEyjSoQ1zVisanQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />

    <script>
        $(document).ready(function () {
            $("#<%=TextBox1.ClientID %>").focus();

            $(".otp-letter-input").keydown(function (event) {
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
                    event.keyCode == 13 || (event.keyCode == 65 && event.ctrlKey === true) ||
                    (event.keyCode >= 35 && event.keyCode <= 40) || (event.keyCode >= 48 && event.keyCode <= 57) ||
                    (event.keyCode >= 96 && event.keyCode <= 105)) {
                } else {
                    if ((event.shiftKey || (event.keyCode < 48 || event.keyCode > 57)) &&
                        (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });

            $(".otp-letter-input").on('input', function () {
                var maxLength = parseInt($(this).attr('maxlength'));
                var currentLength = $(this).val().length;
                if (currentLength === maxLength) {
                    if ($(this).is('#<%=TextBox1.ClientID %>')) {
                        $("#<%=TextBox2.ClientID %>").focus();
                    } else if ($(this).is('#<%=TextBox2.ClientID %>')) {
                        $("#<%=TextBox3.ClientID %>").focus();
                    } else if ($(this).is('#<%=TextBox3.ClientID %>')) {
                        $("#<%=TextBox4.ClientID %>").focus();
                    } else {
                        $(this).next('.otp-letter-input').focus();
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container p-md-5 p-3">
            <div class="row">
                <div class="col-12 col-sm-8 col-md-5 mt-5 mx-auto">
                    <div class="bg-white p-4 p-md-5 rounded-3 shadow-sm border">
                        <div class="col-12 text-end">
                            <a href="LogIn.aspx" class="btn-close" style="font-size: 40px; color: black; font-weight: bold;">
                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                            </a>
                        </div>
                        <div>
                            <p class="text-center text-success">
                                <i class="fa-solid fa-envelope-circle-check" style="font-size: 5rem; color: #002855;"></i>
                            </p>
                            <p class="text-center h5">Please check your email</p>
                            <p class="text-center" id="myParagraph" runat="server"></p>
                            <div class="row pt-4 pb-2">
                                <div class="col-3">
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="otp-letter-input" MaxLength="1"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="otp-letter-input" MaxLength="1"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="otp-letter-input" MaxLength="1"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="otp-letter-input" MaxLength="1"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row pt-4">
                                <div class="text-center full-width">
                                    <asp:Button ID="VOTPBtn" runat="server" Text="Verify Code" OnClick="VOTPBtn_Click" class="btn btn-lg btn-primary full-width bg-purple" />
                                </div>
                                <br />

                                <p class="text-center pt-4">
                                    Didn't get the code?
                                    <asp:Button ID="ResendCodeBtn" runat="server" Text="Resend Code" CssClass="hyperlink-button" OnClick="resend_Code" />
                                </p>

                                <p class="text-center pt-2">Resend Verification Code In: <span id="timer">10:00</span></p>
                            </div>

                            <script>
                                $(document).ready(function () {
                                    function startTimer(duration, display) {
                                        var timer = duration, minutes, seconds;
                                        var intervalId = setInterval(function () {
                                            minutes = parseInt(timer / 60, 10);
                                            seconds = parseInt(timer % 60, 10);

                                            minutes = minutes < 10 ? "0" + minutes : minutes;
                                            seconds = seconds < 10 ? "0" + seconds : seconds;

                                            display.text(minutes + "m " + seconds + "s");

                                            if (--timer < 0) {
                                                clearInterval(intervalId);
                                                $('#resendLink').removeClass('disabled');
                                            }
                                        }, 1000);

                                        return intervalId;
                                    }

                                    $('#resendLink').click(function (e) {
                                        e.preventDefault();
                                        clearInterval(timerIntervalId);
                                        $(this).addClass('disabled');
                                        timerIntervalId = startTimer(600, $('#timer'));
                                    });

                                    var timerIntervalId = startTimer(600, $('#timer'));
                                });
                            </script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
