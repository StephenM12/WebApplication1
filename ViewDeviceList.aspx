<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDeviceList.aspx.cs" Inherits="WebApplication1.ViewDeviceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" />
    <link href='https://unpkg.com/boxicons/css/boxicons.min.css' rel='stylesheet' />
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap" rel="stylesheet"/>
    <link href='https://unpkg.com/boxicons/css/boxicons.min.css' rel='stylesheet'/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.css">
    <link href="CSS/ViewDeviceList.css" rel="stylesheet" />
    <script type="module" src="https://unpkg.com/boxicons@2.1.4/dist/boxicons.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
    

    

    <form id="form1" runat="server">
        <div class="container mt-5">            
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <div class="card">
                    <div class="cardcontent-header d-flex justify-content-between align-items-center">
                        <div><h2> Manage All E-Paper Device here </h2></div>
                        <div class="d-flex">
                            <div class="search-bar-container mr-3">
                                <input type="text" id="searchBar" class="search-bar" placeholder="Search devices..." />
                            </div>
                            <button type="button" class="add-device-btn" data-toggle="modal" data-target="#addModal">
                                <i class='bx bx-plus'></i>Add Device
                            </button>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="card-body">
                                <div class="card-container" id="deviceContainer">
                                    <asp:Label ID="NoDevicesLabel" runat="server" Text="No devices at this moment" Visible="false" CssClass="no-devices-message" />

                                    <asp:Repeater ID="Repeater1" runat="server">
                                        <ItemTemplate>
                                            <div class="card device-card"data-epd-id="<%# Eval("EPD_ID") %>" >
                                               <div class="card-header">
                                                    <!-- Three-dot icon for options -->
                                                    <div class="options-icon" onclick="toggleOptions(event, '<%# Eval("EPD_ID") %>')">
                                                        <i class='bx bx-dots-horizontal-rounded'></i>
                                                    </div>
                                                    <div class="options-dropdown" id="optionsDropdown-<%# Eval("EPD_ID") %>" style="display:none;">
                                                        <ul>
                                                            <li id="uploadImageOption-<%# Eval("EPD_ID") %>"
                                                                class="upload-image-option"
                                                                data-epdid='<%# Eval("EPD_ID") %>'
                                                                data-ipaddress='<%# Eval("AssignedIPAddress") %>'
                                                                onclick="uploadImage('<%# Eval("EPD_ID") %>', '<%# Eval("AssignedIPAddress") %>')">
                                                                <i class='bx bx-upload' style="margin-right: 5px;"></i> 
                                                                Upload Image
                                                            </li>
                                                        </ul>
                                                    </div>


                                                </div>

                                                <div class="card-body">
                                                    <div class="device-icon">
                                                        <i class='bx bxs-devices' ></i>
                                                    </div>
                                                    <h5 class="card-title"><%# Eval("EPD_ID") %></h5>
                                    
                                                    <div class="info-container">
                                        
                                                        <div class="info-row">
                                                            <span class="label-bold">Building:</span>
                                                            <span><%# Eval("AssignedBuilding") %></span>
                                                        </div>
                                                        <div class="info-row">
                                                            <span class="label-bold">Room Number:</span>
                                                            <span><%# Eval("AssignedRoomNumber") %></span>
                                                        </div>
                                                    </div>
                                                   <div class="device-mode-row">
                                                        <span class="device-mode-label">Device Mode:</span>
                                                        <label class="switch">
                                                            <asp:HiddenField ID="HiddenEPDID" runat="server" Value='<%# Eval("EPD_ID") %>' />
                                                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" 
                                                                            OnCheckedChanged="ToggleButton_CheckedChanged" 
                                                                            Checked='<%# Convert.ToString(Eval("DeviceStatus")) == "IMAGE" %>' />
                                                            <span class="slider round"></span>
                                                            <span class="text-label">TEXT</span> <!-- Label for TEXT -->
                                                            <span class="image-label">IMAGE</span> <!-- Label for IMAGE -->
                                                        </label>
                                                    </div>

                                                </div>
                                                <!-- Footer -->
                                                <div class="device-card-footer" data-epd-id="<%# Eval("EPD_ID") %>" onclick="setEditModalData('<%# Eval("EPD_ID") %>', '<%# Eval("AssignedBuilding") %>', '<%# Eval("AssignedRoomNumber") %>'); $('#editModal').modal('show');">
                                                    <span class="edit-text">Edit Device Information</span>
                                                    <i class='bx bx-edit edit-icon'></i>
                                                </div>
                                            </div>


                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            <!-- Edit Device Modal -->
            <div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
                <div class="modal-dialog custom-modal-width" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="editModalLabel">Edit Device Information</h5>
                            <!-- Close button removed -->
                        </div>
                        <div class="modal-body">
                            <div class="form-group d-flex justify-content-center mb-2">
                                <asp:TextBox ID="txtEditEPDID" runat="server" CssClass="form-control custom-textbox" Enabled="false" />
                            </div>              
                            <div class="form-group">
                                <div class="d-flex align-items-center mb-2">
                                    <!-- Assigned Building -->
                                    <label class="mr-2" for="txtViewBuilding">Building: </label>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlEditBuilding" runat="server" CssClass="form-control custom-dropdown select2" AutoPostBack="true" OnSelectedIndexChanged="ddlEditBuilding_SelectedIndexChanged" onchange="validateEditModalFormFields()">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlEditBuilding" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="d-flex align-items-center mb-2">
                                    <!-- Assigned Room Number -->
                                    <label class="mr-2" for="txtViewRoomNumber">Room Number:</label>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlEditRoomNumber" runat="server" CssClass="form-control custom-dropdown select2" onchange="validateEditModalFormFields()">
                                                <asp:ListItem Text="--Select Room--" Value="" />
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
               
                        </div>
                            <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="resetEditModal()">Cancel</button>
                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnEditSave_Click" />


                            <asp:Button ID="btnDelete" runat="server" Text="Delete Device" CssClass="btn btn-danger btn-delete" OnClick="btnDelete_Click" visible="false"/>

                        </div>
                    </div>
                </div>
            </div>

            <!-- Add New Device Modal -->
            <div class="modal fade" id="addModal" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" aria-hidden="true">
                <div class="modal-dialog custom-modal-width" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addModalLabel">Add New Device</h5>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="d-flex align-items-center mb-2">
                                    <label class="mr-2" for="txtNewEPDID">Device Name:</label>
                                    <asp:TextBox ID="txtNewEPDID" runat="server" CssClass="form-control mr-3" 
                                        oninput="validateFormFields()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="d-flex align-items-center mb-2">
                                    <label class="mr-2" for="ddlNewBuilding">Building:</label>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlNewBuilding" runat="server" CssClass="form-control" 
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlAddBuilding_SelectedIndexChanged" 
                                                                onchange="validateFormFields()">
                                                <asp:ListItem Text="--Select--" Value="" />
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlNewBuilding" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="d-flex align-items-center mb-2">
                                    <label class="mr-2" for="ddlNewRoomNumber">Room Number:</label>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlNewRoomNumber" runat="server" CssClass="form-control" 
                                                                onchange="validateFormFields()">
                                                <asp:ListItem Text="--Select Room--" Value="" />
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="resetAddModal()">Cancel</button>
                            <asp:Button ID="btnAddSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnAddSave_Click" Enabled ="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script>
    
            function setEditModalData(epdId, building, roomNumber) {
                document.getElementById('<%= txtEditEPDID.ClientID %>').value = epdId;

                // Set the selected building in the dropdown
                $('#<%= ddlEditBuilding.ClientID %>').val(building).trigger('change');

                // Set the selected room number in the dropdown
                

            }
            $(document).ready(function () {
                $('#addModal').on('hidden.bs.modal', function () {
                    // Clear EPD ID textbox
                    $(this).find('#<%= txtNewEPDID.ClientID %>').val('');

                    // Reset building and room dropdowns to default value
                    $(this).find('#<%= ddlNewBuilding.ClientID %>').prop('selectedIndex', 0);
                    $(this).find('#<%= ddlNewRoomNumber.ClientID %>').prop('selectedIndex', 0);
                });
            });
            $(document).ready(function () {
                // Initialize Select2 for the dropdown
                $('.select2').select2({
                    placeholder: "Select a Building",
                    width: 'resolve'
                });
            });
            $(document).ready(function () {
                // Initialize Select2 for the room number dropdown
                $('.select2').select2({
                    placeholder: "Select a Room Number",
                    width: 'resolve'
                });
            });
            $(document).ready(function () {
                // Detect keyup event on the search bar
                $('#searchBar').on('keyup', function () {
                    var searchTerm = $(this).val().toLowerCase().trim(); // Get the search term

                    // Filter cards by EPD ID
                    $('.device-card').each(function () {
                        var epdID = $(this).data('epd-id').toLowerCase(); // Get the EPD ID from the card

                        // Show or hide cards based on the search term
                        if (epdID.includes(searchTerm)) {
                            $(this).show();
                        } else {
                            $(this).hide();
                        }
                    });
                });
            });
            function validateFormFields() {
                // Get the values of the TextBox and DropDownLists
                var txtEPDID = document.getElementById('<%= txtNewEPDID.ClientID %>').value.trim();
                var ddlBuilding = document.getElementById('<%= ddlNewBuilding.ClientID %>').value;
                var ddlRoomNumber = document.getElementById('<%= ddlNewRoomNumber.ClientID %>').value;

                // Get the Save button element
                var btnSave = document.getElementById('<%= btnAddSave.ClientID %>');

                // Check if the TextBox is NOT empty and both DropDownLists have selected values
                if (txtEPDID.length !== 0 && ddlBuilding !== "" && ddlRoomNumber !== "") {
                    btnSave.disabled = false;  // Enable Save button if all fields are populated
                } else {
                    btnSave.disabled = true;   // Disable Save button if any field is empty
                }
            }

            function validateEditModalFormFields() {
                // Get the values of the TextBox and DropDownLists
                var txtEPDID = document.getElementById('<%= txtEditEPDID.ClientID %>').value.trim();
                var ddlBuilding = document.getElementById('<%= ddlEditBuilding.ClientID %>').value;
                var ddlRoomNumber = document.getElementById('<%= ddlEditRoomNumber.ClientID %>').value;

                // Get the Save button element
                var btnSave = document.getElementById('<%= btnSave.ClientID %>');

                // Check if the TextBox is NOT empty and both DropDownLists have selected values
                if (txtEPDID.length !== 0 && ddlBuilding !== "" && ddlRoomNumber !== "") {
                    btnSave.disabled = false;  // Enable Save button if all fields are populated
                } else {
                    btnSave.disabled = true;   // Disable Save button if any field is empty
                }
            }

            function resetAddModal() {
                $('#addModal').modal('hide');
                // Reset the device name TextBox
                document.getElementById('<%= txtNewEPDID.ClientID %>').value = '';

                // Reset the building dropdown list to the default value
                document.getElementById('<%= ddlNewBuilding.ClientID %>').selectedIndex = 0;

                // Reset the room number dropdown list to the default value
                document.getElementById('<%= ddlNewRoomNumber.ClientID %>').selectedIndex = 0;

                // Optionally disable the Save button
                document.getElementById('<%= btnAddSave.ClientID %>').disabled = true;
            }

            function resetEditModal() {
                $('#editModal').modal('hide');
                // Reset the EPD ID TextBox (though it's disabled, it's good to reset for consistency)
                document.getElementById('<%= txtEditEPDID.ClientID %>').value = '';

                // Reset the building dropdown list to the default value
                document.getElementById('<%= ddlEditBuilding.ClientID %>').selectedIndex = 0;

            // Reset the room number dropdown list to the default value
                document.getElementById('<%= ddlEditRoomNumber.ClientID %>').selectedIndex = 0;
            }
    
            function uploadImage(epdId, ipAddress) {
                // Debugging: log the values to ensure they're correct
                console.log('EPD_ID:', epdId);
                console.log('Assigned IP Address:', ipAddress);

                // Navigate to the IP address if available
                if (ipAddress && ipAddress.trim() !== "") {
                    window.location.href = `http://${ipAddress}/`; // Adjust the URL path as necessary
                } else {
                    alert("IP address not found for EPD_ID: " + epdId);
                }

                // Close the options dropdown after selection
                document.getElementById('optionsDropdown-' + epdId).style.display = 'none';
            }


            // Function to toggle dropdown visibility
            function toggleOptions(event, epdId) {
                event.stopPropagation(); // Prevent click event from bubbling up

                // Hide all other dropdowns
                const allDropdowns = document.querySelectorAll('.options-dropdown');
                allDropdowns.forEach(dropdown => {
                    if (dropdown.id !== 'optionsDropdown-' + epdId) {
                        dropdown.style.display = 'none'; // Hide others
                    }
                });

                // Toggle the clicked dropdown visibility
                const dropdown = document.getElementById('optionsDropdown-' + epdId);
                dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
            }

            // Function to enable or disable the Upload Image option
            function updateUploadImageOption(epdId, status) {
                const uploadOption = document.getElementById('uploadImageOption-' + epdId);
                if (status === 'TEXT') {
                    uploadOption.classList.add('disabled-option');
                    uploadOption.onclick = null; // Disable click event
                } else {
                    uploadOption.classList.remove('disabled-option');
                    const ipAddress = uploadOption.getAttribute('data-ipaddress');
                    uploadOption.onclick = function () {
                        uploadImage(epdId, ipAddress); // Ensure ipAddress is passed here
                    }; // Enable click event
                }
            }


            // Call this function on page load to initialize the options based on the current state
            document.querySelectorAll('.upload-image-option').forEach(option => {
                const epdId = option.id.split('-')[1];
                const status = option.getAttribute('data-status');
                updateUploadImageOption(epdId, status);
            });




        </script>
    </form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
