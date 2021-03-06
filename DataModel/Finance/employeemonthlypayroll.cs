// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class employeemonthlypayroll
    {
        [Column("Payroll Company Code")]
        [StringLength(50)]
        public string Payroll_Company_Code { get; set; }
        [Column("Pay Date")]
        [StringLength(50)]
        public string Pay_Date { get; set; }
        [Column("File Number")]
        [StringLength(50)]
        public string File_Number { get; set; }
        [Column("Payroll Name")]
        [StringLength(50)]
        public string Payroll_Name { get; set; }
        [Column("Worked In Department")]
        [StringLength(50)]
        public string Worked_In_Department { get; set; }
        [Column("Worked In Department Description")]
        [StringLength(50)]
        public string Worked_In_Department_Description { get; set; }
        [Column("Regular Hours Total")]
        [StringLength(50)]
        public string Regular_Hours_Total { get; set; }
        [Column("Regular Earnings Total", TypeName = "money")]
        public decimal? Regular_Earnings_Total { get; set; }
        [Column("Overtime Hours Total")]
        [StringLength(50)]
        public string Overtime_Hours_Total { get; set; }
        [Column("Overtime Earnings Total")]
        [StringLength(50)]
        public string Overtime_Earnings_Total { get; set; }
        [Column("Bereavement Hours")]
        [StringLength(50)]
        public string Bereavement_Hours { get; set; }
        [Column("Bereavement $")]
        [StringLength(50)]
        public string Bereavement__ { get; set; }
        [Column("Bonus Hours")]
        [StringLength(50)]
        public string Bonus_Hours { get; set; }
        [Column("Bonus$", TypeName = "money")]
        public decimal? Bonus_ { get; set; }
        [Column("Holiday Hours")]
        [StringLength(50)]
        public string Holiday_Hours { get; set; }
        [Column("Holiday $")]
        [StringLength(50)]
        public string Holiday__ { get; set; }
        [Column("PTO Hours")]
        [StringLength(50)]
        public string PTO_Hours { get; set; }
        [Column("PTO $")]
        [StringLength(50)]
        public string PTO__ { get; set; }
        [Column("Retro Hours")]
        [StringLength(50)]
        public string Retro_Hours { get; set; }
        [Column("Retro $")]
        [StringLength(50)]
        public string Retro__ { get; set; }
        [Column("Severance $")]
        [StringLength(50)]
        public string Severance__ { get; set; }
        [Column("Gross Pay", TypeName = "money")]
        public decimal? Gross_Pay { get; set; }
        [Column("Social Security - Employee Tax")]
        [StringLength(50)]
        public string Social_Security___Employee_Tax { get; set; }
        [Column("Medicare - Employee Tax")]
        [StringLength(50)]
        public string Medicare___Employee_Tax { get; set; }
        [Column("Federal Income - Employee Tax")]
        [StringLength(50)]
        public string Federal_Income___Employee_Tax { get; set; }
        [Column("State Tax")]
        [StringLength(50)]
        public string State_Tax { get; set; }
        [Column("Local Tax")]
        [StringLength(50)]
        public string Local_Tax { get; set; }
        [Column("Sui EE")]
        [StringLength(50)]
        public string Sui_EE { get; set; }
        [Column("401K")]
        [StringLength(50)]
        public string _401K { get; set; }
        [Column("401K Catch up")]
        [StringLength(50)]
        public string _401K_Catch_up { get; set; }
        [StringLength(50)]
        public string Afllac { get; set; }
        [StringLength(50)]
        public string Medical { get; set; }
        [StringLength(50)]
        public string Dental { get; set; }
        [StringLength(50)]
        public string Vision { get; set; }
        [StringLength(50)]
        public string GAP { get; set; }
        [StringLength(50)]
        public string Garnish { get; set; }
        [Column("Loan 1")]
        [StringLength(50)]
        public string Loan_1 { get; set; }
        [Column("Loan 2 - Repay")]
        [StringLength(50)]
        public string Loan_2___Repay { get; set; }
        [Column("Expense 1")]
        [StringLength(50)]
        public string Expense_1 { get; set; }
        [Column("Expense 2")]
        [StringLength(50)]
        public string Expense_2 { get; set; }
        [Column("Direct Deposit")]
        [StringLength(50)]
        public string Direct_Deposit { get; set; }
        [Column("Net Pay")]
        [StringLength(50)]
        public string Net_Pay { get; set; }
    }
}