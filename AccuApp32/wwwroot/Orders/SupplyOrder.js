var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {


    $("#lstRequestStatus").chosen();

    $("#lstRequestStatus").change(function () {
        supplyOrdersList.ajax.reload();
    });

    $("#lstHasCustomPanels").change(function () {
        if ($(this).val() == "true") {
            $("#txtCustomPanels").parent().css("display", "block");
        } else {
            $("#txtCustomPanels").parent().css("display", "none");
        }
    });


    //list emrrequests
    var supplyOrdersList = $("#tblSupplyOrders").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "SalesOrderID",
        ajax: {
            url: "/Order/SupplyOrdersJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.requestStatus = $("#lstRequestStatus").val()
            }
        },
        order: [[2, "desc"]],
        columns: [
            {
                data: null,
                sortable: false,
                defaultContent: "<input class='order-row' type='checkbox' />"
            },
            { data: "SalesOrderID" },
            { data: "DateCreated" },
            { data: "DatePrinted" },
            { data: "DateShipped" },
            { data: "AccountID" },
            { data: "TrackingNumber" },
            { data: "DeliveryMethodName" },
            { data: "StatusName" },
            { data: "CreatedBy" },
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
            },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-print'>Print This</button>"
            },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-approve'>Approve</button>"
            }
        ]
    });

    //binding event to button Edit and Details
    supplyOrdersList.on('draw', function () {
        $("#tblSupplyOrders button.btn-edit").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmSupplyOrdersModalLabel").text("Edit");
            fillEditForm(SalesOrderID);
        });
        $("#tblSupplyOrders button.btn-details").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmSupplyOrdersDetailsModalLabel").text("Details");
            fillDetailsForm(SalesOrderID);
        });
        $("#tblSupplyOrders button.btn-print").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmEMRRequestModalLabel").text("Details");
            fillDetailsForm(SalesOrderID);
        });
        $("#tblSupplyOrders button.btn-approve").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmEMRRequestModalLabel").text("Details");
            fillDetailsForm(SalesOrderID);
        });
    });

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        FormControlsEnable();
        FormClear();

        $("#lstStatus").val(7);
        $("#lstStatus").prop("disabled", "true");

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
                FillForm(data);
                FormControlsDisable();
                $("#frmEMRRequest").modal("show");
            }
        });
    }

    //filling edit form
    function FillForm(data) {

        FormControlsEnable();

        $("#lstStatus").val(data.Status);
        $("#lstAccountID").val(data.AccountID);
        $("#txtEmrRequestID").val(data.EMRRequestID);
        $("#txtVendorName").val(data.VendorName);
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
        $("#txtRequestedBy").val(data.RequestedBy);
        $("#txtRequestStatus").val(data.Status);
        $("#txtRequestedDate").val(data.RequestedDate);
        $("#lstConnectionType").val(data.ConnectionType);
        $("#txtProjectedLiveDate").val(moment(data.ProjectedLiveDate).format("YYYY-MMM-DD hh:mm:ss"));
        $("#txtActualLiveDate").val(moment(data.ActualLiveDate).format("YYYY-MMM-DD hh:mm:ss"));
        $("#ComtronTerminationDate").val(moment(data.ComtronTerminationDate).format("YYYY-MMM-DD hh:mm:ss"));
        $("#VendorTerminationDate").val(moment(data.VendorTerminationDate).format("YYYY-MMM-DD hh:mm:ss"));

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

        let physIds = $("#tblPhysician tr.phys-row").map(function () {
            return $(this).attr("data-id");
        }).get();

        let physNames = $("#tblPhysician tr.phys-row").map(function () {
            return $(this).find("input.phys-name").val();
        }).get();

        let physNPIs = $("#tblPhysician tr.phys-row").map(function () {
            return $(this).find("input.phys-npi").val();
        }).get();


        let EmrRequestID = $("#EmrRequestID").val();
        let Status = $("#lstStatus").val();
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
        let ProjectedLiveDate = $("#txtProjectedLiveDate").val();
        let ActualLiveDate = $("#txtActualLiveDate").val();
        let ComtronTerminationDate = $("#ComtronTerminationDate").val();
        let VendorTerminationDate = $("#VendorTerminationDate").val();
        let ActionTaken = $("#txtActionTaken").val();
        let Approve = $("#chkApprove").prop("checked");


        $.ajax({
            url: "/EMRRequest/SaveEMRRequest",
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
                ConnectionType: ConnectionType,
                ActionTaken: ActionTaken,
                ProjectedLiveDate: ProjectedLiveDate,
                ActualLiveDate: ActualLiveDate,
                ComtronTerminationDate: ComtronTerminationDate,
                VendorTerminationDate: VendorTerminationDate,
                Approve: Approve
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