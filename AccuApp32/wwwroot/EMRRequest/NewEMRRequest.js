var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {


    $("#lstRequestStatus").chosen();

    $("#lstRequestStatus").change(function () {
        EMRRequestsList.ajax.reload();
    });

    $("#lstHasCustomPanels").change(function () {
        if ($(this).val() == "true") {
            $("#txtCustomPanels").parent().css("display", "block");
        } else {
            $("#txtCustomPanels").parent().css("display", "none");
        }
    });

    $("#lstVendorID").change(function () {
        if ($(this).val() == "-1") {

            var newVendorName = prompt("Enter Name new vendor");
            if (newVendorName != null && newVendorName != "" && newVendorName.toUpperCase()!="NEW VENDOR") {

                $.ajax({
                    method: "POST",
                    url: "/EMRRequest/AddNewVendor",
                    dataType: "JSON",
                    data: { newVendorName: newVendorName },
                    success: function (data) {
                        if (data.message == "OK") {

                            $("#lstVendorID").append(`<option value="${data.ID}">${data.Name}</option>`);
                            $("#lstVendorID").val(data.ID);

                            alert(`New vendor ${data.Name}  added sucessfull!`);

                        } else {

                            alert(`New vendor ${data.Name}  added unsuccessfull!`);

                        }
                    }
                });

            }  
        }
    });


    //list emrrequests
    var EMRRequestsList = $("#tblEMRRequests").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "EMRRequestID",
        ajax: {
            url: "/EMRRequest/NewEMRRequestListJson",
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
            },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-edit'>Edit</button>"
            },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-delete'>Delete</button>"
            }
        ]
    });

    //binding event to button Edit and Details
    EMRRequestsList.on('draw', function () {
        $("#tblEMRRequests button.btn-edit").click(function (event) {
            let EMRRequestID = $(event.target).closest("tr").attr("id");
            $("#frmEMRRequestModalLabel").text("Edit");
            fillEditForm(EMRRequestID);
        });
        $("#tblEMRRequests button.btn-delete").click(function (event) {
            var confirmDelete = confirm("Confirm delete?");
            if (!confirmDelete)
                return false;
            let EMRRequestID = $(event.target).closest("tr").attr("id");

            $.ajax({
                method: "POST",
                url: "/EMRRequest/DeleteEMRRequest",
                dataType: "JSON",
                data: { EMRRequestID: EMRRequestID },
                success: function (data) {
                    EMRRequestsList.ajax.reload();
                }
            });
        });
        $("#tblEMRRequests button.btn-details").click(function (event) {
            let EMRRequestID = $(event.target).closest("tr").attr("id");
            $("#frmEMRRequestDetailsModalLabel").text("Details");
            fillDetailsForm(EMRRequestID);
        });
    });

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        FormControlsEnable();
        FormClear();

        $("#txtStatus").val(7);//new status

        $("#frmEMRRequestModalLabel").text("Create");
        $("#frmEMRRequest").modal("show");
    });

    //filling edit form
    function fillEditForm(EMRRequestID) {

        FormClear();

        $.ajax({
            method: "POST",
            url: "/EMRRequest/GetEMRRequest",
            dataType: "JSON",
            data: { EMRRequestID: EMRRequestID },
            success: function (data) {
                FillForm(data);
                $("#frmEMRRequest").modal("show");
            }
        });

    }

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
    function FillForm(data) {

        FormControlsEnable();

        $("#txtStatus").val(data.Status);
        $("#txtEmrRequestID").val(data.EMRRequestID);
        $("#txtRequestedBy").val(data.RequestedBy);
        $("#txtRequestedDate").val(data.RequestedDate);

        $("#lstAccountID").val(data.AccountID);
        $("#txtOfficeManagerName").val(data.OfficeManagerName);
        $("#txtOfficeManagerEmail").val(data.OfficeManagerEmail);
        $("#txtOfficeManagerPhone").val(data.OfficeManagerPhone);
        $("#txtPhysicianName").val(data.PhysicianName);
        $("#txtPhysicianSpecialty").val(data.PhysicianSpecialty);
        $("#txtExpectedSpecimenCount").val(data.ExpectedSpecimenCount);
        $("#lstHasCustomPanels").val(data.HasCustomPanels.toString());
        $("#txtCustomPanels").val(data.CustomPanels);
        $("#lstVendorID").val(data.VendorID);
        $("#txtVendorContact").val(data.VendorContact);
        $("#txtVendorPhone").val(data.VendorPhone);
        $("#txtVendorFax").val(data.VendorFax);
        $("#txtVendorEmail").val(data.VendorEmail);
        $("#lstConnectionType").val(data.ConnectionType);
    }

    //filling details form
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

 
    function FormControlsDisable() {

        $.each($("#frmEMRRequest input"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $.each($("#frmEMRRequest textarea"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $.each($("#frmEMRRequest select"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $("#btnSave").prop("disabled", true);

    }

    function FormControlsEnable() {

        $.each($("#frmEMRRequest form input"), function (index, item) {
            $(item).prop("disabled", false);
        });

        $.each($("#frmEMRRequest form textarea"), function (index, item) {
            $(item).prop("disabled", false);
        });

        $.each($("#frmEMRRequest form select"), function (index, item) {
            $(item).prop("disabled", false);
        });

        $("#btnSave").prop("disabled", false);

    }


    //clear control element
    function FormClear() {

        $.each($("#frmEMRRequest form input"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmEMRRequest form textarea"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmEMRRequest form select"), function (index, item) {
            $(item).val("");
        });

    }

    //button Ok modal form
    $("#btnSave").click(function () {

        //let physIds = $("#tblPhysician tr.phys-row").map(function () {
        //    return $(this).attr("data-id");
        //}).get();

        let EmrRequestID = $("#txtEmrRequestID").val();
        let Status = $("#txtStatus").val();
        let AccountID = $("#lstAccountID").val();
        let VendorID = $("#lstVendorID").val();

        let OfficeManagerName=$("#txtOfficeManagerName").val();
        let OfficeManagerEmail=$("#txtOfficeManagerEmail").val();
        let OfficeManagerPhone=$("#txtOfficeManagerPhone").val();
        let PhysicianName=$("#txtPhysicianName").val();
        let PhysicianSpecialty=$("#txtPhysicianSpecialty").val();
        let ExpectedSpecimenCount=$("#txtExpectedSpecimenCount").val();
        let HasCustomPanels=$("#lstHasCustomPanels").val()=="true";
        let CustomPanels=$("#txtCustomPanels").val();
        let VendorContact=$("#txtVendorContact").val();
        let VendorPhone=$("#txtVendorPhone").val();
        let VendorFax=$("#txtVendorFax").val();
        let VendorEmail=$("#txtVendorEmail").val();
        let ConnectionType = $("#lstConnectionType").val();

        $.ajax({
            url: "/EMRRequest/NewSaveEMRRequest",
            method: "POST",
            dataType: "JSON",
            data: {
                EmrRequestID: EmrRequestID,
                Status: Status,
                AccountID: AccountID,
                VendorID: VendorID,
                OfficeManagerName: OfficeManagerName,
                OfficeManagerEmail: OfficeManagerEmail,
                OfficeManagerPhone: OfficeManagerPhone,
                PhysicianName: PhysicianName,
                PhysicianSpecialty: PhysicianSpecialty,
                ExpectedSpecimenCount: ExpectedSpecimenCount,
                HasCustomPanels: HasCustomPanels,
                CustomPanels: CustomPanels,
                VendorContact: VendorContact,
                VendorPhone: VendorPhone,
                VendorFax: VendorFax,
                VendorEmail: VendorEmail,
                ConnectionType: ConnectionType
            },
            success: function (data) {
                if (data == "OK") {
                    EMRRequestsList.ajax.reload();
                    $("#frmEMRRequest").modal("hide");
                    FormClear();
                }
            }
        });
    });

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});