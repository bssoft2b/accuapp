var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {


    $("#lstVendorID").change(function () {

        if ($(lstVendorID).val() == "")
            return false;

        $.ajax({
            method: "POST",
            url: "/Order/GetVendorsItems",
            dataType: "JSON",
            data: { vendorID: $("#lstVendorID").val() },
            success: function (data) {

                $("#lstItemID").empty();
                $("#lstItemID").append("<option value=''></option>");

                for (let i = 0; i < data.length; i++) {
                    $("#lstItemID").append("<option value='" + data[i].itemID + "'>" + data[i].label + "</option>");
                }

            }
        });
    });

    $("#lstItemID").change(function (event) {

        if ($(event.target).val() == "")
            return false;

        $.ajax({
            method: "POST",
            url: "/Order/GetVendorItem",
            dataType: "JSON",
            data: { vendorItemID: $(event.target).val() },
            success: function (data) {

                $("#Items").append("<tr  purchaseorderlineid='-1' vendoritemid='" + data.vendorItemID + "'><td>" + data.vendorItemID + "</td><td>" + data.vendorItemName + "</td><td><input class='form-control ordered-qty' value='0' /></td><td><input value='" + data.cost + "' class='form-control unit-cost' readonly/></td><td><input value='" + data.taxRate + "' class='form-control tax-rate' readonly/></td><td><input value='0' class='form-control total-cost' readonly/></td><td><input value='0' class='form-control received-qty'/><td><input value='0' class='form-control backorder-qty'/></td><td><input value='0' class='form-control cancelled-qty'/></td><td></td><td></td><td></td><td><span class='btn btn-danger item-del'>X</span></td><tr>");

                $("span.item-del").unbind("click").on("click", function (event) {
                    $(event.target).closest("tr").remove();
                });
            }
        });
    });

    //list emrrequests
    var purchaseOrdersList = $("#tblPurchaseOrders").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        rowId: "PurchaseOrderID",
        ajax: {
            url: "/Order/PurchaseOrdersJson",
            type: "POST",
            dataSrc: "data"
        },
        order: [[0, "asc"]],
        columns: [
            { data: "PurchaseOrderID" },
            { data: "VendorName" },
            { data: "DateCreated" },
            {
                data: null,
                sortable: false
            },
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
    purchaseOrdersList.on('draw', function () {
        $("#tblPurchaseOrders button.btn-edit").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmPurchaseOrdersModalLabel").text("Edit");
            fillEditForm(SalesOrderID);
        });
        $("#tblPurchaseOrders button.btn-details").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            $("#frmPurchaseOrdersModalLabel").text("Details");

            fillEditForm(SalesOrderID);

            setTimeout(function () {
                FormControlsDisable();
            }, 500);

        });
        $("#tblPurchaseOrders button.btn-delete").click(function (event) {
            let SalesOrderID = $(event.target).closest("tr").attr("id");
            if (confirm("Confirm delete?"))
                DeleteOrder(SalesOrderID);
        });
    });

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        FormControlsEnable();
        FormClear();

        $("#lstStatus").val(7);
        $("#lstStatus").prop("disabled", "true");

        $("#frmEMRRequestModalLabel").text("Create");
        $("#frmEMRRequest").modal("show");
    });

    //filling edit form
    function fillEditForm(purchaseOrderID) {

        $.ajax({
            method: "POST",
            url: "/Order/GetPurchaseOrder",
            dataType: "JSON",
            data: { purchaseOrderID: purchaseOrderID },
            success: function (data) {

                FillForm(data);

                $("#frmPurchaseOrders").modal("show");
            }
        });

    }
    function DeleteOrder(purchaseOrderID) {

        $.ajax({
            method: "POST",
            url: "/Order/DeletePurchaseOrder",
            dataType: "JSON",
            data: { purchaseOrderID: purchaseOrderID },
            success: function (data) {
                purchaseOrdersList.ajax.reload();
            }
        });

    }

    //filling edit form
    function FillForm(data) {


        $("#txtPurchaseOrderID").val(data.purchaseOrderID);
        $("#txtDateCreated").val(data.dateCreated);
        $("#txtDatePlaced").val(data.datePlaced);
        $("#txtDateFulfilled").val(data.dateFulfilled);
        $("#txtInvoiceDate").val(data.invoiceDate);

        $("#lstVendorID").val(data.vendorID);
        $("#lstVendorID").trigger("change");

        //item tables 
        $("#Items tr").remove();

        for (let i = 0; i < data.orderLines.length; i++) {

            $("#Items").append("<tr  purchaseorderlineid='" + data.orderLines[i].purchaseOrderLineID + "' vendoritemid='" + data.orderLines[i].vendorItemID + "'><td>" + data.orderLines[i].vendorItemID + "</td><td>" + data.orderLines[i].vendorItemName + "</td><td><input class='form-control ordered-qty' value='" + data.orderLines[i].orderedQty + "' /></td><td><input value='" + data.orderLines[i].unitCost + "' class='form-control unit-cost' readonly/></td><td><input value='" + data.orderLines[i].taxRate + "' class='form-control tax-rate' readonly/></td><td><input value='0' class='form-control total-cost' readonly/></td><td><input value='" + data.orderLines[i].receivedQty + "' class='form-control received-qty'/><td><input value='" + data.orderLines[i].backOrderQty + "' class='form-control backorder-qty'/></td><td><input value='" + data.orderLines[i].cancelledQty + "' class='form-control cancelled-qty'/></td><td></td><td></td><td></td><td><span class='btn btn-danger item-del'>X</span></td><tr>");


        }

        $("span.item-del").unbind("click").on("click", function (event) {
            $(event.target).closest("tr").remove();
        });

        //footer
        $("#txtShipTo").val(data.shippTo);
        $("#txtOrderNote").val(data.orderNote);
        $("#txtMemo").val(data.memo);

        RecalcTotalCost();

        $("#Items input.ordered-qty").change(function () {
            RecalcTotalCost();
        });
    }

    function FormControlsDisable() {

        $("#frmPurchaseOrders input").prop("disabled", true);
        $("#frmPurchaseOrders textarea").prop("disabled", true);
        $("#frmPurchaseOrders select").prop("disabled", true);
        $("#frmPurchaseOrders button").prop("disabled", true);

        $("button.btn-close").prop("disabled", false);
        $("button.close").prop("disabled", false);

    }

    function FormControlsEnable() {

        $("#frmPurchaseOrders input").prop("disabled", false);
        $("#frmPurchaseOrders textarea").prop("disabled", false);
        $("#frmPurchaseOrders select").prop("disabled", false);
        $("#frmPurchaseOrders button").prop("disabled", false);

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

        $.each($("#frmPurchaseOrders form input"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmPurchaseOrders form textarea"), function (index, item) {
            $(item).val("");
        });

        $.each($("#frmPurchaseOrders form select"), function (index, item) {
            $(item).val("");
        });

        $("#Items tr").remove();
    }
    //recalc
    function RecalcTotalCost() {
        $.each($("#Items tr"), function (index,item) {

            let unitCost = $(item).find("input.unit-cost").val();
            let orderedQty = $(item).find("input.ordered-qty").val();
            let txtTotalCost = $(item).find("input.total-cost");

            let totalCostRow = parseInt(unitCost) * parseFloat(orderedQty);

            $(txtTotalCost).val(totalCostRow);
        });

        //total for order
        let totalOrder = 0;

        $.each($("#Items input.total-cost"), function (index,item) {
            totalOrder += parseFloat($(item).val());
        });

        $("#txtTotalCost").val(totalOrder);
    }

    //button Ok modal form
    $("#btnSave").click(function () {

        let purchaseOrderID = $("#txtPurchaseOrderID").val();
        let vaendorID = $("#lstVendorID").val();

        //ordered lines
        let orderLineIds = $("#Items tr").map(function () {
            return $(this).attr("purchaseorderlineid");
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

        let committedQty = $("#Items input.tax-rate").map(function () {
            return $(this).val() == "" ? "0" : $(this).val();
        }).get();

        let cancelledQty = $("#Items input.cancelled-qty").map(function () {
            return $(this).val() == "" ? "0" : $(this).val();
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

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});