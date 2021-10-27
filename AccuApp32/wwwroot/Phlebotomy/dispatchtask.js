var assignMentTable;
$(function () {

    function fillDispatchTaskEditForm(dispatchTaskId) {

        let response = $.ajax({
            method: "GET",
            url: "/Phlebotomy/GetDispatchTask",
            dataType: "JSON",
            data: { dispatchTaskId: dispatchTaskId }
        });

        response.done(function (data) {

            $("#txtDispatchTaskID").val(data.DispatchTaskID);
            $("#txtAccountID").val(data.AccountID);
            $("#txtAddressLine1").val(data.AddressLine1);
            $("#txtAddressLine2").val(data.AddressLine2);
            $("#txtCity").val(data.City);
            $("#txtState").val(data.State);
            $("#txtZip").val(data.Zip);
            $("#lstCallReason").val(data.ReasonID);
            $("#txtDriver").val(data.Driver);
            $("#txtPickupTime").val(data.PickupTime);
            $("#chkStat").prop("checked", data.Stat);


            $("#lstDispatchStatusID option").remove();
            for (let i = 0; i < data.DispatchStatuses.length; i++) {
                $("#lstDispatchStatusID").append("<option value='" + data.DispatchStatuses[i].Id + "'>" + data.DispatchStatuses[i].Name+"</li>");
            }
            $("#lstDispatchStatusID").val(data.DispatchStatusID);

            $("#lstDispatchContactEmail li").remove();
            for (let i = 0; i < data.DispatchContactEmail.length; i++) {
                $("#lstDispatchContactEmail").append("<li value='" + data.DispatchContactEmail[i].Id + "'><input type='checkbox' />&nbsp;<input type='text' class='form-control' value='" + data.DispatchContactEmail[i].Email+"' /></li>");
            }

        }).then(function () {
            $("#editDispatchTask").modal("show");
        });
    }
    function fillDispatchTaskDetailsForm(dispatchTaskId) {

        let response = $.ajax({
            method: "GET",
            url: "/Phlebotomy/GetDispatchTask",
            dataType: "JSON",
            data: { dispatchTaskId: dispatchTaskId }
        });

        response.done(function (data) {

            $("#lblDispatchTaskID").val(data.DispatchTaskID);
            $("#lblAccountID").val(data.AccountID);
            $("#lblAddressLine1").val(data.AddressLine1);
            $("#lblAddressLine2").val(data.AddressLine2);
            $("#lblCity").val(data.City);
            $("#lblState").val(data.State);
            $("#lblZip").val(data.Zip);
            //$("#lblCallReason").val(data.ReasonID);
            $("#lblDispatchStatusID").val(data.DispatchStatusID==1?"New":"Assigned");
            $("#lblDriver").val(data.Driver);
            $("#lblPickupTime").val(data.PickupTime);
            $("#lblStat").prop("checked", data.Stat);

            $("#lblDispatchContactEmail li").remove();
            for (let i = 0; i < data.DispatchContactEmail.length; i++) {
                $("#lblDispatchContactEmail").append("<li value='" + data.DispatchContactEmail[i].Id + "'>&nbsp;<input type='text' class='form-control' value='" + data.DispatchContactEmail[i].Email + "' readonly /></li>");
            }

        }).then(function () {
            $("#detailsDispatchTask").modal("show");
        });
    }
    $("#btnRemoveDispatchContactEmail").click(function (event) {
        $.each($("#lstDispatchContactEmail input:checked"), function (index, element) {
            $(element).closest("li").remove();
        });
    });

    $("#btnAddDispatchContactEmail").click(function () {
        $("#lstDispatchContactEmail").append("<li value=''><input type='checkbox' />&nbsp;<input type='text' class='form-control' value='' /></li>");
    });


    $("#detailsDispatchTask").on("hidden.bs.modal", function () {
        assignMentTable.destroy();
    });



    var dispatchTasksList = $("#tblTasks").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "DispatchTaskId",
        ajax: {
            url: "/Phlebotomy/DispatchTasksJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) { d.status = $("#selectDispatchStatus").val() }
        },
        columns: [
            {
                data: "DispatchStatusId",
                render: function (data, type, row, meta) {
                    return data == 1 ? "<div style='height:20px;width:20px;background-color:red'></div>" : "<div style='height:20px;width:20px;background-color:limegreen'></div >";
                }
            },
            { data: "DispatchTaskId" },
            { data: "AccountId" },
            { data: "Name" },
            { data: "Telephone" },
            {
                data: "Stat",
                render: function (data, type, row, meta) {
                    return data ? "<input type='checkbox' checked readonly disabled >" : "<input type='checkbox' readonly disabled>";
                }
            },
            { data: "ReasonID" },
            { data: "Driver" },
            { data: "PickupTime" },
            { data: "LastWorked" },
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
        ]
    });

    dispatchTasksList.on('draw', function () {
        $("#tblTasks button.btn-edit").click(function (event) {
            let dispatchtaskId = $(event.target).closest("tr").attr("id");
            fillDispatchTaskEditForm(dispatchtaskId);
        });
        $("#tblTasks button.btn-details").click(function (event) {
            let dispatchtaskId = $(event.target).closest("tr").attr("id");
            fillDispatchTaskDetailsForm(dispatchtaskId);
        });
    });

    $("#selectDispatchStatus").change(function () {
        dispatchTasksList.ajax.reload();
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

        let dispatchTaskID = $("#txtDispatchTaskID").val();
        let accountID = $("#txtAccountID").val();
        let Address1=$("#txtAddressLine1").val();
        let Address2=$("#txtAddressLine2").val();
        let City=$("#txtCity").val();
        let State=$("#txtState").val();
        let Zip=$("#txtZip").val();
        let ReasonID=$("#lstCallReason").val();
        let Driver=$("#txtDriver").val();
        let PickupTime=$("#txtPickupTime").val();
        let Stat = $("#chkStat").prop("checked");
        let DispatchStatusID = $("#lstDispatchStatusID").val();

        let DispatchContactEmails = $("#lstDispatchContactEmail li").map(function () {
            return {
                DispatchTaskId:dispatchTaskID,
                DispatchContactID:$(this).val(),
                DispatchContactEmail:$(this).find("input[type=text]").val()
            };
        }).get();

        let response = $.ajax({
            method: "POST",
            url: "/Phlebotomy/SaveDispatchTask",
            dataType: "JSON",
            data: {
                dispatchTaskID: dispatchTaskID,
                accountID: accountID,
                Address1: Address1,
                Address2: Address2,
                City: City,
                State: State,
                Zip: Zip,
                ReasonID: ReasonID,
                Driver: Driver,
                PickupTime: PickupTime,
                Stat: Stat,
                DispatchStatusID: DispatchStatusID,
                DispatchContactEmails: DispatchContactEmails
            }
        });

        response.done(function (data) {
            if (data == "OK")
                $("#editDispatchTask").modal("hide");
        });
    });

    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});