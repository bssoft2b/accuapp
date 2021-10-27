$(function () {

    $("#btnCreate").click(function () {

        $("#ediTicketModalLabel").text("Create ticket");

        ClearForm();

        fillTicketEditForm();

        $("#editTicket").modal("show");
    });

    $("#lstStatus").change(function () {
        ticketsList.ajax.reload();
    });

    $("#lstType").change(function () {
        ticketsList.ajax.reload();
    });

    function ClearForm() {
        $("#txtTicketID").val("");
        $("#lstTicketStatus").val("");
        $("#lstVendors").val("");
        $("#txtAdditionalContactInfo").val();
        $("#lstTicketType").val("");
        $("#txtTicketDescription").val("");
        $("#lstAccounts").val("");
        $("#txtExamples").val("");
        $("#txtActionTaken").val("");
    }

    function fillTicketEditForm(ticketId) {

        let response = $.ajax({
            method: "GET",
            url: "/Support/GetTicket",
            dataType: "JSON",
            data: { Id: ticketId }
        });

        response.done(function (data) {

            //attachments
            $("#tblTicketAttachments tr.attach").remove();
            for (let i = 0; i < data.Attachments.length; i++) {
                $("#tblTicketAttachments tbody").after("<tr class='attach' attachment-id='" + data.Attachments[i].AttachmentID + "'><td><input type='checkbox' /></td><td><a href='" + data.Attachments[i].AttachmentContent + "' download='" + data.Attachments[i].AttachmentName+"'>" + data.Attachments[i].AttachmentName+"</a></td><tr>");
            }

            //contacts
            $("#tblTicketContacts tr.contact").remove();
            for (let i = 0; i < data.Contacts.length; i++) {
                $("#tblTicketContacts tbody").after("<tr class='contact' contact-id='" + data.Contacts[i].Id + "'><td><input type='checkbox' /></td><td>" + data.Contacts[i].Email + "</td><tr>");
            }

            //actionhistory
            $("#tblActionHistory tr.act").remove()
            for (let i = 0; i < data.TicketActions.length; i++) {
                $("#tblActionHistory tbody").after("<tr class='act' history-id='" + data.TicketActions[i].TicketActionID + "'><td>" + data.TicketActions[i].ActionDate + "</td><td>" + data.TicketActions[i].ActionBy + "</td><td>" + data.TicketActions[i].ActionTaken + "</td><tr>");
            }

            //accounts
            for (let i = 0; i < data.Accounts.length; i++) {
                $("#AccountId").append(`<option value="${data.Accounts[i].AccountID}" >${data.Accounts[i].AccountID} - ${data.Accounts[i].Name}</option>`);
            }

            $("#AccountId").val(data.AccountId);
            $("#lstTicketType").val(data.TicketType);

            $("#txtTicketID").val(data.TicketId);
            $("#txtTicket").attr("readonly", true);

            $("#txtEnteredBy").val(data.EnteredBy);
            $("#txtEnteredBy").attr("readonly", true);

            $("#txtDateEntered").val(data.DateEntered);
            $("#txtDateEntered").attr("readonly", true);

            $("#txtExamples").val(data.Examples);

            $("#lstTicketStatus").val(data.TicketStatus);
            if (data.TicketId == -1) {
                $("#lstTicketStatus").prop("disabled", true);
            } else {
                if (Roles.filter(t => t == "Admin" || t == "Account Manager").length>0) {
                    $("#lstTicketStatus").prop("disabled", false);
                } else {
                    $("#lstTicketStatus").prop("disabled", true);
                }
            }


            $("#txtAdditionalContactInfo").val(data.AdditionalContactInfo);
            $("#lstTicketType").val(data.TicketType);
            $("#txtTicketDescription").val(data.TicketDescription);

            $("#lstVendors").val(data.VendorId);
            $("#lstVendors").prop("disabled", true);


        }).then(function () {
            $("#editTicket").modal("show");
        });
    }

    var ticketsList = $("#tblTickets").DataTable({
        dom: "lpfrtip",
        lengthChange: false,
        serverSide: true,
        processing: true,
        rowId: "TicketID",
        ajax: {
            url: "/support/Ticketsjson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.ticketStatus = $("#lstStatus").val();
                d.ticketType = $("#lstType").val();
            }
        },
        columns: [
            { data: "TicketID" },
            { data: "TicketStatusText" },
            { data: "TicketTypeText" },
            { data: "AccountID" },
            { data: "Name" },
            { data: "EnteredBy" },
            { data: "Info" },
            { data: "DateEntered" },
            { data: "LastModified" },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-edit-ticket'>Edit</button>"
            },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary'>Details</button>"
            }
        ]
    });

    ticketsList.on('draw', function () {
        $("#tblTickets button.btn-edit-ticket").click(function (event) {
            let ticketId = $(event.target).closest("tr").attr("id");
            ClearForm();
            fillTicketEditForm(ticketId);
        });
    });

    $("#btnAddTicketAttachment").click(function () {
        $("#tblTicketAttachments tbody").after("<tr class='attach' attachment-id=''><td><input type='checkbox' /></td><td><input type='hidden' data='content' /><input type='hidden' data='name' /><input type='file' onchange='passValues(this);'/> </td></tr>")
        return false;
    });

    $("#btnRemoveTicketAttachment").click(function () {
        $.each($("#tblTicketAttachments").find("input:checked"), function (index, item) {
            $(item).parent().parent().remove();
        });
        return false;
    });

    $("#btnAddTicketContact").click(function () {
        $("#tblTicketContacts tbody").after("<tr class='contact' contact-id=''><td><input type='checkbox' /></td><td><input type='text' data='email' class='form-control' /></td></tr>")
        return false;
    });

    $("#btnRemoveTicketContact").click(function () {
        $.each($("#tblTicketContacts").find("input:checked"), function (index, item) {
            $(item).parent().parent().remove();
        });
        return false;
    });

    $("#btnSave").click(function () {

        let ticketId = $("#txtTicketID").val();
        let dateEntered = $("#txtDateEntered").val();
        let enteredBy = $("#txtEnteredBy").val();
        let ticketStatus = $("#lstTicketStatus").val();
        let ticketType = $("#lstTicketType").val();
        let additionalContactInfo = $("#txtAdditionalContactInfo").val();
        let ticketDescription = $("#txtTicketDescription").val();
        let vendorId = $("#lstVendors").val();
        let accountId = $("#AccountId").val();
        let examples = $("#txtExamples").val();
        let actionTaken = $("#txtActionTaken").val();

        let contact = $("#tblTicketContacts tr.contact").map(function (index, item) {
            let contactId = $(item).attr("contact-id");
            if(contactId!="")
                return contactId;
        }).get();

        let email = $("#tblTicketContacts tr.contact").map(function (index, item) {
            let email = $(item).find("input[type=text]").val();
            return email;
        }).get();

        let attachment = $("#tblTicketAttachments tr.attach").map(function (index, item) {
            let attachment = $(item).attr("attachment-id");
            return attachment;
        }).get();

        let content = $("#tblTicketAttachments tr.attach").map(function (index, item) {
            let content = $(item).find("input[data=content]").val();
            return content;
        }).get();

        let attachName=$("#tblTicketAttachments tr.attach").map(function (index, item) {
            let name = $(item).find("input[data=name]").val();
            return name;
        }).get();

        $.ajax({
            url: "/support/saveticket",
            dataType: "json",
            method: "POST",
            data: {
                ticketId,
                dateEntered,
                enteredBy,
                ticketStatus,
                ticketType,
                additionalContactInfo,
                ticketDescription,
                vendorId,
                accountId,
                examples,
                actionTaken,
                contact,
                email,
                attachment,
                attachName,
                content
            },
            success: function (data) {
                ticketsList.ajax.reload();
                $("#editTicket").modal("hide");
            }
        })

    });

    $("#lstTicketType").change(function() {

        let val=$(this).val();

        if(val== "0"){
            $("#lstVendors").prop("disabled", false);
        } else {
            $("#lstVendors").prop("disabled", true);
        }

    });
});


function passValues(element) {
    console.log(element);
    console.log(element.files);
    let filetest = element.files[0];
    let fileReader = new FileReader();
    fileReader.onloadend = function () {
        $(element).parent().find('[data="content"]').val(fileReader.result);
        $(element).parent().find('[data="name"]').val(filetest.name);
        return false;
    };
    fileReader.readAsDataURL(filetest);
    return false;
}
