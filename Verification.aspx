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
    <title></title>

    <style>
        .hyperlink-button {
            background: none;
            border: none;
            color: blue; /* or any color you prefer for hyperlinks */
            text-decoration: underline;
            cursor: pointer;
            padding: 0; /* Remove padding */
            margin: 0; /* Remove margin */
            font-size: inherit; /* Inherit font size */
        }
    </style>

    <!-- bootstrap 5 stylesheet -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.0.1/css/bootstrap.min.css" integrity="sha512-Ez0cGzNzHR1tYAv56860NLspgUGuQw16GiOOp/I2LuTmpSK9xDXlgJz3XN4cnpXWDmkNBKXR/VDMTCnAaEooxA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <!-- fontawesome 6 stylesheet -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" integrity="sha512-SzlrxWUlpfuzQ+pcUCosxcglQRNAq/DZjVsC0lE40xsADsfeQoEypE+enwcOiGjk/bSuGGKHEyjSoQ1zVisanQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />

    <script>
        $(document).ready(function () {
            // Set focus on TextBox1 when the document is ready
            $("#<%=TextBox1.ClientID %>").focus();

            $(".otp-letter-input").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, enter, and numbers
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
                    event.keyCode == 13 || (event.keyCode == 65 && event.ctrlKey === true) ||
                    (event.keyCode >= 35 && event.keyCode <= 40) || (event.keyCode >= 48 && event.keyCode <= 57) ||
                    (event.keyCode >= 96 && event.keyCode <= 105)) {
                    // Let it happen, don't do anything
                }
                else {
                    // Ensure that it is a number and stop the keypress
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
                    // Move to the next input element
                    if ($(this).is('#<%=TextBox1.ClientID %>')) {
                        // If current input is TextBox1, move to TextBox2
                        $("#<%=TextBox2.ClientID %>").focus();
                    } else if ($(this).is('#<%=TextBox2.ClientID %>')) {
                        // If current input is TextBox2, move to TextBox3
                        $("#<%=TextBox3.ClientID %>").focus();
                    } else if ($(this).is('#<%=TextBox3.ClientID %>')) {
                        // If current input is TextBox3, move to TextBox4
                        $("#<%=TextBox4.ClientID %>").focus();
                    } else {
                        // Otherwise, move to the next input
                        $(this).next('.otp-letter-input').focus();
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container p-5">
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-5 mt-5">
                    <div class="bg-white p-5 rounded-3 shadow-sm border">
                        <div class="col-sm-11 text-end">
                            <a href="LogIn.aspx" class="btn-close" style="font-size: 40px; color: black; font-weight: bold;">
                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                            </a>
                        </div>
                        <div>
                            <p class="text-center text-success" style="font-size: 5.5rem;">
                                <i class="fa-solid fa-envelope-circle-check" style="color: #3B1867;"></i>
                            </p>
                            <p class="text-center text-center h5 ">Please check your email</p>
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
                                    <asp:Button ID="VOTPBtn" runat="server" Text="Verify Code" OnClick="VOTPBtn_Click" class="bg-purple btn btn:hover btn-primary full-width bg-purple" />
                                </div>
                                <br />

                                <p class="text-center pt-4">Didn't get the code?
                                    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="resend_Code" /></p>

                                <p class="text-center pt-2">Resend Verification Code In: <span id="timer">10:00</span></p>
                            </div>

                            <script>
                                $(document).ready(function () {
                                    // Timer countdown function
                                    function startTimer(duration, display) {
                                        var timer = duration, minutes, seconds;
                                        var intervalId = setInterval(function () {
                                            minutes = parseInt(timer / 60, 10);
                                            seconds = parseInt(timer % 60, 10);

                                            minutes = minutes < 10 ? "0" + minutes : minutes;
                                            seconds = seconds < 10 ? "0" + seconds : seconds;

                                            display.text(minutes + "m " + seconds + "s");

                                            if (--timer < 0) {
                                                clearInterval(intervalId); // Clear interval when timer reaches 0
                                                $('#resendLink').removeClass('disabled');
                                            }
                                        }, 1000);

                                        return intervalId; // Return interval id for clearing later
                                    }

                                    // Resend link click event
                                    $('#resendLink').click(function (e) {
                                        e.preventDefault();
                                        // Clear existing interval if any
                                        clearInterval(timerIntervalId);
                                        // Add your resend functionality here
                                        // For example, make an AJAX call to resend the code
                                        $(this).addClass('disabled'); // Disable the link while waiting for the timer
                                        timerIntervalId = startTimer(600, $('#timer')); // Set timer to 10 minutes (600 seconds)
                                    });

                                    // Start the initial timer
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