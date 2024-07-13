<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VRizal.aspx.cs" Inherits="WebApplication1.VRizal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QkE+k9pRTCMF9aXdfMqH9d5cdBOUc+ANiNHlcMz8sHs7pO7mq7CK2reA9IVNJoZy" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="./CSS/VRizal_Style.css" />
    <section class="vh-100" style="background-image: url('Images/Rizal.png'); background-size: cover; background-position: center;" />

    <br />
    <br />

    <div class="container mx-auto">
        <h1 class="text-center mb-5">Rizal Rooms</h1>

        <style>
            .return-symbol {
                position: absolute;
                top: 120px; /* Adjust as needed */
                right: 350px; /* Adjust as needed */
            }
        </style>
        <!-- Return Symbol -->
        <div class="return-symbol">
            <a href="ImageDisplay.aspx">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-return-left" viewBox="0 0 16 16">
                    <path fill-rule="evenodd" d="M14.5 1.5a.5.5 0 0 1 .5.5v4.8a2.5 2.5 0 0 1-2.5 2.5H2.707l3.347 3.346a.5.5 0 0 1-.708.708l-4.2-4.2a.5.5 0 0 1 0-.708l4-4a.5.5 0 1 1 .708.708L2.707 8.3H12.5A1.5 1.5 0 0 0 14 6.8V2a.5.5 0 0 1 .5-.5" />
                </svg>
                Back to Image Display
            </a>
        </div>

        <div id="roomCards" class="row">
            <!-- Rizal Building Rooms -->
            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room1">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">R101</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room2">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">R102</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>

            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room3">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">R103</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>

            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room4">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">R104</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>

            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room5">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">Room 201</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room6">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">Room 202</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room7">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">Room 203</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>

            <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room8">
                <div class="card text-white bg-success">
                    <div class="card-body">
                        <h5 class="card-title">Room 204</h5>
                        <p class="card-text">Available</p>
                    </div>
                </div>
            </div>
            <!-- Repeat for remaining rooms in Rizal Building -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
