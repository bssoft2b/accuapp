// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class temp_AndysRequest
    {
        public string Accession { get; set; }
        [Column("Patient ID")]
        public string Patient_ID { get; set; }
        [Column("Patient Chart")]
        public string Patient_Chart { get; set; }
        [Column("Social Security #")]
        public string Social_Security__ { get; set; }
        [Column("Patient Last Name")]
        public string Patient_Last_Name { get; set; }
        [Column("Patient First Name")]
        public string Patient_First_Name { get; set; }
        public string MI { get; set; }
        [Column("Patient Address")]
        public string Patient_Address { get; set; }
        [Column("Patient City")]
        public string Patient_City { get; set; }
        [Column("Patient State")]
        public string Patient_State { get; set; }
        [Column("Patient Zip")]
        public string Patient_Zip { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        [Column("Patient Phone")]
        public string Patient_Phone { get; set; }
        [Column("Injury Date")]
        public string Injury_Date { get; set; }
        [Column("Subscriber Last Name")]
        public string Subscriber_Last_Name { get; set; }
        [Column("Subsc. First Name")]
        public string Subsc__First_Name { get; set; }
        [Column("Subsc. Address")]
        public string Subsc__Address { get; set; }
        [Column("Subsc. City")]
        public string Subsc__City { get; set; }
        [Column("Subsc. State")]
        public string Subsc__State { get; set; }
        [Column("Subsc. Zip")]
        public string Subsc__Zip { get; set; }
        [Column("Subsc. Sex")]
        public string Subsc__Sex { get; set; }
        [Column("Subsc. DOB")]
        public string Subsc__DOB { get; set; }
        [Column("Subsc. Phone")]
        public string Subsc__Phone { get; set; }
        [Column("Subsc. Relation")]
        public string Subsc__Relation { get; set; }
        public string Employer { get; set; }
        [Column("Client ID")]
        public string Client_ID { get; set; }
        [Column("Client Name")]
        public string Client_Name { get; set; }
        [Column("Phys. ID")]
        public string Phys__ID { get; set; }
        [Column("Phys. Name")]
        public string Phys__Name { get; set; }
        [Column("Order Billtype")]
        public string Order_Billtype { get; set; }
        [Column("Billtype Name")]
        public string Billtype_Name { get; set; }
        [Column("Billtype Phone")]
        public string Billtype_Phone { get; set; }
        public string Policy { get; set; }
        public string Group { get; set; }
        [Column("Insurance Covered?")]
        public string Insurance_Covered_ { get; set; }
        [Column("Current Primary Ins")]
        public string Current_Primary_Ins { get; set; }
        [Column("Ins Name")]
        public string Ins_Name { get; set; }
        [Column("Ins Class")]
        public string Ins_Class { get; set; }
        [Column("Ins Address")]
        public string Ins_Address { get; set; }
        [Column("Ins City")]
        public string Ins_City { get; set; }
        [Column("Ins State")]
        public string Ins_State { get; set; }
        [Column("Ins Zip")]
        public string Ins_Zip { get; set; }
        [Column("Ins Phone")]
        public string Ins_Phone { get; set; }
        [Column("Policy#")]
        public string Policy_ { get; set; }
        [Column("Group 2")]
        public string Group_2 { get; set; }
        [Column("Current Secondary Ins")]
        public string Current_Secondary_Ins { get; set; }
        [Column("Ins Name 2")]
        public string Ins_Name_2 { get; set; }
        [Column("Ins Class 2")]
        public string Ins_Class_2 { get; set; }
        [Column("Ins Address 2")]
        public string Ins_Address_2 { get; set; }
        [Column("Ins City 2")]
        public string Ins_City_2 { get; set; }
        [Column("Ins State 2")]
        public string Ins_State_2 { get; set; }
        [Column("Ins Zip 2")]
        public string Ins_Zip_2 { get; set; }
        [Column("Ins Phone 2")]
        public string Ins_Phone_2 { get; set; }
        [Column("Policy# 2")]
        public string Policy__2 { get; set; }
        [Column("Group 3")]
        public string Group_3 { get; set; }
        [Column("Order Entered By (ID)")]
        public string Order_Entered_By__ID_ { get; set; }
        [Column("Entry Date")]
        public string Entry_Date { get; set; }
        [Column("Collection Date")]
        public string Collection_Date { get; set; }
        [Column("Final Report Date")]
        public string Final_Report_Date { get; set; }
        public string SalesGrp1 { get; set; }
        [Column("SalesGrp1 Name")]
        public string SalesGrp1_Name { get; set; }
        public string SalesGrp2 { get; set; }
        [Column("SalesGrp2 Name")]
        public string SalesGrp2_Name { get; set; }
        public string SalesGrp3 { get; set; }
        [Column("SalesGrp3 Name")]
        public string SalesGrp3_Name { get; set; }
        public string SalesGrp4 { get; set; }
        [Column("SalesGrp4 Name")]
        public string SalesGrp4_Name { get; set; }
        [Column("Prescribed Medications")]
        public string Prescribed_Medications { get; set; }
        public string Diag1 { get; set; }
        public string Diag2 { get; set; }
        public string Diag3 { get; set; }
        public string Diag4 { get; set; }
        public string Diag5 { get; set; }
        public string Diag6 { get; set; }
        [Column("Bill On Hold")]
        public string Bill_On_Hold { get; set; }
        [Column("Spec. Source")]
        public string Spec__Source { get; set; }
        [Column("Complete?")]
        public string Complete_ { get; set; }
        [Column("Order #")]
        public string Order__ { get; set; }
        [Column("Date/Time Verified")]
        public string Date_Time_Verified { get; set; }
        [Column("Test Code")]
        public string Test_Code { get; set; }
        [Column("Test Name")]
        public string Test_Name { get; set; }
        [Column("Test Type")]
        public string Test_Type { get; set; }
        [Column("Sendout Labs for Order")]
        public string Sendout_Labs_for_Order { get; set; }
        [Column("Depts for Order")]
        public string Depts_for_Order { get; set; }
    }
}