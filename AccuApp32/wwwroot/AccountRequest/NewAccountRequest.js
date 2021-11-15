var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {

    $("#lstResultDeliveryOption").chosen();
    $("#lstResultDeliveryOption_chosen").css("width", "200px");

    $("#btnAddPhysician").click(function () {
        $("#tblPhysician tbody").after("<tr data-id='' class='phys-row'><td><input type='checkbox' /></td><td><input class='form-control phys-name' /></td><td><input class='form-control phys-npi' /></td></tr>");
        return false;
    });

    $("#btnDeletePhysician").click(function () {

        $.each($("#tblPhysician input[type=checkbox]:checked"), function (index, item) {
            $(item).parent().parent().remove();
        });

        return false;
    });

    $("#lstRequestType").change(function () {
        if ($(this).val() == "2") {
            $("#txtExistingAccountID").parent().css("display", "block");
        } else {
            $("#txtExistingAccountID").parent().css("display", "none");
        }
    });

    $("#lstNeedPgxPortal").change(function () {
        if ($(this).val() == "true") {
            $("#txtPhysicianEmail").parent().parent().css("display", "block");
        } else {
            $("#txtPhysicianEmail").parent().parent().css("display", "none");
        }
    });

    $("#lstResultDeliveryOption").change(function () {

        let v = $(this).val();

        if (v.filter(t => t == "2").length > 0) {
            $("#txtEMRVendor").parent().css("display", "block");
        } else {
            $("#txtEMRVendor").parent().css("display", "none");
        }
        if (v.filter(t => t == "1").length > 0) {
            $("#lstAuto").parent().css("display", "block");
        } else {
            $("#lstAuto").parent().css("display", "none");
        }

    });

    //list accountrequests
    var accountrequestsList = $("#tblAccountRequests").DataTable({
        serverSide: true,
        processing: true,
        rowId: "AccountRequestID",
        ajax: {
            url: "/AccountRequest/NewAccountRequestsJson",
            type: "POST",
            dataSrc: "data"
        },
        columns: [
            { data: "RequestDate" },
            { data: "ClientType" },
            { data: "PracticeName" },
            { data: "Address" },
            { data: "Suite" },
            { data: "City" },
            { data: "State" },
            { data: "Zip" },
            { data: "Phone" },
            { data: "OfficeContactName" },
            { data: "ContactEmail" },
            { data: "GroupID" },
            { data: "SalesRep" },
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
        ]
    });

    //binding event to button Edit and Details
    accountrequestsList.on('draw', function () {
        $("#tblAccountRequests button.btn-edit").click(function (event) {
            let accountRequestID = $(event.target).closest("tr").attr("id");
            $("#frmAccountRequestModalLabel").text("Edit");
            fillEditForm(accountRequestID);
            FormControlsEnable();
        });
        $("#tblAccountRequests button.btn-details").click(function (event) {
            let accountRequestID = $(event.target).closest("tr").attr("id");
            $("#frmAccountRequestModalLabel").text("Details");
            fillEditForm(accountRequestID);
            FormControlsDisable();
        });
    });

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        FormClear();
        FormControlsEnable();
        $("#frmAccountRequestModalLabel").text("Create");
        $("#lstStatus").prop("disabled", "true");
        $("#frmAccountRequest").modal("show");
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

    function FormControlsEnable() {

        $.each($("#frmAccountRequest form input"), function (index, item) {
            $(item).prop("disabled", false);
        });

        $.each($("#frmAccountRequest form textarea"), function (index, item) {
            $(item).prop("disabled", false);
        });

        $.each($("#frmAccountRequest form select"), function (index, item) {
            $(item).prop("disabled", false);
        });

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


        let AccountRequestID=$("#txtAccountRequestId").val();
        let GroupID=$("#lstGroup").val();
        let RequestType=$("#lstRequestType").val();
        let ExistingAccountID=$("#txtExistingAccountID").val();
        let ClientType=$("#lstClientType").val();

        let PracticeName=$("#txtPracticeName").val();
        let PracticeNPI=$("#txtPracticeNPI").val();

        let ResultDeliveryOption= $("#lstResultDeliveryOption").val();
        let EMRVendor=$("#txtEMRVendor").val();
        let Auto=$("#lstAuto").val();
        let Address=$("#txtAddress").val();
        let Suite=$("#txtSuite").val();
        let City=$("#txtCity").val();
        let State=$("#txtState").val();
        let Zip=$("#txtZip").val();
        let Phone=$("#txtPhone").val();
        let Fax=$("#txtFax").val();
        let OfficeContactName=$("#txtOfficeContactName").val();
        let ContactEmail=$("#txtContactEmail").val();
        let ContactEmail1=$("#txtContactEmail1").val();
        let SuppliesNeeded=$("#chkSuppliesNeeded").prop("checked");
        let NeedPgxPortal=$("#lstNeedPgxPortal").val();
        let PhysicianEmail=$("#txtPhysicianEmail").val();
        let Status = $("#lstStatus").val();


        $.ajax({
            url: "/AccountRequest/SaveAccountRequest",
            method: "POST",
            dataType: "JSON",
            data: {
                AccountRequestID: AccountRequestID,
                GroupID: GroupID,
                RequestType: RequestType,
                ExistingAccountID: ExistingAccountID,
                ClientType: ClientType,
                PracticeName: PracticeName,
                PracticeNPI: PracticeNPI,
                ResultDeliveryOption: ResultDeliveryOption,
                EMRVendor: EMRVendor,
                Auto: Auto,
                Address: Address,
                Suite: Suite,
                City: City,
                State: State,
                Zip: Zip,
                Phone: Phone,
                Fax: Fax,
                OfficeContactName: OfficeContactName,
                ContactEmail: ContactEmail,
                ContactEmail1: ContactEmail1,
                SuppliesNeeded: SuppliesNeeded,
                NeedPgxPortal: NeedPgxPortal,
                PhysicianEmail: PhysicianEmail,
                PhysIDs: physIds,
                PhysNames: physNames,
                PhysNPIs: physNPIs,
                Status:Status
            },
            success: function (data) {
                if (data == "OK") {
                    accountrequestsList.ajax.reload();
                    $("#frmAccountRequest").modal("hide");
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