<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Home_Style.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <form id="form1" runat="server">

        <div class="container mx-auto">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </h1>
            <br />

            <div class="dropdown">
                <asp:Button ID="SelectBuildingBtn" runat="server" Text="Select a Building" CssClass="btn btn-primary btn-block full-width bg-color dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false" />
                <div class="dropdown-content full-width">
                    <a href="#" onclick="showSchedule('Rizal Building')">Rizal Building</a>
                    <a href="#" onclick="showSchedule('Einstein Building')">Einstein Building</a>
                    <a href="#" onclick="showSchedule('ETYCB Building')">ETYCB Building</a>
                </div>
            </div>

            <div class="table-responsive" id="scheduleTable">
                <!-- GridView will be populated here based on selection -->
            </div>
        </div>


        <asp:PlaceHolder runat="server">
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
            <script src="Scripts/home.js"></script>
        </asp:PlaceHolder>
    </form>
</asp:Content>




