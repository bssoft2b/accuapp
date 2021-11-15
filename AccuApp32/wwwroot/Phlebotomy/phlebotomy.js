var assignMentTable;
$(function () {

    function chkExistID(v) {
        if (!v.match("^[a-zA-Z0-9]*$")) {
            $("#msgError").text("Only Alphabets and Numbers allowed.");
        }
        else {
            let response = $.ajax({
                method: "GET",
                url: "/Phlebotomy/ExistsPhlebotomists",
                dataType: "JSON",
                data: { employeeId: v }
            });
            response.done(function (data) {
                if (data)
                    $("#msgError").text("This EmployeeID is exists.Choose another ID.");
                else
                    $("#msgError").text("");
            });
        }
    }

    $("#frmEmployer").validate({
        rules: {
            txtEmployeeID: {
                required: true,
            },
            txtAddress1: {
                required: true,
           },
            txtCity: {
                required: true
            },
            txtState: {
                required: true,
                minlength: 2,
                maxlength:2
            },
            txtZip: {
                required: true,
                minlength: 6,
                maxlength: 6
            },
            txtTelephone: {
                required: true
            },
            txtEmployeeStartDate: {
                required: true
            }
        },
        messages: {
            txtEmployeeID: {
                required:"Field required"
            }, 
            txtAddress1: {
                required: "Field required"
            },
            txtCity: {
                required: "Field required"
            },
            txtState: {
                required: "Field required",
                minlength: "Length is 2 symbols",
                maxlength: "Length is 2 symbols"
            },
            txtZip: {
                required: "Field required",
                minlength: "Length is 6 symbols",
                maxlength: "Length is 6 symbols"
            },
            txtEmployeeStartDate: {
                required: "Field required"
            },
            txtTelephone: {
                required: "Field required"
            }
        }
    });

    function setCheckEmployeeID() {
        $("#txtEmployeeID").unbind("change").on("change", function (event) {
            let v = $(event.target).val();
            chkExistID(v);
        });
    }

    $("#editPhlebotomist").on("hidden.bs.modal", function () {
        $("#txtEmployeeID").unbind("keypress").unbind("change");
        $("#txtEmployeeID").prop("disabled",false);
    });

    function fillPhlebotomistEditForm(employeeId) {

        let response = $.ajax({
            method: "GET",
            url: "/Phlebotomy/GetPhlebotomist",
            dataType: "JSON",
            data: { employeeId: employeeId }
        });

        response.done(function (data) {

            $("#txtEmployeeID").val(data.EmployeeID);
            $("#txtLastName").val(data.LastName);
            $("#txtFirstName").val(data.FirstName);
            $("#txtTelephone").val(data.Telephone);
            $("#txtAddress1").val(data.Address1);
            $("#txtAddress2").val(data.Address2);
            $("#txtCity").val(data.City);
            $("#txtState").val(data.State);
            $("#txtZip").val(data.Zip);
            $("#lstLeads").val(data.Leads).trigger("chosen:updated");
            $("#txtEmployeeStartDate").val(moment(data.EmployeeStartDate).format("YYYY-MM-DD"));
            $("#txtTerminationDate").val(moment(data.TerminationDate).format("YYYY-MM-DD"));

        }).then(function () {
            $("#editPhlebotomist").modal("show");
        });
    }
    function fillPhlebotomistDetailsForm(employeeId) {

        let response = $.ajax({
            method: "GET",
            url: "/Phlebotomy/GetPhlebotomistDetails",
            dataType: "JSON",
            data: { employeeId: employeeId }
        });

        response.done(function (data) {

            $("#PhlebotomistsDetails tbody>tr").remove();

            $("#lblEmployeeID").text(data.EmployeeID);
            $("#lblLastName").text(data.LastName);
            $("#lblFirstName").text(data.FirstName);
            $("#lblTelephone").text(data.Telephone);
            $("#lblAddress1").text(data.Address1);
            $("#lblAddress2").text(data.Address2);
            $("#lblCity").text(data.City);
            $("#lblState").text(data.State);
            $("#lblZip").text(data.Zip);
            $("#lblTerminationDate").text(data.TerminationDate);
            $("#lblEmployeeStartDate").text(data.EmployeeStartDate);

            for(let i = 0; i <data.PhlebotomistAssignmentLines.length; i++) {
                $("#PhlebotomistsDetails tbody").append("<tr><td>" + data.PhlebotomistAssignmentLines[i].AccountID + "</td><td>" + data.PhlebotomistAssignmentLines[i].GroupID + "</td><td>" + data.PhlebotomistAssignmentLines[i].PhlebotomistAssignmentID + "</td><td>" + data.PhlebotomistAssignmentLines[i].PhlebotomistAssignmentLineID + "</td><td>" + data.PhlebotomistAssignmentLines[i].Percentage);
            }
        }).then(function () {
            assignMentTable=$("#PhlebotomistsDetails").DataTable();
            $("#detailsPhlebotomist").modal("show");
        });
    }

    function ClearForm() {
        $.each($("#editPhlebotomist form input"), function (index, item) {
            $(item).val("");
        });
        $("#lstLeads").val("").trigger("chosen:updated");
    }

    $("#detailsPhlebotomist").on("hidden.bs.modal", function () {
        assignMentTable.destroy();
    });

    $("#btnCreate").click(function () {
        ClearForm();
        setCheckEmployeeID();
        $("#txtEmployeeID").prop("disabled", false);
        $("#editPhlebotomistModalLabel").text("Create");
        $("#editPhlebotomist").modal("show");
    });

    $("#lstLeads").chosen()
    $("#lstLeads_chosen").css("width", "200px");


    var phlebotomistsList = $("#Phlebotomists").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "EmployeeID",
        ajax: {
            url: "/Phlebotomy/phlebotomistsjson",
            type: "POST",
            dataSrc: "data",
            data: function (d) { d.activeStatus = $("#ActiveStatus").val() }
        },
        columns: [
            { data: "FirstName" },
            { data: "LastName" },
            { data: "State" },
            { data: "Telephone" },
            { data: "EmployeeID" },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-details-user'>Details</button>"
            },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-edit-user'>Edit</button>"
            },
        ]
    });

    phlebotomistsList.on('draw', function () {
        $("#Phlebotomists button.btn-edit-user").click(function (event) {
            let phlebotomistId = $(event.target).closest("tr").attr("id");
            fillPhlebotomistEditForm(phlebotomistId);
            $("#txtEmployeeID").prop("disabled", true);
        });
        $("#Phlebotomists button.btn-details-user").click(function (event) {
            let phlebotomistId = $(event.target).closest("tr").attr("id");
            fillPhlebotomistDetailsForm(phlebotomistId);
        });
    });

    $("#ActiveStatus").change(function () {
        phlebotomistsList.ajax.reload();
    });

    $("#btnAddLead").click(function () {
        let v = $("#lstLeads option:selected").val();
        let n = $("#lstLeads option:selected").text();

        let exists = $("#lstPhlebotomists li").filter(function (item) {
            return v == $(this).attr("value");
        });

        if (exists.length == 0)
            $("#lstPhlebotomistsLeads").append("<li value=" + v + "><input type='checkbox' />&nbsp;<span>" + n + "</span></li>");

    });
    $("#btnOk").click(function (event) {

        if (!$("#frmEmployer").valid()) {
            return false;
        }

        let employeeId = $("#txtEmployeeID").val();
        let LastName = $("#txtLastName").val();
        let FirstName = $("#txtFirstName").val();
        let Telephone = $("#txtTelephone").val();
        let Address1 = $("#txtAddress1").val();
        let Address2 = $("#txtAddress2").val();
        let City = $("#txtCity").val();
        let State = $("#txtState").val();
        let Zip = $("#txtZip").val();
        let employeeStartDate = $("#txtEmployeeStartDate").val();
        let terminationDate = $("#txtTerminationDate").val();

        let Leads = $("#lstLeads").map(function () {
            return $(this).val();
        }).get();

        let response = $.ajax({
            method: "POST",
            url: "/Phlebotomy/SavePhlebotomist",
            dataType: "JSON",
            data: {
                employeeId: employeeId,
                LastName: LastName,
                FirstName: FirstName,
                Telephone: Telephone,
                Address1: Address1,
                Address2: Address2,
                City: City,
                State: State,
                Zip: Zip,
                Leads: Leads,
                employeeStartDate: employeeStartDate,
                terminationDate: terminationDate
            }
        });

        response.done(function (data) {
            if (data == "OK")
                $("#editPhlebotomist").modal("hide");
        });
    });

    $("#btnRemoveLead").click(function (event) {
        $.each($("#lstPhlebotomistsLeads input:checked"),function (index,element) {
            $(element).closest("li").remove();
        });
    });

    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});