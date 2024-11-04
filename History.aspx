<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="WebApplication1.History" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <link rel="stylesheet" href="./CSS/History_Style.css" />

        <h5 class="view-history-title" style="font-size: 35px;">VIEW HISTORY</h5>

        <br />

        <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
            <ContentTemplate>
              <asp:HiddenField ID="HiddenField1" runat="server" />
              <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                <asp:ListItem Value="All">All</asp:ListItem>
                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                <asp:ListItem Value="Accepted">Accepted</asp:ListItem>
                <asp:ListItem Value="Deployed">Deployed</asp:ListItem>
                <asp:ListItem Value="Rejected">Rejected</asp:ListItem>
            </asp:DropDownList>

                <asp:GridView ID="gvRoomRequestHistory" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-striped table-bordered"
                    AllowPaging="True" PageSize="10" OnPageIndexChanging="gvRoomRequestHistory_PageIndexChanging"
                    OnRowDataBound="gvRoomRequestHistory_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + (gvRoomRequestHistory.PageIndex * gvRoomRequestHistory.PageSize) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="HistoryID" HeaderText="History ID" />--%>
                        <%--<asp:BoundField DataField="RequestID" HeaderText="Request ID" />--%>
                        <asp:BoundField DataField="RequestedByEmail" HeaderText="Requested by" />
                        <asp:BoundField DataField="RequestDateTime" HeaderText="Request Date & Time"
                            DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="ApprovalDateTime" HeaderText="Approval Date & Time"
                            DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                        <%--<asp:BoundField DataField="Remarks" HeaderText="Remarks" />--%>
                    </Columns>
                </asp:GridView>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>