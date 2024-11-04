<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RequestapprovalForm.aspx.cs" Inherits="RoomRequestForm.RequestSignatureForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/RequestapprovalForm_Style.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.css">
    <a href="https://www.flaticon.com/free-icons/docx" title="docx icons">Docx icons created by kliwir art - Flaticon</a>

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script src="Scripts/script.js"></script>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <!-- Loading overlay -->
        <div id="loadingOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255,255,255,0.8); z-index: 9999;">
            <img src="\Images\Half circle.gif" alt="Loading..." style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        </div>
        <div class="content-wrapper">
            <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand" DataKeyNames="RequestID" >
                        <Columns>
                             <asp:TemplateField HeaderText="No">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 + (GridView1.PageIndex * GridView1.PageSize) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RequestedByEmail" HeaderText="RequestedByEmail" SortExpression="RequestedByEmail" />
                            <asp:BoundField DataField="Course" HeaderText="CourseCode" SortExpression="CourseCode" />
                            <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="Section" />
                            <asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />
                            <asp:BoundField DataField="Faculty" HeaderText="Faculty" SortExpression="Faculty" />
                            <asp:BoundField DataField="Building" HeaderText="Building" SortExpression="Building" />
                            <asp:BoundField DataField="Room" HeaderText="RoomNumber" SortExpression="RoomNumber" />
                            <asp:BoundField DataField="PurposeoftheRoom" HeaderText="PurposeoftheRoom" SortExpression="PurposeoftheRoom" />
                            <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                            <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                            <asp:BoundField DataField="startTime" HeaderText="StartTime" SortExpression="StartTime" />
                            <asp:BoundField DataField="endTime" HeaderText="EndTime" SortExpression="EndTime" />
                           <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:Image ID="imgFileType" runat="server" 
                                               ImageUrl='<%# GetFileIcon(Eval("ContentType").ToString()) %>' 
                                               AlternateText="File Icon" 
                                               Width="50px" Height="50px" 
                                               Visible='<%# !string.IsNullOrEmpty(Eval("FileName").ToString()) %>' />
                                    <br />
                                    <asp:LinkButton ID="lnkDownload" runat="server" 
                                                    PostBackUrl='<%# "~/DownloadFile.ashx?RequestID=" + Eval("RequestID") %>' 
                                                    Text='<%# Eval("FileName") %>' 
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("FileName").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>


                             <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnDeploy" runat="server" Text="Accept" CommandName="Accept" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnReject" runat="server" Text="Reject" CommandName="Reject" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-secondary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="modal fade" id="rejectModal" tabindex="-1" aria-labelledby="rejectModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                         <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                        <asp:HiddenField ID="hiddenRequestId" runat="server" />
                        <asp:HiddenField ID="HiddenRequester_Email" runat="server" />
                        </ContentTemplate>
            </asp:UpdatePanel>

                        <h5 class="modal-title" id="rejectModalLabel">Reject Request</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>Do you want to provide a remark for this rejection?</p>
                        <asp:TextBox ID="remarkText" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3"></asp:TextBox>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnSubmitRemark" runat="server" Text="Reject" OnClick="btnSubmitRemark_Click" CssClass="btn btn-danger" />

                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            //// Show the loading GIF when form is being submitted or button clicked
            $("form").on("submit", function () {
                $("#loadingOverlay").fadeIn();
            });

            // Hide the loading GIF after the page has fully loaded
            $(window).on("load", function () {
                $("#loadingOverlay").fadeOut(); // Hide loading gif
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                // Hide loading gif after partial postbacks
                $("#loadingOverlay").fadeOut();
            });
        </script>
    </form>
</asp:Content>