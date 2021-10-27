var assignMentTable;
$(function () {

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

            $("#lstLeads option").remove();

            for (let i = 0; i < data.Leads.length; i++) {
                $("#lstLeads").append("<option value=" + data.Leads[i].Id + ">" + data.Leads[i].Name + "</option>");
            }

            $("#lstPhlebotomistsLeads li").remove();

            for (let i = 0; i < data.PhlebotomistLeads.length; i++) {
                $("#lstPhlebotomistsLeads").append("<li value=" + data.PhlebotomistLeads[i].Id + "><input type='checkbox' />&nbsp;<span>" + data.PhlebotomistLeads[i].Name + "</span></li>");
            }
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

            for(let i = 0; i <data.PhlebotomistAssignmentLines.length; i++) {
                $("#PhlebotomistsDetails tbody").append("<tr><td>" + data.PhlebotomistAssignmentLines[i].AccountID + "</td><td>" + data.PhlebotomistAssignmentLines[i].GroupID + "</td><td>" + data.PhlebotomistAssignmentLines[i].PhlebotomistAssignmentID + "</td><td>" + data.PhlebotomistAssignmentLines[i].PhlebotomistAssignmentLineID + "</td><td>" + data.PhlebotomistAssignmentLines[i].Percentage);
            }
        }).then(function () {
            assignMentTable=$("#PhlebotomistsDetails").DataTable();
            $("#detailsPhlebotomist").modal("show");
        });
    }

    $("#detailsPhlebotomist").on("hidden.bs.modal", function () {
        assignMentTable.destroy();
    });



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

        let employeeId = $("#txtEmployeeID").val();
        let LastName = $("#txtLastName").val();
        let FirstName = $("#txtFirstName").val();
        let Telephone = $("#txtTelephone").val();
        let Address1 = $("#txtAddress1").val();
        let Address2 = $("#txtAddress2").val();
        let City = $("#txtCity").val();
        let State = $("#txtState").val();
        let Zip = $("#txtZip").val();

        let phlebotomistsLead = $("#lstPhlebotomistsLeads li").map(function () {
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
                phlebotomistsLead: phlebotomistsLead
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