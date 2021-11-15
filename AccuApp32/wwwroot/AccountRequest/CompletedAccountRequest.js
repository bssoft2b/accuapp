var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {

    $("#lstResultDeliveryOption").chosen();
    $("#lstResultDeliveryOption_chosen").css("width", "200px");

    //list accountrequests
    var accountrequestsList = $("#tblAccountRequests").DataTable({
        serverSide: true,
        processing: true,
        rowId: "AccountRequestID",
        ajax: {
            url: "/AccountRequest/CompletedAccountRequestsJson",
            type: "POST",
            dataSrc: "data"
        },
        columns: [
            { data: "PracticeName" },
            { data: "ClientType" },
            { data: "GroupID" },
            { data: "SalesRep" },
            { data: "RequestedBy" },
            { data: "RequestDate" },
            { data: "RequestStatus" },
            { data: "ClientIDS" },
            { data: "CompletedBy" },
            { data: "CompletedDate" },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-details'>Details</button>"
            }
        ]
    });

    //binding event to button Edit and Details
    accountrequestsList.on('draw', function () {
        $("#tblAccountRequests button.btn-details").click(function (event) {
            let accountRequestID = $(event.target).closest("tr").attr("id");
            $("#frmAccountRequestModalLabel").text("Details");
            fillEditForm(accountRequestID);
            FormControlsDisable();
        });
    });

    //filling edit form
    function fillEditForm(accountRequestID) {

        FormClear();

        $.ajax({
            method: "POST",
            url: "/AccountRequest/GetAccountRequest",
            dataType: "JSON",
            data: { accountRequestID: accountRequestID },
            success: function (data) {
                FillForm(data);
                $("#frmAccountRequest").modal("show");
            }
        });

    }

    //filling edit form
    function FillForm(data) {

        let multiValue = [];

        if (data.ResultDeliveryOptionList.indexOf(",") > -1) {
            multiValue = data.ResultDeliveryOptionList.split(",");
        } else if (data.ResultDeliveryOptionList.indexOf(";") > -1) {
            multiValue = data.ResultDeliveryOptionList.split(";");
        } else {
            multiValue[0] = data.ResultDeliveryOptionList;
        }


        $("#txtAccountRequestId").val(data.AccountRequestID);
        $("#lstGroup").val(data.GroupID);
        $("#lstRequestType").val(data.RequestType);
        $("#txtExistingAccountID").val(data.ExistingAccountID);
        $("#lstClientType").val(data.ClientType);

        $("#txtPracticeName").val(data.PracticeName);
        $("#txtPracticeNPI").val(data.PracticeNPI);

        for (let i = 0; i < data.Physician.length; i++) {
            $("#tblPhysician tbody").after(`<tr data-id="${data.Physician[i].PhysicianRequestID}" class='phys-row'><td><input type='checkbox' /></td><td><input class='form-control phys-name' type='text' value='${data.Physician[i].PhysicianName}' /></td><td><input class='form-control phys-npi' type='text' value='${data.Physician[i].PhysicianNPI}' /></td></tr>`);
        }

        $("#lstResultDeliveryOption").val(multiValue).trigger("chosen:updated");
        $("#txtEMRVendor").val(data.EMRVendor);
        $("#lstAuto").val(data.Auto);
        $("#txtAddress").val(data.Address);
        $("#txtSuite").val(data.Suite);
        $("#txtCity").val(data.City);
        $("#txtState").val(data.State);
        $("#txtZip").val(data.Zip);
        $("#txtPhone").val(data.Phone);
        $("#txtFax").val(data.Fax);
        $("#txtOfficeContactName").val(data.OfficeContactName);
        $("#txtContactEmail").val(data.ContactEmail);
        $("#txtContactEmail1").val(data.ContactEmail1);
        $("#txtFax").val(data.Fax);
        $("#chkSuppliesNeeded").prop("checked",data.SuppliesNeeded);
        $("#lstNeedPgxPortal").val(data.NeedPgxPortal.toString());
        $("#txtPhysicianEmail").val(data.PhysicianEmail);
        $("#txtWebUserName").val(data.WebUserName);
        $("#txtWebUserPassword").val(data.WebUserPassword);
        $("#txtRequestedBy").val(data.RequestedBy);
        $("#txtRequestDate").val(data.RequestDate);
        $("#txtCompletedBy").val(data.CompletedBy);
        $("#txtCompletedDate").val(data.CompletedDate);
        $("#lstStatus").val(data.RequestStatus);
        if (data.NeedPgxPortal.toString() == "true") {
            $("#txtPhysicianEmail").parent().parent().css("display", "block");
        } else {
            $("#txtPhysicianEmail").parent().parent().css("display", "none");
        }

    }
 
    function FormControlsDisable() {

        $.each($("#frmAccountRequest input"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $.each($("#frmAccountRequest textarea"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $.each($("#frmAccountRequest select"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $.each($("#frmAccountRequest button"), function (index, item) {
            $(item).prop("disabled", true);
        });

        $("button.close").prop("disabled", false);
        $("button.btn-close").prop("disabled", false);
    }

    //clear control element
    function FormClear() {

        $.each($("#frmAccountRequest form input"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmAccountRequest form textarea"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmAccountRequest form select"), function (index, item) {
            $(item).val("");
        });


        $("#tblPhysician tr.phys-row").remove();

        $("#lstStatus").val(0);

    }

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});