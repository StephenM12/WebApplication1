<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="WebApplication1.History" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
    <link rel="stylesheet" href="./CSS/History_Style.css" />
  
   <h5 class="view-history-title" style="font-size: 35px;">VIEW HISTORY</h5>

    <br />

    <!-- Card Container -->
    <div class="card-container">
        <div class="card">
            <div class="card-body">
                <h5 class="card-title" style="font-weight:bold;">RIZAL</h5>
                <div class="button-container">
                    <asp:Button ID="VRizal" runat="server" Text="View History" class="btn btn-primary btn-block fa-lg full-width bg-purple" />
                </div>
            </div>
        </div>

        <br>

        <div class="card">
            <div class="card-body">
                <h5 class="card-title" style="font-weight: bold;">EINSTEIN</h5>
                <div class="button-container">
                    <asp:Button ID="VEinstein" runat="server" Text="View History" class="btn btn-primary btn-block fa-lg full-width bg-purple" />
                </div>
            </div>
        </div>

        <br>

        <div class="card">
            <div class="card-body">
                <h5 class="card-title" style="font-weight: bold;" >ETYCB</h5>
                <div class="button-container">
                    <asp:Button ID="VETYCB" runat="server" Text="View History" class="btn btn-primary btn-block fa-lg full-width bg-purple" />
                </div>
            </div>
        </div>
    </div>

    </form>

</asp:Content>
