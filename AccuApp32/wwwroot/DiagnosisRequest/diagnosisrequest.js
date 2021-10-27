var assignMentTable;
var responseData;
var responseEmployeeData;
var responseCodeData;
var DiagnosisReustDetailsTable;

$(function () {

    //list accountrequests
    var diagnosisRequestList = $("#tblAccounts").DataTable({
        serverSide: true,
        processing: true,
        rowId: "AccountID",
        ajax: {
            url: "/DiagnosisRequest/GetAccountsListJson",
            type: "POST",
            dataSrc: "data"
        },
        columns: [
            { data: "AccountID" },
            { data: "Name" },
            { data: "GroupID" },
            { data: "SubGroup" },
            { data: "Telephone" },
            { data: "OpenCount",sortable:false },
            {
                data: null,
                sortable: false,
                defaultContent: "<button class='btn btn-secondary btn-details'>Details</button>"
            }
        ]
    });

    //binding event to button Details
    diagnosisRequestList.on('draw', function () {
        $("#tblAccounts button.btn-details").click(function (event) {
            let accountID = $(event.target).closest("tr").attr("id");
            fillDetailsForm(accountID);
        });
    });

    $("#frmAccountDetails").on("hidden.bs.modal", function () {
        DiagnosisReustDetailsTable.destroy();
    });

    function fillDetailsForm(AccountID) {
        $.ajax({
            method: "POST",
            url: "/DiagnosisRequest/GetAccountDiagnosisRequest",
            dataType: "JSON",
            data: { AccountID: AccountID },
            success: function (data) {
                FillDetailsForm(data);
                $("#frmAccountDetails").modal("show");
            }
        });
    }
    //filling details form
    function FillDetailsForm(data) {

        $("#txtAccountID").val(data.Account.AccountID);
        $("#txtName").val(data.Account.Name);
        $("#txtAddress").val(data.Account.Address);
        $("#txtTelephone").val(data.Account.Telephone);
        $("#txtGroupID").val(data.Account.GroupID);
        $("#txtSubGroup").val(data.Account.SubGroup);

        $("#tblDiagnosisRequests tbody>tr").remove();

        for (let i = 0; i < data.DiagnosisRequest.length; i++) {
            let row = data.DiagnosisRequest[i];
            $("#tblDiagnosisRequests tbody").append("<tr id='"+row.DiagnosisRequestID+"'><td>" + row.DiagnosisRequestID + "</td><td>" + row.PhysicianID + "</td><td>" + row.AccountID + "</td><td>" + row.AccessionNumber + "</td><td>" + row.FirstName + "</td><td>" + row.LastName + "</td><td>" + row.DOB + "</td><td>" + row.RequestDate + "</td><td>" + row.LastPrinted + "</td><td>" + row.LastPrintedBy + "</td><td><button type='button' class='btn btn-secondary'>Edit</button></td><td><button type='button' class='btn btn-secondary'>Edit</button></td></tr>");
        }

        $("#tblDiagnosisRequests tbody button").click(function () {

            let DiagnosisRequestID = $(this).closest("tr").attr("id");
            fillEditFormDiagnosisRequest(DiagnosisRequestID);
            $("#frmDiagnosisReq").modal("show");
        });

        DiagnosisReustDetailsTable = $("#tblDiagnosisRequests").DataTable({
            "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
        });
    }

    function fillEditFormDiagnosisRequest(DiagnosisRequestID) {
        $.ajax({
            url: "/DiagnosisRequest/GetDiagnosisRequestLines",
            method: "POST",
            dataType: "JSON",
            data: { DiagnosisRequestID: DiagnosisRequestID},
            success: function (data) {
                fillEditFormDiagnosisRequestLines(data);
            }
        });
    }

    function fillEditFormDiagnosisRequestLines(data) {

        $("#txtDiagnosisRequestID1").val(data.DiagnosisRequestID);
        $("#txtAccountID1").val(data.AccountID);
        $("#txtName1").val(data.Name1);
        $("#txtAddress1").val(data.Address);
        $("#txtTelephone1").val(data.Telephone);
        $("#txtPhysicianID1").val(data.PhysicianID);
        $("#txtName2").val(data.Name2);
        $("#txtNPI1").val(data.NPI);
        $("#txtAccessionNumber1").val(data.AccessionNumber);
        $("#txtServiceDate1").val(data.ServiceDate);
        $("#txtRequestDate1").val(data.RequestDate);
        $("#txtFirstName1").val(data.FirstName);
        $("#txtLastName1").val(data.LastName);
        $("#txtDOB1").val(data.DOB);

        let sText = "By signing below, " + (data.PhysicianID == 0 ? data.Name1:data.Name2) + " NPI:" + data.NPI + " affirm that the above information accurately reflects the the patient's condition.";
        $("#signingTitleText").text(sText);

        $("#tblDiagnosisRequestLines tbody>tr").remove();

        for (let i = 0; data.DiagnosisRequestLines[i]; i++) {
            let row = data.DiagnosisRequestLines[i];
            $("#tblDiagnosisRequestLines tbody").append("<tr DiagnosisRequestID='" + row.DiagnosisRequestID + "'  DiagnosisRequestLineID='" + row.DiagnosisRequestLineID+"'><td>" + row.UncoveredCode + "</td><td>" + row.CommonlyUsedDiagnoses+"</td></tr>");
        }


        $("#tblDiagnosisHistoryLines tbody>tr").remove();
        $("#tblAddCodeList tbody>tr").remove();

        for (let i = 0; data.DiagnosisHistoryLines[i]; i++) {
            let row = data.DiagnosisHistoryLines[i];
            $("#tblDiagnosisHistoryLines tbody").append("<tr DiagnosisRequestID='" + row.DiagnosisRequestID + "'  DiagnosisRequestLineID='" + row.DiagnosisHistoryLineID + "'><td>" + row.Code + "</td><td>" + row.Description + "</td></tr>");
        }



    }
    $("#txtCodeSearch").keypress(function (event) {
        if (event.keyCode != "13")
            return;
        if ($("#txtCodeSearch").val() == "")
            return;

        $.ajax({
            method: "POST",
            url: "/DiagnosisRequest/GetCodeListJson",
            dataType: "JSON",
            data: { searchValue: $("#txtCodeSearch").val() },
            success: function (data) {

                responseCodeData = data;
                $("#selectCodeList option").remove();
                $("#selectCodeList").append("<option value=''>--</option>");
                for (let i = 0; i < data.length; i++) {
                    $("#selectCodeList").append("<option value='" + data[i].Code + "'>" + data[i].Code + " " + data[i].Description + "</option>");
                }
            }
        })

        event.preventDefault();

    });
    //dropdown select
    $("#selectCodeList").change(function () {
        var codeid = $(this).find("option:selected").val();
        let selectedItem = responseCodeData.find(function (item) {
            return item.Code == codeid;
        });
        $("#tblAddCodeList tbody").append("<tr ><td>" + selectedItem.Code + "</td><td>" + selectedItem.Description + "</td>" + "<td><button class='btn btn-secondary delete' alt='delete records'>X</button>");
        $("#tblAddCodeList tbody>tr:last button.delete").click(function (event) {
            if (event.keyCode != "13")
                return;
            $(this).closest("tr").remove();
        });

    });



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
                data: { type: type, searchValue: search },
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
    $("#btnCreateOk1").click(function () {

        //preparing date
        let DiagnosisRequestID = $("#txtDiagnosisRequestID1").val();
        let esign = $("#esign").val();
        let DiagnosisLines = $("#tblAddCodeList tbody tr").map(function () {
            return $(this).find("td:first").text();
        }).get();


        $.ajax({
            url: "/DiagnosisRequest/SaveDiagnosisReuestLines",
            method: "POST",
            dataType: "JSON",
            data: { DiagnosisRequestID: DiagnosisRequestID, Codes: DiagnosisLines, ElectronicSign: esign},
            success: function (data) {
                if (data == "OK") {
                    $("#frmDiagnosisReq").modal("hide");
                    //frmCreateFormClear();
                }
            }
        });
    });

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });

    var signaturePad = new SignaturePad(document.getElementById('signature-pad'), {
        backgroundColor: 'rgba(255, 255, 255, 0)',
        penColor: 'rgb(0, 0, 0)'
    });

    $('#btnClearSignaturePad').click(function () {
        signaturePad.clear();
    });

});