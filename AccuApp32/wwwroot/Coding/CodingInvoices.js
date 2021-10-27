var assignMentTable;
$(function () {

    $("#lstStatus").chosen()
    $("#lstStatus_chosen").css("width", "200px");

    function ClearForm() {
        $("#txtKey").val("");
        $("#txtInvoice").val("");
        $("#txtBillType").val("");
        $("#txtPrevBillType").val("");
        $("#lstStatus").val([]).trigger("chosen:updated");
        $("#txtNotes").val("");
    }

    function fillInvoiceEditForm(key) {

        let response = $.ajax({
            method: "GET",
            url: "/Coding/GetInvoice",
            dataType: "JSON",
            data: { id: key }
        });

        response.done(function (data) {

            let multiValue = [];

            if (data.Status.indexOf(",")>-1) {
                multiValue = data.Status.split(",");
            } else if (data.Status.indexOf(";")>-1) {
                multiValue = data.Status.split(";");
            } else {
                multiValue[0] = data.Status;
            }

            $("#txtKey").val(data.key);
            $("#txtInvoice").val(data.InvoiceNo);
            $("#txtBillType").val(data.BillType);
            $("#txtPrevBillType").val(data.PrevBillType);
            $("#lstStatus").val(multiValue).trigger("chosen:updated");
            $("#txtNotes").val(data.Notes);

            $("#frmEditCodingInvoice").valid();

        }).then(function () {
            $("#editCodingInvoices").modal("show");
        });
    }

    var codingInvoicesList = $("#tblCodingInvoicesList").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        searching: false,
        processing: true,
        ajax: {
            url: "/Coding/GetCodingInvoicesListJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.invoice = $("#srcInvoice").val();
                d.pbtype = $("#srcPrevBillType").val();
                d.btype = $("#srcBillType").val();
                d.status = $("#srcStatus").val();
                d.user = $("#lstUser").val();
                d.fromdate = $("#srcFromDate").val();
                d.todate = $("#srcToDate").val();
            }
        },
        order:[[1,"asc"]],
        rowId: "Key",
        columns: [
            {
                data: null,
                sortable: false,
                defaultContent: "<input type='checkbox' />"
            },
            { data: "InvoiceNo" },
            { data: "PrevBillType" },
            { data: "BillType" },
            { data: "Status" },
            { data: "User" },
            { data: "Notes", sortable:false },
            { data: "Date" },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-edit-user'>Edit</button>"
            },
        ]
    });

    codingInvoicesList.on('draw', function () {
        $("#tblCodingInvoicesList button.btn-edit-user").click(function (event) {
            let key = $(event.target).closest("tr").attr("id");
            fillInvoiceEditForm(key);
        });
    });

    $("#btnSearch").click(function () {
        codingInvoicesList.ajax.reload();
    });

    $("#btnReset").click(function () {

        $("#srcInvoice").val("");
        $("#srcPrevBillType").val("");
        $("#srcBillType").val("");
        $("#srcStatus").val("");
        $("#lstUser").val([]).change("change:updated");
        $("#srcFromDate").val("");
        $("#srcToDate").val("");

        codingInvoicesList.ajax.reload();

    });

    $("#btnCreateNew").click(function () {
        ClearForm();
        $("#editCodingInvoices").modal("show");
    });

    //validation
    $("#frmEditCodingInvoice").validate({
        rules: {
            txtBillType: "required",
            txtInvoice:"required"
        },
        messages: {
            txtBillType: "Enter bill type...",
            txtInvoice:"Enter invoice number..."
        }
    });


    $("#btnOk").click(function (event) {

        if (!$("#frmEditCodingInvoice").valid())
            return;
        let key=$("#txtKey").val();
        let invoice=$("#txtInvoice").val();
        let billType=$("#txtBillType").val();
        let prevBillType=$("#txtPrevBillType").val();
        let notes = $("#txtNotes").val();
        let vals = $("#lstStatus").val().map(function (el, i) {
            return el;
        });

        let status = "";
        for (let i = 0; i < vals.length; i++) {
            status += vals[i] + ",";
        }

        if (status != "") {
            status = status.substring(0, status.length - 1)
        }

        let response = $.ajax({
            method: "POST",
            url: "/Coding/SaveCodingInvoice",
            dataType: "JSON",
            data: {
                key: key,
                invoice: invoice,
                billType: billType,
                prevBillType,
                status: status,
                notes:notes
            }
        });

        response.done(function (data) {
            if (data == "OK") {
                $("#editCodingInvoices").modal("hide");
                codingInvoicesList.ajax.reload();
            }
        });
    });

    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});