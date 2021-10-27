var editor;
var actionOptions = [
    { label: "", value: "" },
    { label: "Address Changed", value: "Address Changed" },
    { label: "NPI Denial", value: "NPI Denial" },
    { label: "Waiver of Liability", value: "Waiver of Liability" },
    { label: "Dr. is not in PECOS", value: "Dr. is not in PECOS" },
    { label: "Mod 90 Denial", value: "Mod 90 Denial" },
    { label: "Not Covered Pt Plan", value: "Not Covered Pt Plan" },
    { label: "1st Level Appeal Sent", value: "1st Level Appeal Sent" },
    { label: "Appeal to Medicare Secondary", value: "Appeal to Medicare Secondary" },
    { label: "Check Sent to Pt", value: "Check Sent to Pt" },
    { label: "Sent Back for Review", value: "Sent Back for Review" },
    { label: "No Active Insurance", value: "No Active Insurance" },
    { label: "Appeal In Process", value: "Appeal In Process" },
    { label: "COB Denial", value: "COB Denial" },
    { label: "Medical Records Needed", value: "Medical Records Needed" },
    { label: "Requisition is not signed", value: "Requisition is not signed" },
    { label: "Pt Incarcerated", value: "PT Incarcerated" },
    { label: "Facility Stay", value: "Facility Stay" },
    { label: "Medical Records Sent to Ins", value: "Medical Records Sent to Ins" },
    { label: "UNPAID CODES DENIED DUE TO SERVICE EXEEDING ACCEPTABLE MAXIMUM PER DAY", value: "UNPAID CODES DENIED DUE TO SERVICE EXEEDING ACCEPTABLE MAXIMUM PER DAY" },
    { label: "UNPAID DRUG SCREEN CODES DENIED DUE TO MISSING AUTHORIZATION", value: "UNPAID DRUG SCREEN CODES DENIED DUE TO MISSING AUTHORIZATION" },
    { label: "Claim has been applied to patients copayment", value: "Claim has been applied to patients copayment" },
    { label: "CLAIM APPLIED TO CO-INSURANCE", value: "CLAIM APPLIED TO CO-INSURANCE" },
    { label: "Applied to Ded.", value: "Applied to Ded." },
    { label: "Chk Paid to PT. Co-ins/Ded", value: "Chk Paid to PT. Co-ins/Ded" },
    { label: "BILLED TO SECONDARY INSURANCE", value: "BILLED TO SECONDARY INSURANCE" },
    { label: "CHANGED BILL TYPE", value: "CHANGED BILL TYPE" },
    { label: "MAXIMUM BENEFITS HAS BEEN REACHED", value: "MAXIMUM BENEFITS HAS BEEN REACHED" },
    { label: "REF PHYSICIAN IS NOT PARTICIPATING", value: "REF PHYSICIAN IS NOT PARTICIPATING" },
    { label: "INVALID/MISSING REFERR.PROVIDER", value: "INVALID/MISSING REFERR.PROVIDER" },
    { label: "CPT CODE NOT COVERED/BUNDLED", value: "CPT CODE NOT COVERED/BUNDLED" },
    { label: "PT NEEDS TO UPDATE COORDINATION OF BENEFITS", value: "PT NEEDS TO UPDATE COORDINATION OF BENEFITS" },
    { label: "PATIENT ID/NAME DOES NOT MATCH/ WRONG INS INFO", value: "PATIENT ID/NAME DOES NOT MATCH/ WRONG INS INFO" },
    { label: "INAPPROPRIATE MODIFIER", value: "INAPPROPRIATE MODIFIER" },
    { label: "NEED LETTER OF MEDICAL NECESSITY", value: "NEED LETTER OF MEDICAL NECESSITY" },
    { label: "NEED MEDICAL RECORDS", value: "NEED MEDICAL RECORDS" },
    { label: "NEED TO CHANGE NUMBER OF UNITS", value: "NEED TO CHANGE NUMBER OF UNITS" },
    { label: "CLAIM IS NOT ON FILE", value: "CLAIM IS NOT ON FILE" },
    { label: "NEEDS SUPPORTING DOCUMENTATION", value: "NEEDS SUPPORTING DOCUMENTATION" },
    { label: "OVERPAID", value: "OVERPAID" },
    { label: "FIXED NEGATIVE BALANCES IN OUR SYSTEM", value: "FIXED NEGATIVE BALANCES IN OUR SYSTEM" },
    { label: "COURT", value: "COURT" },
    { label: "PATIENT RECEIVED CHECK", value: "PATIENT RECEIVED CHECK" },
    { label: "CLAIM WAS/WILL BE PAID BY INS", value: "CLAIM WAS/WILL BE PAID BY INS" },
    { label: "STATEMENT SENT TO PATIENT", value: "STATEMENT SENT TO PATIENT" },
    { label: "PATIENT PAID BY CC/ MONEY ORDER/ CHECK", value: "PATIENT PAID BY CC/ MONEY ORDER/ CHECK" },
    { label: "LEFT MESSAGE TO PATIENT/INS/DR OFFICE", value: "LEFT MESSAGE TO PATIENT/INS/DR OFFICE" },
    { label: "CLAIM IS PENDING", value: "CLAIM IS PENDING" },
    { label: "PRE-EXISTING CONDITION", value: "PRE-EXISTING CONDITION" },
    { label: "PATIENT SENDING PAYMENT", value: "PATIENT SENDING PAYMENT" },
    { label: "INS PAYMENT POSTED", value: "INS PAYMENT POSTED" },
    { label: "CALL MADE/CALL RECIEVED", value: "CALL MADE/CALL RECIEVED" },
    { label: "RETURNED CHECK TO PT/DR/INS", value: "RETURNED CHECK TO PT/DR/INS" },
    { label: "CLAIM SUBMITTED/SENT FOR REPROCESSING", value: "CLAIM SUBMITTED/SENT FOR REPROCESSING" },
    { label: "SENT FAX", value: "SENT FAX" },
    { label: "TIMELY FILING", value: "TIMELY FILING" },
    { label: "NO AUTHORIZATION/NP PROVIDER", value: "NO AUTHORIZATION/NP PROVIDER" },
    { label: "Paid By Insurance", value: "Paid By Insurance" }
];

$(function () {


    $("#btnMarkAsWork").click(function () {

        let actions = [];
        let invoices = [];

        $.each($("input[type=checkbox].mark-as-worked"), function (i, item) {
            if ($(item).prop("checked")) {
                let select = $(item).closest("tr").find("select.action-taken");
                let inv = $(item).closest("tr").find("td:eq(3)").text();

                actions.push(select.val())
                invoices.push(inv);
            }
        });
        //send mark to server

        $.post({
            url: "/billingTools/MarkInvoice",
            dataType: "JSON",
            data: { actions: actions, invoices: invoices, users:""}
        })
    });

    $.get({
        url: "/BillingTools/GetUsersJson",
        dataType: "JSON",
        async:false,
        success: function (data) {
            $.each(data, function () {
                $("#Users").append("<option value='" + this.username+"'>"+this.name+"</option>");
            });
        }
    });

    var alertsList = $("#Alerts").DataTable({
        //dom: "Bfrtip",
        serverSide: true,
        processing: true,
        rowId: "Invoice",
        ajax: {
            url: "/BillingTools/GetAlertsJson",
            type: "POST",
            dataSrc: "data"
        },
        pageLength:1000,
        orderCellsTop: true,
        fixedHeader: true,
        columns: [
            {
                data: null,
                searchable: false,
                sortable: false,
                defaultContent: "<input type='checkbox' class='mark-as-worked' />"
            },
            {
                data:null,
                searchable: false,
                sortable: false,
                render: function (data, type, row) {

                    var select = $("<select class='action-taken'></select>");

                    $.each(actionOptions, function (i, item) {
                        select.append($("<option></option>").attr("value", actionOptions[i].value).text(actionOptions[i].label));
                    });     

                    return $(select).prop("outerHTML");
                }
            },
            { data: "DateAlertCreated", sortable: false },
            { data: "Invoice",sortable:false },
            { data: "InsuranceName",sortable:false },
            { data: "OldBalance",sortable:false },
            { data: "NewBalance",sortable:false },
            { data: "User",sortable:false },
            { data: "Reason",sortable:false }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        }
    });
});