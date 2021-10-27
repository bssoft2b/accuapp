var editor;
var agentsOptions;

$(function () {


    $.get({
        url: "/BillingTools/GetAgentGroups",
        dataType: "JSON",
        async:false,
        success: function (data) {
            agentsOptions = data;
        }
    });

    $("#BillTypes thead input").on("keyup", function () {
        billTypesList.ajax.reload();
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "/BillingTools/UpdateBillType",
        table: "#BillTypes",
        idSrc: 'BillType',
        fields: [
            {
                label: "Ins Name",
                name: "InsName",
            },
            {
                label: "Agent",
                name: "Agent",
                type: "select",
                options: agentsOptions
            },
            {
                label: "Timely Filing",
                name: "TimelyFiling",
            },
            {
                label: "Appeal Timely",
                name: "AppealTimely",
            },
            {
                label: "In Network",
                name: "InNetwork",
                type: "checkbox",
                separator: "|",
                options: [{ label: "", value: 1 }]
            },
            {
                label: "Notes",
                name: "Notes",
            }
        ]
    });


    // Activate an inline edit on click of a table cell
    $('#BillTypes').on('click', 'tbody td:not(:first-child)', function (e) {
        editor.inline(billTypesList.cell(this).index(), {
            onBlur: 'submit'
        });
    });



    var billTypesList = $("#BillTypes").DataTable({
        //dom: "Bfrtip",
        serverSide: true,
        processing: true,
        rowId: "BillType",
        ajax: {
            url: "/BillingTools/GetBillTypesJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.srcAgent = $("#srcAgent").val();
                d.srcBilltype = $("#srcBillType").val(),
                d.srcInsName = $("#srcInsName").val(),
                d.srcCountInvoices = $("#srcCountInvoices").val(),
                d.srcTimelyFiling = $("#srcTimelyFiling").val(),
                d.srcAppealTimely = $("#srcAppealTimely").val(),
                d.srcNotes = $("#srcNotes").val()
            },
        },
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        orderCellsTop: true,
        fixedHeader: true,
        columns: [
            { data: "BillType"},
            { data: "InsName" },
            { data: "Agent"},
            { data: "CountInvoices" },
            { data: "TimelyFiling" },
            { data: "AppealTimely" },
            {
                data: "InNetwork",
                searchable: false,
                sortable: false,
                render: function (data, type, row) {
                    if (type == "display") {
                        if(data)
                            return '<input type="checkbox" class="editor-active" checked>';
                        else 
                            return '<input type="checkbox" class="editor-active">';
                    }
                    return data;
                }
            },
            { data: "Notes" }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        }
    });
});