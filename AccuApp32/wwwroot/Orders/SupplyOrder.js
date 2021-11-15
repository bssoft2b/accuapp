var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {


    $("#lstStatusFilter").chosen();

    $("#lstStatusFilter").change(function () {
        supplyOrdersList.ajax.reload();
    });


    var addressesObj;
    $("#lstAccountID").change(function (event) {

        $("#txtDeliveryAddressName").val("");
        $("#txtAddress").val("");
        $("#txtCity").val("");
        $("#txtSuite").val("");
        $("#txtState").val("");
        $("#txtZip").val("");


        let accountID = $(event.target).val();

        $.ajax({
            url: "/Order/GetAdressesJson",
            method: "POST",
            dataType: "JSON",
            data: {
                AccountID: accountID
            },
            success: function (data) {

                $("#lstDeliveryAddressID").empty();
                $("#lstDeliveryAddressID").append("<option value=''> - select address - </option>");
                $("#lstDeliveryAddressID").append("<option value='-1'>New Adress</option>");

                for (let i = 0; i < data.length; i++) {
                    $("#lstDeliveryAddressID").append("<option value='" + data[i].DeliveryAddressID + "'>" + data[i].DeliveryAddressName + "</option>");
                }
            }
        });
    });

    $("#lstDeliveryAddressID").change(function (event) {

        $("#txtDeliveryAddressName").val("");
        $("#txtAddress").val("");
        $("#txtCity").val("");
        $("#txtSuite").val("");
        $("#txtState").val("");
        $("#txtZip").val("");

        let deliveryAddressID = $("#lstDeliveryAddressID").val();

        if (deliveryAddressID == "-1") {

            $("#txtDeliveryAddressName").prop("readonly",false);
            $("#txtAddress").prop("readonly", false);
            $("#txtCity").prop("readonly", false);
            $("#txtSuite").prop("readonly", false);
            $("#txtState").prop("readonly", false);
            $("#txtZip").prop("readonly", false);

        } else {

            $("#txtDeliveryAddressName").prop("readonly", true);
            $("#txtAddress").prop("readonly", true);
            $("#txtCity").prop("readonly", true);
            $("#txtSuite").prop("readonly", true);
            $("#txtState").prop("readonly", true);
            $("#txtZip").prop("readonly", true);

        }

        $.ajax({
            url: "/Order/GetAdressJson",
            method: "POST",
            dataType: "JSON",
            data: {
                deliveryAddressID: deliveryAddressID
            },
            success: function (data) {
                $("#txtDeliveryAddressName").val(data.DeliveryAddressName);
                $("#txtAddress").val(data.Address);
                $("#txtCity").val(data.City);
                $("#txtSuite").val(data.Suite);
                $("#txtState").val(data.State);
                $("#txtZip").val(data.Zip);
            }
        });

    });

    $("#txtDateShipped").datetimepicker();

    $("#frmSupplyOrders").on("hidden.bs.modal", function () {
        FormClear();
        FormControlsEnable();
    });

    $("#txtItemID").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Order/LoadItems",
                dataType: "json",
                method: "POST",
                data: {
                    filter: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        }, minLength: 2,
        select: function (event, ui) {
            $.ajax({
                url: "/Order/getitem",
                dataType: "json",
                method: "POST",
                data: {

                    itemid: ui.item.id
                },
                success: function (data) {
                    if (data == null)
                        return false;
                    $("#Items").append("<tr  salesorderlineid='-1' itemid='" + data.itemID + "'><td>" + data.itemName + "</td><td><input class='form-control ordered-qty' value='' /></td><td><input value='" + data.itemCost + "' class='form-control unit-cost'/></td><td>" + data.itemDescription + "</td><td><input value='' class='form-control committed-qty'/></td><td><input value='' class='form-control cancelled-qty'/><td><input value='' class='form-control backordered-qty'/></td><td><button class='btn btn-danger item-del'>X</button></td><tr>")
                    $("#txtItemID").val("");
                    $("button.item-del").unbind("click").on("click", function (event) {
                        $(event.target).closest("tr").remove();
                    });
                }
            });
        }
    });


    //list emrrequests
    var supplyOrdersList = $("#tblSupplyOrders").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "SalesOrderID",
        ajax: {
            url: "/Order/SupplyOrdersJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.requestStatus = $("#lstRequestStatus").val()
            }
        },
        order: [[2, "desc"]],
        columns: [
            {
                data: null,
                sortable: false,
                defaultContent: "<input class='order-row' type='checkbox' />"
            },
            { data: "SalesOrderID" },
            { data: "DateCreated" },
            { data: "DatePrinted" },
            { data: "DateShipped" },
            { data: "AccountID" },
            { data: "TrackingNumber" },
            { data: "DeliveryMethodName" },
            { data: "StatusName" },
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
            {
                data: null,
                sortable: false,
                render: function (data, type, row, meta) {
                    if (data.SalesOrderStatusID != 3)
                        return "<button class='btn btn-secondary btn-delete'>Delete</button>";
                    else
                        return "<button class='btn btn-secondary btn-delete' disabled> Delete</button>";
                }
            },
            {
                data: null,
                sortable: false,
                render: function (data, type, row, meta) {
                    if (data.SalesOrderStatusID != 3)
                        return "<button class='btn btn-secondary btn-print'>Print This</button>";
                    else
                        return "<button class='btn btn-secondary btn-print'  disabled>Print This</button>";
                }
            },
            {
                data: null,
                sortable: false,
                render: function (data, type, row, meta) {
                    if (data.SalesOrderStatusID != 3)
                        return "<button class='btn btn-secondary btn-approve'>Approve</button>";
                    else
                        return "<button class='btn btn-secondary btn-approve' disabled>Approve</button>";
                }
            }
        ]
    });

    //binding event to button Edit and Details
    supplyOrdersList.on('draw', function () {
        $("#tblSupplyOrders button.btn-edit").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmSupplyOrdersModalLabel").text("Edit");
            fillEditForm(SalesOrderID);
        });
        $("#tblSupplyOrders button.btn-details").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmSupplyOrdersDetailsModalLabel").text("Details");

            fillEditForm(SalesOrderID);

            setTimeout(function () {
                FormControlsDisable();
            }, 500);
            
        });
        $("#tblSupplyOrders button.btn-delete").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            if(confirm("Confirm delete?"))
                DeleteOrder(SalesOrderID);
        });
        $("#tblSupplyOrders button.btn-print").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            PrintForm(SalesOrderID);
        });
        $("#tblSupplyOrders button.btn-approve").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            ApproveOrder(SalesOrderID);
        });
    });

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        FormControlsEnable();
        FormClear();

        $("#frmSupplyOrdersModalLabel").text("Create");
        $("#frmSupplyOrders").modal("show");

        $("#lstOrderStatusID").val(1);
        $("#lstOrderStatusID").prop("disabled", true);

    });

    //filling edit form
    function fillEditForm(salesOrderID) {

        $.ajax({
            method: "POST",
            url: "/Order/GetSupplyOrder",
            dataType: "JSON",
            data: { salesOrderID: salesOrderID },
            success: function (data) {

                FillForm(data);

                $("#frmSupplyOrders").modal("show");
            }
        });

    }
    function DeleteOrder(salesOrderID) {

        $.ajax({
            method: "POST",
            url: "/Order/DeleteSupplyOrder",
            dataType: "JSON",
            data: { salesOrderID: salesOrderID },
            success: function (data) {
                supplyOrdersList.ajax.reload();
            }
        });

    }

    //filling edit form
    function FillForm(data) {


        $("#txtSupplyOrderID").val(data.salesOrderID);
        $("#lstOrderStatusID").val(data.salesOrderStatusID);
        $("#lstDeliveryMethodID").val(data.deliveryMethodID);

        $("#lstAccountID").empty();
        $("#lstAccountID").append("<option value=''></option>");
        for (let i = 0; i < data.accounts.length; i++) {
            $("#lstAccountID").append("<option value='" + data.accounts[i].accountID + "'>" + data.accounts[i].accountID + " - " + data.accounts[i].name + "</option>")
        }
        $("#lstAccountID").val(data.accountID);

        $("#lstSalesRep").val(data.salesRep);
        $("#txtTrackingNumber").val(data.trackingNumber);

        $("#lstDeliveryAddressID").empty();
        $("#lstDeliveryAddressID").append("<option value=''>-selected address-</option>");
        $("#lstDeliveryAddressID").append("<option value='-1'>New Delivery Adress</option>");
        for (let i = 0; i < data.deliveryAddresses.length; i++) {
            $("#lstDeliveryAddressID").append("<option value='" + data.deliveryAddresses[i].deliveryAddressID + "'>" + data.deliveryAddresses[i].deliveryAddressName+"</option>");
        }
        $("#lstDeliveryAddressID").val(data.deliveryAddressID);

        //address
        $("#txtDeliveryAddressName").val(data.deliveryAddress.deliveryAddressName);
        $("#txtAddress").val(data.deliveryAddress.address);
        $("#txtCity").val(data.deliveryAddress.city);
        $("#txtSuite").val(data.deliveryAddress.suite);
        $("#txtState").val(data.deliveryAddress.state);
        $("#txtZip").val(data.deliveryAddress.zip);

        //item tables 
        $("#Items tr").remove();
        for (let i = 0; i < data.orderLines.length; i++) {

            $("#Items").append("<tr  salesorderlineid='" + data.orderLines[i].salesOrderLineID + "' itemid='" + data.orderLines[i].itemID + "'><td>" + data.orderLines[i].itemName + "</td><td><input class='form-control ordered-qty' value='" + data.orderLines[i].orderedQty + "' /></td><td><input value='" + data.orderLines[i].unitCost + "' class='form-control unit-cost'/></td><td>" + data.orderLines[i].itemDescription + "</td><td><input value='" + data.orderLines[i].committedQty + "' class='form-control committed-qty'/></td><td><input value='" + data.orderLines[i].cancelledQty + "' class='form-control cancelled-qty'/><td><input value='" + data.orderLines[i].backOrderedQty+"' class='form-control backordered-qty'/></td><td><button class='btn btn-danger item-del'>X</button></td><tr>")
        }
        $("#txtItemID").val("");
        $("button.item-del").unbind("click").on("click", function (event) {
            $(event.target).closest("tr").remove();
        });

        //
        $("#txtOrderNote").val(data.orderNote);
        $("#txtShippingNote").val(data.shippingNote);

        $("#txtDateShipped").val(data.dateShipped);

        $("#txtDateCreated").val(data.dateCreated);
        $("#txtCreatedBy").val(data.createdBy);

        $("#txtUpdatedDate").val(data.updatedDate);
        $("#txtUpdatedBy").val(data.updatedBy);

        $("#txtCompletedDate").val(data.completedDate);
    }
 
    function FormControlsDisable() {

        $("#frmSupplyOrders input").prop("disabled", true);
        $("#frmSupplyOrders textarea").prop("disabled", true);
        $("#frmSupplyOrders select").prop("disabled", true);
        $("#frmSupplyOrders button").prop("disabled", true);

        $("button.btn-close").prop("disabled", false);
        $("button.close").prop("disabled", false);

    }

    function FormControlsEnable() {

        $("#frmSupplyOrders input").prop("disabled", false);
        $("#frmSupplyOrders textarea").prop("disabled", false);
        $("#frmSupplyOrders select").prop("disabled", false);
        $("#frmSupplyOrders button").prop("disabled", false);

        $("#Items input").prop("disabled", false);
        $("#Items textarea").prop("disabled", false);
        $("#Items select").prop("disabled", false);
        $("#Items button").prop("disabled", false);

        $("button.btn-close").prop("disabled", false);
        $("button.close").prop("disabled", false);

        $("#btnSave").prop("disabled", false);

    }


    //clear control element
    function FormClear() {

        $.each($("#frmSupplyOrders form input"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmSupplyOrders form textarea"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmSupplyOrders form select"), function (index, item) {
            $(item).val("");
        });

        $("#lstDeliveryAddressID").empty();

        $("#Items tr").remove();

    }

    //button Ok modal form
    $("#btnSave").click(function () {

        let supplyOrderID = $("#txtSupplyOrderID").val();
        let orderStatusID = $("#lstOrderStatusID").val();
        let deliveryMethodID = $("#lstDeliveryMethodID").val();
        let accountID = $("#lstAccountID").val();
        let deliveryAddressID = $("#lstDeliveryAddressID").val();
        let deliveryAddressName = $("#txtDeliveryAddressName").val();
        let address = $("#txtAddress").val();
        let city = $("#txtCity").val();
        let suite = $("#txtSuite").val();
        let state = $("#txtState").val();
        let zip = $("#txtZip").val();
        let salesRep = $("#lstSalesRep").val();
        let trackingNumber = $("#txtTrackingNumber").val();


        //ordered lines
        let orderLineIds = $("#Items tr").map(function () {
            return $(this).attr("salesorderlineid");
        }).get();

        let itemIds = $("#Items tr").map(function () {
            return $(this).attr("itemid");
        }).get();

        let orderedQty = $("#Items input.ordered-qty").map(function () {
            return $(this).val() == "" ? "0" : $(this).val();
        }).get();

        let unitCost = $("#Items input.unit-cost").map(function () {
            return $(this).val() == "" ? "0" : $(this).val();
        }).get();

        let committedQty = $("#Items input.committed-qty").map(function () {
            return $(this).val() == "" ? "0" : $(this).val();
        }).get();

        let cancelledQty = $("#Items input.cancelled-qty").map(function () {
            return $(this).val() == "" ? "0":$(this).val();
        }).get();

        let backOrderedQty = $("#Items input.backordered-qty").map(function () {
            return $(this).val() == "" ? "0" : $(this).val();
        }).get();

        let orderNote = $("#txtOrderNote").val();
        let shippingNote = $("#txtShippingNote").val();
        let dateShipped = $("#txtDateShipped").val();


        $.ajax({
            url: "/Order/SaveSupplyOrder",
            method: "POST",
            dataType: "JSON",
            data: {
                    supplyOrderID,
                    orderStatusID,
                    deliveryMethodID,
                    accountID,
                    deliveryAddressID,
                    deliveryAddressName,
                    address,
                    city,
                    suite,
                    state,
                    zip,
                    orderLineIds,
                    itemIds,
                    orderedQty,
                    unitCost,
                    committedQty,
                    cancelledQty,
                    backOrderedQty,
                    orderNote,
                    shippingNote,
                    dateShipped,
                    salesRep,
                    trackingNumber
            },
            success: function (data) {
                if (data == "OK") {
                    supplyOrdersList.ajax.reload();
                    $("#frmSupplyOrders").modal("hide");
                    FormClear();
                }
            },
            error: function (xhr, status, errorThrown) {
                if (xhr.status == 403) {
                    alert("Forbidden.You don't have permission for saving!");
                }
            }
        });
    });

    function openPDFOne(salesOrderID) {

        $.ajax({
            async: true,
            url: '/order/PrintSalesOrder',
            type: "POST",
            data: {
                id: salesOrderID
            },
            success: function (response) {
                response = JSON.parse(response);
                console.log(response);
                let data = response.PDF;
                let fileName = "SupplyOrder.pdf";
                if (window.navigator && window.navigator.msSaveOrOpenBlob) { // IE workaround
                    let byteCharacters = atob(data);
                    let byteNumbers = new Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    let byteArray = new Uint8Array(byteNumbers);
                    let blob = new Blob([byteArray], { type: 'application/pdf' });
                    window.navigator.msSaveOrOpenBlob(blob, fileName);
                }
                else { // much easier if not IE
                    let pdfWindow = window.open("");
                    pdfWindow.document.write('<iframe  width="100%" height="100%" src="data:application/pdf;base64, ' + response.PDF + '"></iframe>');
                }
            },
            error: function (x, t, m) {
                alert(t);
            }
        });
    }
    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});