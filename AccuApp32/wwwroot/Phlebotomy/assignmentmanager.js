var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {

    //list of phlebotomistAssignment list
    var phlebotomistAssigmnemtList = $("#tblAssignments").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "PhlebotomistAssignmentID",
        ajax: {
            url: "/Phlebotomy/AssignmentManagmentJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) { d.status = $("#selectConfirmed").val() }
        },
        columns: [
            {
                data: "Confirmed",
                render: function (data, type, row, meta) {
                    return data ? "<input type='checkbox' checked readonly disabled >" : "<input type='checkbox' readonly disabled>";
                }
            },
            { data: "EmployeeID" },
            { data: "FirstName" },
            { data: "LastName" },
            { data: "State" },
            { data: "Telephone" },
            { data: "Month" },
            { data: "Year" },
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
    phlebotomistAssigmnemtList.on('draw', function () {
        $("#tblAssignments button.btn-edit").click(function (event) {
            let phlebotomistAssignmentID = $(event.target).closest("tr").attr("id");
            $("#frmAssignmentManagerModalLabel").text("Edit");
            fillEditForm(phlebotomistAssignmentID);
        });
        $("#tblAssignments button.btn-details").click(function (event) {
            let phlebotomistAssignmentID = $(event.target).closest("tr").attr("id");
            $("#frmAssignmentManagerModalLabel").text("Details");
            fillDetailsForm(phlebotomistAssignmentID);
        });
        $("#tblAssignments button.btn-delete").click(function (event) {

            if (!confirm("do you confirm the deletion?"))
                return false;

            let phlebotomistAssignmentID = $(event.target).closest("tr").attr("id");

            $.ajax({
                method: "POST",
                url: "/Phlebotomy/DeletePhlebotomyAssignment",
                dataType: "JSON",
                data: { phlebotomistAssignmentID: phlebotomistAssignmentID },
                success: function (data) {
                    phlebotomistAssigmnemtList.ajax.reload();
                }
            });

        });
    });

    //binding event to dropdown Confirmed/UnConfirmed
    $("#selectConfirmed").change(function () {
        phlebotomistAssigmnemtList.ajax.reload();
    });

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        frmFormControlsEnable();
        frmCreateFormClear();
        $("#frmAssignmentManagerModalLabel").text("Create");
        $("#frmAssignmentManager").modal("show");
    });

    //filling edit form
    function fillEditForm(phlebotomistAssignmentID) {
        $.ajax({
            method: "POST",
            url: "/Phlebotomy/GetPhlebotomyAssignment",
            dataType: "JSON",
            data: { phlebotomistAssignmentID: phlebotomistAssignmentID },
            success: function (data) {
                FillForm(data);
                $("#frmAssignmentManager").modal("show");
            }
        });
    }

    function fillDetailsForm(phlebotomistAssignmentID) {
        $.ajax({
            method: "POST",
            url: "/Phlebotomy/GetPhlebotomyAssignment",
            dataType: "JSON",
            data: { phlebotomistAssignmentID: phlebotomistAssignmentID },
            success: function (data) {
                FillDetailsForm(data);
                $("#frmAssignmentManager").modal("show");
            }
        });
    }

    //filling edit form
    function FillForm(data) {

        frmFormControlsEnable();

        $("#txtEmployeeSearch").val(data.EmployeeID);
        $("#txtFirstName").val(data.FirstName);
        $("#txtLastName").val(data.LastName);
        $("#txtState").val(data.State);
        $("#txtTelephone").val(data.Telephone);
        $("#chkConfirmed").prop("checked", data.Confirmed);
        $("#txtLastModifiedBy").val(data.LastModifiedBy);
        $("#txtLastModifiedDate").val(data.LastModifiedDate);
        $("#selectMonth").val(data.Month);
        $("#selectYear").val(data.Year);


        $("#tblDistribution tbody>tr").remove();

        for (let i = 0; i < data.PhlebotomyAssignmentLines.length; i++) {
            let row = data.PhlebotomyAssignmentLines[i];
            $("#tblDistribution tbody").append("<tr type='" + row.type + "'><td><input type='text' class='form-control percent' value='" + row.Percentage + "' /></td><td><input type='search' class='btn btn-light border search' placeholder='search string/press Enter' value='" + row.ID + "'><select class='form-control' style='display:none;'></select></td><td><label class='name' >" + row.Name + "</label></td><td><label class='salesrep'>" + row.SalesRep + "</label></td><td><button type='button' class='btn btn-danger remove'>Remove</button></td></tr>");
            BindEventDetailRow(row.type);
        }
    }
    //filling details form
    function FillDetailsForm(data) {

        $("#txtEmployeeSearch").val(data.EmployeeID);
        $("#txtFirstName").val(data.FirstName);
        $("#txtLastName").val(data.LastName);
        $("#txtState").val(data.State);
        $("#txtTelephone").val(data.Telephone);
        $("#chkConfirmed").prop("checked", data.Confirmed);
        $("#txtLastModifiedBy").val(data.LastModifiedBy);
        $("#txtLastModifiedDate").val(data.LastModifiedDate);
        $("#selectMonth").val(data.Month);
        $("#selectYear").val(data.Year);

        $("#tblDistribution tbody>tr").remove();

        for (let i = 0; i < data.PhlebotomyAssignmentLines.length; i++) {
            let row = data.PhlebotomyAssignmentLines[i];
            $("#tblDistribution tbody").append("<tr type='" + row.type + "'><td><input type='text' class='form-control percent' value='" + row.Percentage + "' /></td><td><input type='search' class='btn btn-light border search' placeholder='search string/press Enter' value='" + row.ID + "'><select class='form-control' style='display:none;'></select></td><td><label class='name' >" + row.Name + "</label></td><td><label class='salesrep'>" + row.SalesRep + "</label></td><td><button type='button' class='btn btn-danger remove'>Remove</button></td></tr>");
        }

        frmFormControlsDisable();
    }

    function frmFormControlsDisable() {
        $("#txtEmployeeSearch").prop("disabled", true);
        $("#chkConfirmed").prop("disabled",true);
        $("#selectMonth").prop("disabled",true);
        $("#selectYear").prop("disabled", true);
        $("#btnAddAccountDistribution").prop("disabled", true)
        $("#btnAddGroupDistribution").prop("disabled", true)
        $("#btnCreateOk").prop("disabled", true)


        $("#tblDistribution input").prop("disabled", true);
        $("#tblDistribution button").prop("disabled", true);

    }

    function frmFormControlsEnable() {
        $("#txtEmployeeSearch").prop("disabled", false);
        $("#chkConfirmed").prop("disabled", false);
        $("#selectMonth").prop("disabled", false);
        $("#selectYear").prop("disabled", false);
        $("#btnAddAccountDistribution").prop("disabled", false)
        $("#btnAddGroupDistribution").prop("disabled", false)
        $("#btnCreateOk").prop("disabled", false)


        $("#tblDistribution input").prop("disabled", false);
        $("#tblDistribution button").prop("disabled", false);

    }


    //clear control element
    function frmCreateFormClear() {

        $("#txtEmployeeSearch").val("");
        $("#txtFirstName").val("");
        $("#txtLastName").val("");
        $("#txtState").val("");
        $("#txtTelephone").val("");
        $("#chkConfirmed").prop("checked", false);
        $("#txtLastModifiedBy").val("");
        $("#txtLastModifiedDate").val("");
        $("#selectMonth").val("");
        $("#selectYear").val("");

        $("#selectEmployeeID option").remove();
        $("#tblDistribution tbody tr").remove();
    }

   
    //modal form event
    //binding event 
    $("#txtEmployeeSearch").keypress(function (event) {
        if (event.keyCode != "13")
            return;
        if ($("#txtEmployeeSearch").val() == "")
            return;

        $.ajax({
            method: "POST",
            url: "/Phlebotomy/GetEmployeeListJson",
            dataType: "JSON",
            data: { searchValue: $("#txtEmployeeSearch").val() },
            success: function (data) {

                responseEmployeeData = data;
                $("#selectEmployeeID option").remove();
                $("#selectEmployeeID").append("<option value=''>--</option>");
                for (let i = 0; i < data.length; i++) {
                    $("#selectEmployeeID").append("<option value='" + data[i].EmployeeID + "'>" + data[i].EmployeeID + " " + data[i].FirstName + " " + data[i].LastName + "</option>");
                }
                if (data.length > 0)
                    $("#selectEmployeeID").show();
            }
        })
    });
    //dropdown select
    $("#selectEmployeeID").change(function () {
        var employeeid = $(this).find("option:selected").val();
        let selectedItem = responseEmployeeData.find(function (item) {
            return item.EmployeeID == employeeid;
        });

        $("#txtEmployeeSearch").val(selectedItem.EmployeeID);
        $("#txtFirstName").val(selectedItem.FirstName);
        $("#txtLastName").val(selectedItem.LastName);
        $("#txtState").val(selectedItem.State);
        $("#txtTelephone").val(selectedItem.Telephone);

        $(this).hide();
    });
    //button Account Distribution
    $("#btnAddAccountDistribution").click(function () {
        $("#tblDistribution tbody").append("<tr type='account'><td><input type='text' class='form-control percent' /></td><td><input type='search' class='btn btn-light border search' placeholder='search string/press Enter'><select class='form-control' style='display:none;'></select></td><td><label class='name' ></label></td><td><label class='salesrep'></label></td><td><button type='button' class='btn btn-danger remove'>Remove</button></td></tr>");
        BindEventDetailRow("account");
    });

    //button Group distribution
    $("#btnAddGroupDistribution").click(function () {
        $("#tblDistribution tbody").append("<tr type='group'><td><input type='text' class='form-control percent' /></td><td><input type='search' class='btn btn-light border search' placeholder='search string/press Enter'><select class='form-control' style='display:none;'></select></td><td><label class='name' ></label></td><td><label class='salesrep'></label></td><td><button type='button' class='btn btn-danger remove'>Remove</button></td></tr>");
        BindEventDetailRow("group");
    });
    //bind event to detail row 
    function BindEventDetailRow(type) {
        $("#tblDistribution tbody tr:last button.remove").click(function (event) {
            $(event.target).closest("tr").remove();
        });

        $("#tblDistribution tbody tr:last select").change(function (event) {
            var id = $(this).find("option:selected").val();
            let selectItem = responseData.find(function (item) {
                return item.ID == id;
            });


            let txtID = $(this).closest("tr").find("input.search");
            let txtName = $(this).closest("tr").find("label.name");
            let txtSalesRep = $(this).closest("tr").find("label.salesrep");

            txtID.val(selectItem.ID);
            txtName.text(selectItem.Name);
            txtSalesRep.text(selectItem.SalesRep);

            $(this).hide();
        });

        $("#tblDistribution tbody tr:last input.search").keypress(function (event) {
            if (event.keyCode != "13")
                return;
            var search = $(event.target).val();
            var dropdown = $(event.target).closest("tr").find("select");
            $.ajax({
                url: "/Phlebotomy/GetAccountsGroupJson",
                method: "POST",
                dataType: "JSON",
                data: { type:type,searchValue: search },
                success: function (data) {
                    $(dropdown).find("option").remove();
                    $(dropdown).append("<option value=''>--</option>");
                    responseData = data;
                    for (let i = 0; i < data.length; i++) {
                        $(dropdown).append("<option value='" + data[i].ID + "'>" + data[i].ID + " " + data[i].Name + "</option>");
                    }
                    if (data.length > 0)
                        $(dropdown).show();
                }
            });
        });
    }
    //button Ok modal form
    $("#btnCreateOk").click(function () {

        //preparing date

        let employeeID = $("#txtEmployeeSearch").val();
        let month = $("#selectMonth").val();

        let percent = $("#tblDistribution input.percent").map(function () {
            return $(this).val();
        }).get();

        let IDs = $("#tblDistribution input.search").map(function () {
            return $(this).val();
        }).get();

        let typs= $("#tblDistribution tbody tr").map(function () {
            return $(this).attr("type");
        }).get();


        $.ajax({
            url: "/Phlebotomy/SaveAssignment",
            method: "POST",
            dataType: "JSON",
            data: { employeeID: employeeID,month:month,percent:percent,ids:IDs,typs:typs},
            success: function (data) {
                if (data == "OK") {
                    $("#frmAssignmentManager").modal("hide");
                    frmCreateFormClear();
                }
            }
        });
    });

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});