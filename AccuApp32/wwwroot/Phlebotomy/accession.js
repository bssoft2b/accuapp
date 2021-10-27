var assignMentTable;
var responseData;
var responseEmployeeData;

$(function () {

    let date1 = new Date();
    let dat1 = moment(date1).add(-7, 'days').format("YYYY-MM-DD");
    $('#fromDate').val(dat1);

    let date2 = new Date();
    let dat2 = moment(date2).format("YYYY-MM-DD");
    $('#toDate').val(dat2);


    $("#fromDate").change(function () {
        accessionList.ajax.reload();
    });

    $("#toDate").change(function () {
        accessionList.ajax.reload();
    });


    //acession list
    var accessionList = $("#tblAccessions").DataTable({
        dom: "lpfrtip",
        serverSide: true,
        processing: true,
        searching:false,
        rowId: "AccessionId",
        ajax: {
            url: "/Phlebotomy/AccessionListJson",
            type: "POST",
            dataSrc: "data",
            data: function (d) {
                d.date1 = $("#fromDate").val(),
                d.date2 = $("#toDate").val()
            }
        },
        columns: [
            {
                data: "Status",
                sortable: false
            },
            { data: "AccountId"},
            { data: "PatientName" },
            { data: "PatientDOB" },
            { data: "PatientGender" },
            { data: "InsuranceName" },
            { data: "InsuranceId" },
            { data: "Note",sortable:false },
            {
                data: "Status1",
                sortable:false,
                render: function (data, type, row, meta) {
                    return data ? "<input type='checkbox' checked readonly disabled >" : "<input type='checkbox' readonly disabled>";
                }
            },
            {
                data: "Status2",
                sortable: false,
                render: function (data, type, row, meta) {
                    return data ? "<input type='checkbox' checked readonly disabled >" : "<input type='checkbox' readonly disabled>";
                }
            },
            {
                data: "Status3",
                sortable: false,
                render: function (data, type, row, meta) {
                    return data ? "<input type='checkbox' checked readonly disabled >" : "<input type='checkbox' readonly disabled>";
                }
            },
            {
                data: "Status4",
                sortable: false,
                render: function (data, type, row, meta) {
                    return data ? "<input type='checkbox' checked readonly disabled >" : "<input type='checkbox' readonly disabled>";
                }
            },
            { data: "LastModifiedBy" },
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
            }
        ]
    });

    //binding event to button Edit and Details
    accessionList.on('draw', function () {
        $("#tblAccessions button.btn-edit").click(function (event) {
            let accessionID = $(event.target).closest("tr").attr("id");
            $("#frmAccessionModalLabel").text("Edit");
            ControlsDisable(false);
            fillEditForm(accessionID);
        });
        $("#tblAccessions button.btn-details").click(function (event) {
            let accessionID = $(event.target).closest("tr").attr("id");

            ControlsDisable(true);

            $("#frmAccessionModalLabel").text("Details");
            fillEditForm(accessionID);
        });
    });

    //enable/disable controls
    function ControlsDisable(toggle) {

        $.each($("#frmAccession form input"), function (index, item) {
            $(item).prop("disabled", toggle);
        });
        $.each($("#frmAccession form textarea"), function (index, item) {
            $(item).prop("disabled", toggle);
        });

    }

    //binding event to button Create new
    $("#btnCreateNew").click(function () {
        ControlsDisable(false);
        frmFormClear();
        $("#frmAccessionModalLabel").text("Create");
        $("#frmAccession").modal("show");
    });

    //filling edit form
    function fillEditForm(accessionId) {
        $.ajax({
            method: "POST",
            url: "/Phlebotomy/GetAccession",
            dataType: "JSON",
            data: { accessionId: accessionId },
            success: function (data) {
                FillForm(data);
                $("#frmAccession").modal("show");
            }
        });
    }

    //filling edit form
    function FillForm(data) {

        frmFormClear();

        $("#txtAccessionId").val(data.AccessionId);
        $("#txtAccountId").val(data.AccountId);
        $("#txtPatientName").val(data.PatientName);
        $("#txtPatientDOB").val(moment(data.PatientDOB).format("YYYY-MM-DD"));
        $("#txtInsuranceName").val(data.InsuranceName);
        $("#txtInsuranceId").val(data.InsuranceId);
        $("#txtNote").val(data.Note);
        $("#chkStat1").prop("checked", data.Stat1);
        $("#chkStat2").prop("checked", data.Stat2);
        $("#chkStat3").prop("checked", data.Stat3);
        $("#chkStat4").prop("checked", data.Stat4);

    }
    //filling details form

    //clear control element
    function frmFormClear() {

        $("#txtAccessionId").val("");
        $("#txtAccountId").val("");
        $("#txtPatientName").val("");
        $("#txtPatientDOB").val("");
        $("#txtInsuranceName").val("");
        $("#txtInsuranceId").val("");
        $("#txtNote").val("");
        $("#chkStat1").prop("checked", false);
        $("#chkStat2").prop("checked", false);
        $("#chkStat3").prop("checked", false);
        $("#chkStat4").prop("checked", false);

    }

    //button Ok modal form
    $("#btnSave").click(function () {

        //preparing date

        let accessionId = $("#txtAccessionId").val();
        let accountId = $("#txtAccountId").val();
        let patientName = $("#txtPatientName").val();
        let patientDOB = $("#txtPatientDOB").val();
        let insuranceName = $("#txtInsuranceName").val();
        let insuranceId = $("#txtInsuranceId").val();
        let note = $("#txtNote").val();
        let stat1 = $("#chkStat1").prop("checked");
        let stat2 = $("#chkStat2").prop("checked");
        let stat3 = $("#chkStat3").prop("checked");
        let stat4 = $("#chkStat4").prop("checked");

        $.ajax({
            url: "/Phlebotomy/SaveAcession",
            method: "POST",
            dataType: "JSON",
            data: {
                accessionId: accessionId,
                accountId: accountId,
                patientName: patientName,
                patientDOB: patientDOB,
                insuranceName: insuranceName,
                insuranceId: insuranceId,
                note: note,
                stat1: stat1,
                stat2: stat2,
                stat3: stat3,
                stat4: stat4
            },
            success: function (data) {
                if (data == "OK") {
                    accessionList.ajax.reload();
                    $("#frmAccession").modal("hide");
                    frmFormClear();
                }
            }
        });
    });

    //draggable
    $(".modal-dialog").draggable({
        handle: ".modal-header"
    });
});