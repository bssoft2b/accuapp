using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AccuApp32.Services;
using SendGrid.Helpers.Mail;

namespace AccuApp32MVC.Services
{
    public static class EmailSenderExtensions
    {
        //const string INTERFACEEMAIL = "interfaces@accureference.com";
        const string INTERFACEEMAIL = "interfaces@accureference.com";
        const string SumeshEMAIL = "sumesh.vija@accureference.com";
        const string AccountsEMAIL = "accounts@accureference.com";
        const string JulieEMAIL = "julie.olender@accureference.com";
        const string KristinaEMAIL = "kristina.neyman@accureference.com";
        const string MaryEMAIL = "Mary.Machevska@accureference.com";
        const string ZacEMAIL = "zac.arnold@accureference.com";
        //Phlebotomy or Floats
        public static Task SendFloatExpenseApproveRequest(this IEmailSender emailSender, string from, string to, DateTime completeDate, int expenseID, string email)
        {
            return emailSender.SendEmailAsync(email, "Missing Expense Documentation", $"Please provide supporting documentation or receipts for your stop " + from + " to " + to + " on " + completeDate + ", using the FloatExpenses Page of AccuApp at https://accuapp.accureference.com/Phlebotomy/FloatExpenses/Edit?id=" + expenseID + " .");
        }

        //Client Services
        public static Task SendEmailAssignedNotification(this IEmailSender emailSender, string name, string sendTo, string email, string notes, string status)
        {
            return emailSender.SendEmailAsync(sendTo, "DispatchTask for account : " + name + " : was " + "assigned", "DispatchTask for account : " + name + " was " + "assigned by " + email + " ,current status is : " + status + " ,current note is : " + notes + ".");

        }
        public static Task SendEmailDispatchTasksNotification(this IEmailSender emailSender, string name, string sendTo, string email, string notes, string status, bool created)
        {
            return emailSender.SendEmailAsync(sendTo, "DispatchTask for account : " + name + " : was " + (created ? "created" : "modified"), "DispatchTask for account : " + name + " was " + (created ? "created" : "modified") + " by " + email + " ,current status is : " + status + " ,current note is : " + notes + ".");
        }

        //Administration
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.");
        }

        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }

        public static Task EmailConfirmedNotification(this IEmailSender emailSender, string email, string newUser, string url)
        {
            return emailSender.SendEmailAsync(email, "User Created",
                $"Please go into AccuApp and finish providing the adequate permissions for the new user: '{newUser}', using the following link. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Manage Permissions</a>.");
        }

        public static Task RolesConfirmedNotification(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "User Confirmed",
                $"An Administrator has finished providing the neccessary permissions for your account. You may now login and access any and all relevant features of the application..");
        }

        public static Task BlockUserNotification(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "User Blocked",
                $"Your account to AccuApp has been blocked. If you have questions as to why feel free to contact support@accureference.com .");
        }

        //Account Management
        public static Task SendAccountCreatedNotification(this IEmailSender emailSender, string email, string practice, string accounts)
        {
            return emailSender.SendEmailAsync(email, "Account Created",
                $"Your Account Creation Request for " + practice + " has been completed and assigned to account numbers: " + accounts + ". For more details please login to the application and check the completed account request.");
        }

        public static Task SendAccountRequestCreatedNotification(this IEmailSender emailSender, string email, string practice)
        {
            return emailSender.SendEmailAsync(INTERFACEEMAIL, "Account Request Placed",
                $"A New Account Request for practice: " + practice + ", has been placed by " + email + ". For more details please login to the application and check the new account requests page.");
        }

        public static Task SendAccountRequestCreatedNotificationWithFile(this IEmailSender emailSender, string email, string EmailBody, string practice, string Filecontents, string FileName, string contactEmail="",string contactEmail1="")
        {
            List<string> cc = new List<string>
            {
               SumeshEMAIL,
               JulieEMAIL,
               KristinaEMAIL,
               ZacEMAIL
            };

            if (!string.IsNullOrEmpty(contactEmail))
                cc.Add(contactEmail);

            if (!string.IsNullOrEmpty(contactEmail1))
                cc.Add(contactEmail1);

            foreach (var item in cc)
            {
                emailSender.SendEmailAsync(item, "Account Request Placed",
                $"A New Account Request for practice: " + practice + ", has been placed by " + email + " <br/> <br/> Please check more details below." + EmailBody, Filecontents, FileName);
            }
            return emailSender.SendEmailAsync(email, "Account Request Placed",
                $"A New Account Request for practice: " + practice + ", has been placed by " + email + " <br/> <br/> Please check more details below." + EmailBody, Filecontents, FileName);
        }

        public static Task SendEmrRequestCreatedNotification(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(INTERFACEEMAIL, "EMR Request Placed",
                $"A New EMR Request has been placed by " + email + ". For more details please login to the application and check the new emr requests page.");
        }

        public static Task SendEmrRequestCreatedNotification(this IEmailSender emailSender, string email, string EmailBody, string Filecontents, string FileName)
        {
            List<string> cc = new List<string>
            {
               SumeshEMAIL,
               INTERFACEEMAIL
            };
            foreach (var item in cc)
            {
                emailSender.SendEmailAsync(item, "EMR Request Placed", $"A New EMR Request has been placed by " + email + " <br/> <br/> Please check more details below." + EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
            }
            return emailSender.SendEmailAsync(email, "EMR Request Placed", $"A New EMR Request has been placed by " + email + " <br/> <br/> Please check more details below." + EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
        }

        public static Task SendEmrRequestCreatedConfirmation(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "EMR Request Placed",
                $"You have placed a new emr request. For more details please login to the application and check the new EMR requests page.");
        }

        public static Task SendEmrRequestModifiedNotification(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(INTERFACEEMAIL, "EMR Request Modified",
                $"The EMR Request has been modified by " + email + ". For more details please login to the application and check the new EMR requests page.");
        }

        public static Task SendEmrRequestModifiedNotification(this IEmailSender emailSender, string EmailBody, string EmrStatus, string email, string modifiedBy, string Filecontents = "", string FileName = "")
        {
            List<string> cc = new List<string>
            {
              INTERFACEEMAIL ,
               modifiedBy,
               SumeshEMAIL
            };

            if(EmrStatus.ToUpper() == "PENDING QUOTE APPROVAL")
            {
                cc.Add(MaryEMAIL);
            }

            foreach (var item in cc)
            {
                emailSender.SendEmailAsync(item, "EMR Request " + EmrStatus, EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
            }
            return emailSender.SendEmailAsync(email, "EMR Request " + EmrStatus, EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
        }

        //implement subscribe
        public static Task SendEmrRequestModifiedNotification(this IEmailSender emailSender, string email, string modifiedBy)
        {
            return emailSender.SendEmailAsync(email, "EMR Request Modified",
                $"The EMR Request has been modified by " + modifiedBy + ". For more details please login to the application and check the new emr requests page.");
        }

        //implement subscribe
        public static Task SendEmrRequestQuoteUploaded(this IEmailSender emailSender, string email, string modifiedBy)
        {
            return emailSender.SendEmailAsync(email, "EMR Request Quote Uploaded", $"The Quote for an EMR Request has been uploaded by " + modifiedBy + ". For more details please login to the application and check the new emr requests page, for requests in a \"Pending Quote Approval\" status.");
        }
        //implement subscribe
        public static Task SendEmrRequestQuoteApproved(this IEmailSender emailSender, string EmailBody, string EmrStatus, string email, string modifiedBy, string Filecontents = "", string FileName = "")
        {
            return emailSender.SendEmailAsync(email, "EMR Request Quote Approved", EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
        }
        //implement subscribe 
        public static Task SendEmrRequestDeniedNotification(this IEmailSender emailSender, string EmailBody, string EmrStatus, string email, string modifiedBy, string Filecontents = "", string FileName = "")
        {
            return emailSender.SendEmailAsync(email, "EMR Request Denied", EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
        }

        public static Task SendEmrRequestCompletedNotification(this IEmailSender emailSender, string EmailBody, string EmrStatus, string email, string modifiedBy, string Filecontents = "", string FileName = "")
        {
            //cc is not working for some reason so what I am doing is iterate for each user and send email to them
            List<string> cc = new List<string>
            {
              INTERFACEEMAIL ,
               modifiedBy,
               SumeshEMAIL
            };

            foreach (var item in cc)
            {
                emailSender.SendEmailAsync(item, "EMR Request " + EmrStatus, EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
            }
            return emailSender.SendEmailAsync(email, "EMR Request " + EmrStatus, EmailBody, (Filecontents != "" ? Filecontents : ""), (FileName != "" ? FileName : ""));
        }

        public static Task SendAccountRequestModifiedNotification(this IEmailSender emailSender, string email, string practice)
        {
            return emailSender.SendEmailAsync(INTERFACEEMAIL, "Account Request Modified",
                $"The Account Request for practice: " + practice + ", has been modified by " + email + ". For more details please login to the application and check the new account requests page.");
        }

        public static Task SendAccountRequestDeletedNotification(this IEmailSender emailSender, string email, string practice)
        {
            return emailSender.SendEmailAsync(INTERFACEEMAIL, "Account Request Deleted",
                $"The Account Request for practice: " + practice + ", has been deleted by " + email + ". Please note that the request is no longer valid and should not be processed or appear in the application.");
        }

        public static Task SendAccountRequestModifiedNotification(this IEmailSender emailSender, string email, string practice, string modifier)
        {
            return emailSender.SendEmailAsync(email, "Account Request Modified",
                $"The Account Request for practice: " + practice + ", has been modified by " + modifier + ". For more details please login to the application and check the new account requests page.");
        }

        public static Task SendAccountRequestModifiedNotificationWithFile(this IEmailSender emailSender, string email, string practice, string modifier, string Filecontents, string FileName,string ContactEmail="",string ContactEmail1="")
        {
            return emailSender.SendEmailAsync(email, "Account Request ",
                $"The Account Request for practice: " + practice + ", has been modified by " + modifier + ". For more details please login to the application and check the new account requests page.", Filecontents, FileName);
        }

        public static Task SendAccountRequestModifiedNotificationWithFile(this IEmailSender emailSender, string email, string Status, string EmailBody, string practice, string modifier, string Filecontents, string FileName, string ContactEmail = "", string ContactEmail1 = "")
        {
            Task tsk = null;

            if (!string.IsNullOrEmpty(ContactEmail))
            {
                tsk=emailSender.SendEmailAsync(ContactEmail, "Account Request: " + Status,
                $"The Account Request for practice: " + practice + ", has been modified by " + modifier + " <br/> <br/> Please check more details below." + EmailBody, Filecontents, FileName);
            }

            if (!string.IsNullOrEmpty(ContactEmail1))
            {
                tsk=emailSender.SendEmailAsync(ContactEmail1, "Account Request: " + Status,
                $"The Account Request for practice: " + practice + ", has been modified by " + modifier + " <br/> <br/> Please check more details below." + EmailBody, Filecontents, FileName);
            }

            return tsk;
        }

        public static Task SendAccountRequestCompletedNotification(this IEmailSender emailSender, string email, string practice, string modifier)
        {
            return emailSender.SendEmailAsync(email, "Account Request Completed",
                $"The Account Request for practice: " + practice + ", has been modified by " + modifier + ". For more details please login to the application and check the completed account requests page.");
        }

        public static Task SendAccountRequestCompletedNotificationWithFile(this IEmailSender emailSender, string email, string Status, string EmailBody, string practice, string modifier, string Filecontents, string FileName)
        {
            return emailSender.SendEmailAsync(email, "Account Request: " + Status,
                $"The Account Request for practice: " + practice + ", has been modified by " + modifier + "<br /> <br /> Please check more details below." + EmailBody, Filecontents, FileName);
        }

        public static Task SendAccountRequestDeniedNotification(this IEmailSender emailSender, string email, string practice, string modifier)
        {
            return emailSender.SendEmailAsync(email, "Account Request Denied",
                $"The Account Request for practice: " + practice + ", has been denied by " + modifier + ". For more details please login to the application and check the completed account requests page.");
        }

        public static Task SendAccountRequestDeniedNotificationWithFile(this IEmailSender emailSender, string email, string EmailBody, string practice, string modifier, string Filecontents, string FileName)
        {
            return emailSender.SendEmailAsync(email, "Account Request Denied",
                $"The Account Request for practice: " + practice + ", has been denied by " + modifier + "<br /> <br /> Please check more details below." + EmailBody, Filecontents, FileName);
        }

        public static Task SendAccessionPendingNotification(this IEmailSender emailSender, string id, string email, string url)
        {
            return emailSender.SendEmailAsync("coding@accureference.com", $"Accession Management Request: '{id}'", $"The accession request is in the Pending state, following changes by '{email}'. Please use the following link for details. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Accession Management</a>.");
        }
        public static Task SendAccessionCompletedByPhlebotomyNotification(this IEmailSender emailSender, string id, string email, string url)
        {
            return emailSender.SendEmailAsync("coding@accureference.com", $"Accession Management Request: '{id}'", $"The accession request has been sent to other lab, following changes by '{email}'. Please use the following link for details. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Accession Management</a>.");
        }
        public static Task SendAccessionCompletedByCodingNotification(this IEmailSender emailSender, string id, string email, string url)
        {
            return emailSender.SendEmailAsync(email, $"Accession Management Request: '{id}'", $"The accession request is now completed. Please use the following link for details. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Accession Management</a>.");
        }
        public static Task SendAccessionGreenlabNotification(this IEmailSender emailSender, string id, string email, string url)
        {
            return emailSender.SendEmailAsync(email, $"Accession Management Request: '{id}'", $"The accession request has been sent to Greenlab. Please use the following link for details. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Accession Management</a>.");
        }
        public static Task SendAccessionDeniedNotification(this IEmailSender emailSender, string id, string email, string url)
        {
            return emailSender.SendEmailAsync(email, $"Accession Management Request: '{id}'", $"The accession request has been denied. Please use the following link for details. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Accession Management</a>.");
        }
        public static Task SendAccessionNotSentToOtherLabNotification(this IEmailSender emailSender, string id, string email, string url)
        {
            return emailSender.SendEmailAsync("coding@accureference.com", $"Accession Management Request: '{id}'", $"The accession request is not sent to other lab, following changes by '{email}'. Please use the following link for details. <br /><a href='{HtmlEncoder.Default.Encode(url)}'>Accession Management</a>.");
        }

        public static Task SendItemNotification(this IEmailSender emailSender, string email, string OrderNote)
        {
            if (OrderNote != null && OrderNote != "")
            {
                return emailSender.SendEmailAsync(email, "Please prepare the below order", "Please prepare the below order and contact supplies@accureference.com for fufillment of Order Number : " + OrderNote);
            }
            else
            {
                return emailSender.SendEmailAsync(email, "Please prepare the below order", "Please prepare the below order and contact supplies@accureference.com for fufillment of Order Number : ");
            }
        }
        public static Task SendIOrderTotalNotification(this IEmailSender emailSender, string email, string OrderNote)
        {

            return emailSender.SendEmailAsync(email, "Order Total > $500", OrderNote + " Email notification for order total > $500.");

        }
        public static Task SendItemNotificationSupplyOrderItems(this IEmailSender emailSender, string Subject, string email, string SupplyEmail, string OrderNote)
        {
            //sending email to supplies with different body
            //sending only Order number for now as per sumesh's request
            if(!string.IsNullOrEmpty(SupplyEmail))
            {
                emailSender.SendEmailAsync(SupplyEmail, Subject, "Please prepare the Sales Order # " + (!string.IsNullOrEmpty(OrderNote) ? OrderNote : "") + "  created by " + email + "");
            }
            return emailSender.SendEmailAsync(email, Subject, "Supplies team supplies@accureference.com has been notified with the Sales Order # " + (!string.IsNullOrEmpty(OrderNote) ? OrderNote : ""));
        }

        public static Task SendItemNotificationLabSupplyOrderItems(this IEmailSender emailSender, string Subject, string email, string SupplyEmail, string OrderNote)
        {
            //sending email to supplies with different body
            //sending only Order number for now as per sumesh's request
            if (!string.IsNullOrEmpty(SupplyEmail))
            {
                emailSender.SendEmailAsync(SupplyEmail, Subject, "Please prepare the Sales Order # " + (!string.IsNullOrEmpty(OrderNote) ? OrderNote : "") + "  created by " + email + "");
            }
            return emailSender.SendEmailAsync(email, Subject, "Supplies team supplies@accureference.com has been notified with the Sales Order # " + (!string.IsNullOrEmpty(OrderNote) ? OrderNote : ""));
        }


        public static Task SendAccountRequestRejectedNotification(this IEmailSender emailSender, string email, string practice)
        {
            return emailSender.SendEmailAsync(INTERFACEEMAIL, "Account Request Denied",
                $"" + email + " has requested: " + practice + ", which already exists or has been requested elsewhere. Please contact the rep regarding this already existing account. For more details please login to the application and check the completed account requests page.");
        }

        //Warehouse
        public static Task SendPurchaseOrderRequest(this IEmailSender emailSender, string filecontents, string email)
        {
            List<string> cc = new List<string>
            {
                "purchasing@accureference.com"
            };
            if (email == "")
            {
                email = SumeshEMAIL;
            }
            return emailSender.SendEmailAsync(email, cc, "Purchase Order Request",
                $"Purchase Order Request attachment. If you have questions about this purchase order, please contact the purchasing team at Purchasing@accureference.com . Please send order confirmation to Purchasing@accureference.com", filecontents, "PurchaseOrder.pdf");
        }

        public static Task SendPickTicketCopy(this IEmailSender emailSender, string filecontents)
        {
            return emailSender.SendEmailAsync("supplies@accureference.com", "Printed Pick Tickets",
                $"Pick Ticket attachment.", filecontents, "PickTicket.pdf");
        }

        public static Task SendShipTicketCopy(this IEmailSender emailSender, string filecontents)
        {
            return emailSender.SendEmailAsync("supplies@accureference.com", "Printed Ship Ticket",
                $"Ship Ticket attachment.", filecontents, "ShipTicket.pdf");
        }

        //Support
        public static Task TicketCreatedNotification(this IEmailSender emailSender, string email, string ticketType, string ticketDescription, int ticketID, int accountID, string accountName)
        {
            return emailSender.SendEmailAsync(email, ticketType + " Ticket Created", "Ticket ID: " + ticketID.ToString() + " has been created for Account: " + accountID.ToString() + " - " + accountName + " with the following description:<br/>" + ticketDescription +
                "" +
                "<br/>Please login to <a src=\"https:\\accuapp.accureference.com\">AccuApp</a> to view more details about this ticket and to respond to the request.");
        }

        public static Task TicketModifiedNotification(this IEmailSender emailSender, string email, string ticketAction, int ticketID, int accountID, string accountName)
        {
            return emailSender.SendEmailAsync(email, "Ticket ID: " + ticketID.ToString() + " AccountID: " + accountID.ToString() + " - " + accountName + " Modified", "Ticket ID: " + ticketID.ToString() + " has been modified with the following action:\n" + ticketAction +
                "<br /> Please login to <a src=\"https:\\\\accuapp.accureference.com\">AccuApp</a> to view more details about this ticket and to respond to the request.");
        }

        public static Task TicketCompletedNotification(this IEmailSender emailSender, string email, string ticketAction, int ticketID)
        {
            return emailSender.SendEmailAsync(email, "Ticket ID: " + ticketID.ToString() + " Completed", "Ticket ID: " + ticketID.ToString() + " has been completed with the following message:\n" + ticketAction +
                "\nPlease login to <a src=\"https:\\\\accuapp.accureference.com\">AccuApp</a> to view more details about this ticket.");
        }

        public static Task TicketDeletedNotification(this IEmailSender emailSender, string email, int ticketID)
        {
            return emailSender.SendEmailAsync(email, "Ticket ID: " + ticketID.ToString() + " Deleted", "Ticket ID: " + ticketID.ToString() + " has been deleted. Feel free to ignore previous emails on this ticket.");
        }
        public static Task PhlebEditNotification(this IEmailSender emailSender, string email, string employeeID, string username)
        {
            return emailSender.SendEmailAsync(email, "The phlebotomist EmployeeID=" + employeeID + " edited.", " The phlebotomist EmployeeID=" + employeeID + " is edited  by " + username);
        }
        public static Task PhlebCreateNotification(this IEmailSender emailSender,string e, string employeeID, string username)
        {
            return emailSender.SendEmailAsync(e, "The phlebotomist EmployeeID=" + employeeID + " created", " The phlebotomist EmployeeID=" + employeeID + " is created  by " + username);
        }
    }
}
