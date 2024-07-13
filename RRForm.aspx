<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RRForm.aspx.cs" Inherits="RoomRequestForm.RRForm" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />

    <style>
        /* General adjustments */
        body {
            font-size: 11px; /* Reduce the base font size */
        }

        .form-label {
            font-size: 11px; /* Reduce form label font size */
        }

        .form-control {
            font-size: 11px; /* Reduce form control font size */
        }

        .header-right h4 {
            font-size: 12px; /* Reduce header font size */
        }

        .custom-line {
            border-width: 1px; /* Thin the border */
        }

        .signature-box {
            height: 30px; /* Reduce signature box height */
            width: 200px; /* Reduce signature box width */
            border: 1px solid #000; /* Ensure the border is visible */
            margin: 0 auto; /* Center horizontally */
            margin-bottom: 10px; /* Add margin bottom */
        }

        /* Optional: Reduce margins and paddings */
        .form-outline {
            margin-bottom: 5px; /* Reduce bottom margin */
        }

        .mb-3, .pb-1 {
            margin-bottom: 3px !important; /* Adjust spacing */
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container mt-3">
            <div class="card-body p-md-3 mx-md-2">
                <div class="row">
                    <div class="col-md-6">
                        <div class="text-center">
                            <img src="Images/MMCL.png" style="width: 130px;" alt="logo" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="header-right">
                            <h4 class="mt-1 mb-1 pb-1 text-center">Permit to Use Mapua MCL Facilities and Items | PTX</h4>
                            <h4 class="mt-1 mb-1 pb-1 text-center">For Co-curricular, Extra curricular and Rentals</h4>
                            <h4 class="mt-1 mb-1 pb-1 text-center">FORM OVPA-002A</h4>
                            <h4 class="mt-1 mb-1 pb-1 text-center">Revision No: 03</h4>
                            <h4 class="mt-1 mb-1 pb-1 text-center">Revision Date: 1/03/2014</h4>
                        </div>
                    </div>
                </div>

                <hr class="custom-line" />
                    
                 <div class="form-outline mb-3">
                 <label style="font-weight: bold;" class="form-label ms-2" for="email">Email:</label>
                 <asp:TextBox ID="email" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Requester's Email"></asp:TextBox>
                 </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2" for="RCourseCodeTB">Course Code:</label>
                    <asp:TextBox ID="RCourseCodeTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Course Code"></asp:TextBox>
                </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2" for="RSectionTB">Section:</label>
                    <asp:TextBox ID="RSectionTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Section"></asp:TextBox>
                </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2" for="RProfTB">Professor/Instructor:</label>
                    <asp:TextBox ID="RProfTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Professor/Instructor"></asp:TextBox>
                </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2" for="RRoomNumberTB">Room Number:</label>
                    <asp:TextBox ID="RRoomNumberTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Room Number"></asp:TextBox>
                </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2" for="SelectBuildingDL">Select Building:</label>
                    <asp:DropDownList ID="SelectBuildingDL" runat="server" class="form-control">
                        <asp:ListItem Text="RIZAL" Value="RIZAL"></asp:ListItem>
                        <asp:ListItem Text="EINSTEIN" Value="EINSTEIN"></asp:ListItem>
                        <asp:ListItem Text="ETYCB" Value="ETYCB"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2"    for="RFacultyDL">Faculty:</label>
                    <asp:DropDownList ID="RFacultyDL" runat="server" class="form-control">
                        <asp:ListItem Text="CCIS" Value="CCIS"></asp:ListItem>
                        <asp:ListItem Text="MITL" Value="MITL"></asp:ListItem>
                        <asp:ListItem Text="CMET" Value="CMET"></asp:ListItem>
                        <asp:ListItem Text="ETYCB" Value="ETYCB"></asp:ListItem>
                        <asp:ListItem Text="CAS" Value="CAS"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-outline mb-3">
                    <div class="row align-items-center">
                        <div class="col-sm-6">
                            <label style="font-weight: bold;" class="form-label ms-2" for="SelectDateTB">Select Date:</label>
                            <asp:TextBox ID="SelectDateTB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-sm-6">
                            <label style="font-weight: bold;" class="form-label ms-2" for="EndDateTB">Select End Date:</label>
                            <asp:TextBox ID="EndDateTB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>  
                    </div>
                </div>

                <div class="form-outline mb-3">
                    <label style="font-weight: bold;" class="form-label ms-2" for="RTimeDL">Select Time:</label>
                    <asp:DropDownList ID="RTimeDL" runat="server" class="form-control">
                        <asp:ListItem Text="7:00 AM - 8:15 AM" Value="7:15 AM - 8:15 AM"></asp:ListItem>
                        <asp:ListItem Text="8:15 AM - 9:30 AM" Value="8:15 AM - 9:30 AM"></asp:ListItem>
                        <asp:ListItem Text="9:30 AM - 10:45 AM" Value="9:30 AM - 10:45 AM"></asp:ListItem>
                        <asp:ListItem Text="10:45 AM - 12:00 PM" Value="10:45 AM - 12:00 PM"></asp:ListItem>
                        <asp:ListItem Text="12:00 PM - 1:15 PM" Value="12:00 PM - 1:15 PM"></asp:ListItem>
                        <asp:ListItem Text="1:15 PM - 2:30 PM" Value="1:15 PM - 2:30 PM"></asp:ListItem>
                        <asp:ListItem Text="2:30 PM - 3:45 PM" Value="2:30 PM - 3:45 PM"></asp:ListItem>
                        <asp:ListItem Text="3:45 PM - 5:00 PM" Value="3:45 PM - 5:00 PM"></asp:ListItem>
                        <asp:ListItem Text="5:00 PM - 6:15 PM" Value="5:00 PM - 6:15 PM"></asp:ListItem>
                        <asp:ListItem Text="6:15 PM - 7:30 PM" Value="6:15 PM - 7:30 PM"></asp:ListItem>
                        <asp:ListItem Text="7:30 PM - 8:45 PM" Value="7:30 PM - 8:45 PM"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <hr class="custom-line" />

          
                <div class="d-flex justify-content-center">
                    <asp:Button ID="btn_Export" runat="server" Text="Submit" Onclick="BtnExport_Click"/>
                    <asp:Button ID="btn_Back" runat="server" Text="Back" Onclick="Btn_Back" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>