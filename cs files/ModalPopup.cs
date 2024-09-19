using System.Web.UI;

namespace WebApplication1.cs_files
{
    public class ModalPopup
    {
        //use this is page if doesnt need registering
        public static void ShowMessage(Page page, string message, string title = "Notification", bool showConfirmButton = false)
        {
            string confirmButtonHtml = showConfirmButton ?
                "<button type='button' class='btn btn-primary' id='confirmButton'>Confirm</button>" : "";

            string modalScript = $@"
                <div class='modal fade' id='customModal' tabindex='-1' aria-labelledby='customModalLabel' aria-hidden='true'>
                    <div class='modal-dialog'>
                        <div class='modal-content'>
                            <div class='modal-header'>
                                <h5 class='modal-title' id='customModalLabel'>{title}</h5>
                                <button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button>
                            </div>
                            <div class='modal-body'>
                                {message}
                            </div>
                            <div class='modal-footer'>
                                {confirmButtonHtml}
                                <button type='button' class='btn btn-secondary' data-bs-dismiss='modal'>OK</button>
                            </div>
                        </div>
                    </div>
                </div>
                <script type='text/javascript'>
                    $(document).ready(function () {{
                        $('#customModal').modal('show');
                        $('#confirmButton').click(function () {{
                            $('#hiddenFieldConfirmation').val('Confirmed');
                            $('#customModal').modal('hide');
                        }});
                    }});
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {{
                        $('#customModal').modal('show');
                        $('#confirmButton').click(function () {{
                            $('#hiddenFieldConfirmation').val('Confirmed');
                            $('#customModal').modal('hide');
                        }});
                    }});
                 </script>";

            // Register the script to ensure it's executed on postback
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ModalPopupScript", modalScript, false);
        }

        // Method to register the modal HTML into the page
        public static void RegisterModalHtml(Page page)
        {
            string modalHtml = @"
            <div class='modal fade' id='customModal' tabindex='-1' aria-labelledby='customModalLabel' aria-hidden='true'>
                <div class='modal-dialog'>
                    <div class='modal-content'>
                        <div class='modal-header'>
                            <h5 class='modal-title' id='customModalLabel'>Notification</h5>
                            <button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button>
                        </div>
                        <div class='modal-body'>
                            This is a reusable modal message.
                        </div>
                        <div class='modal-footer'>
                            <button type='button' class='btn btn-secondary' data-bs-dismiss='modal'>Close</button>
                            <button type='button' class='btn btn-primary' id='confirmButton' style='display: none;'>Confirm</button>
                        </div>
                    </div>
                </div>
            </div>";

            // Inject the modal HTML into the page
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ModalPopupHtml", modalHtml, false);
        }

        // Method to call the modal
        public static void ShowMessage_(Page page, string message, string title = "Notification", bool showConfirmButton = false)
        {
            string script = $@"
                $('#customModalLabel').text('{title}');
                $('#customModal .modal-body').text('{message}');

                // Show or hide the confirm button
                if ({showConfirmButton.ToString().ToLower()}) {{
                    $('#confirmButton').show();
                }} else {{
                    $('#confirmButton').hide();
                }}

                // Add a click event listener to the confirm button
                $('#confirmButton').off('click').on('click', function() {{
                    // Hide the current modal
                    $('#customModal').modal('hide');
                }});

                // Show the modal
                $('#customModal').modal('show');"
            ;

            // Register the script to show the modal
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ShowModalScript", script, true);


        }

    }
}