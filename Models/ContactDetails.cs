using System;
namespace LoanLeads.models
{
    public class ContactDetails
    {
        public int LeadId { get; set; }
        public string ContactType { get; set; }
        public string ContactName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
    }
}