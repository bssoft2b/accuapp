
$(function () {

    $("#filter").change(function () {
        accountDictionaryList.ajax.reload();
    });

    //acession list
    var accountDictionaryList = $("#tblAccountDictionary").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        searching:true,
        rowId: "AccountId",
        ajax: {
            url: "/Phlebotomy/ActionDictionaryJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.Status = $("#filter").val()
            }
        },
        order:[[1,"asc"]],
        columns: [
            { data: "AccountId"},
            { data: "Name" },
            { data: "Telephone" },
            { data: "Fax" },
            { data: "Address" },
            { data: "City" },
            { data: "State" }
        ]
    });
});