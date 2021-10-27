var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {


    $("#lstRequestStatus").chosen();

    $("#lstRequestStatus").change(function () {
        EMRRequestsList.ajax.reload();
    });

    //list emrrequests
    var EMRRequestsList = $("#tblEMRRequests").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "EMRRequestID",
        ajax: {
            url: "/EMRRequest/CompletedEMRRequestListJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                let vals = $("#lstRequestStatus").val().map(function (el, i) {
                    return el;
                });

                let status = "";
                for (let i = 0; i < vals.length; i++) {
                    status += vals[i] + ",";
                }

                if (status != "") {
                    status = status.substring(0, status.length - 1)
                }

                d.requestStatus = status;
            }
        },
        columns: [
            { data: "RequestedDate" },
            { data: "StatusName" },
            { data: "AccountID" },
            { data: "AccountName" },
            { data: "VendorName" },
            { data: "ConnectionType" },
            { data: "RequestedBy" },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-details'>Details</button>"
            }
        ]
    });

    //binding event to button Edit and Details
    EMRRequestsList.on('draw', function () {
        $("#tblEMRRequests button.btn-details").click(function (event) {
            let EMRRequestID = $(event.target).closest("tr").attr("id");
            $("#frmEMRRequestDetailsModalLabel").text("Details");
            fillDetailsForm(EMRRequestID);
        });
    });

    function fillDetailsForm(EMRRequestID) {

        $.ajax({
            method: "POST",
            url: "/EMRRequest/GetEMRRequest",
            dataType: "JSON",
            data: { EMRRequestID: EMRRequestID },
            success: function (data) {
                FillDetailsForm(data);
                $("#frmEMRRequestDetails").modal("show");
            }
        });
    }

    //filling edit form
    function FillDetailsForm(data) {

        $("#lblOfficeManagerName").text(data.OfficeManagerName);
        $("#lblOfficeManagerEmail").text(data.lblOfficeManagerEmail)
        $("#lblOfficeManagerPhone").text(data.OfficeManagerPhone);
        $("#lblPhysicianName").text(data.PhysicianName);
        $("#lblPhysicianSpecialty").text(data.PhysicianSpecialty);
        $("#lblExpectedSpecimenCount").text(data.ExpectedSpecimenCount);
        $("#lblCustomPanels").text(data.CustomPanels);
        $("#lblVendor").text(data.VendorName);
        $("#lblVendorContact").text(data.VendorContact);
        $("#lblVendorPhone").text(data.VendorPhone);
        $("#lblVendorFax").text(data.VendorFax);
        $("#lblVendorEmail").text(data.VendorEmail);
        $("#lblConnectionType").text(data.ConnectionType);
        $("#lblRequestedBy").text(data.RequestedBy);
        $("#blRequestedDate").text(data.RequestedDate);
        $("#lblVendorInstallationCost").text(data.VendorInstallationCost);
        $("#lblVendorMaintenanceCost").text(data.VendorMaintenanceCost);
        $("#lblVendorAnnualCost").text(data.VendorAnnualCost);
        $("#lblComtronInstallationCost").text(data.ComtronInstallationCost);
        $("lblRequestStatus").text(data.RequestStatus);
        $("#lblDenialReason").text(data.DenialReason);
        $("#chkPOSent").text(data.POSent);
        $("#chkLISRequestSent").text(data.LISRequestSent);
        $("#lblProjectedLiveDate").text(data.ProjectedLiveDate);
        $("#lblActualLiveDate").text(data.ActualLiveDate);
        $("#lblComtronTerminationDate").text(data.ComtronTerminationDate);
        $("#lblVendorTerminationDate").text(data.VendorTerminationDate);
        $("#lblAccountID").text(data.AccountID);

    }

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});