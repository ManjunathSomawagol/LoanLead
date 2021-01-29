using System.Collections.Generic;
namespace LoanLeads.models
{
    public class LeadInformation
    {
        public int LeadId { get; set; }
        public long LoanAmount { get; set; }
        public string LeadSource { get; set; }
        public string CommunicationMode { get; set; }
        public string Status { get; set; }
        public List<ContactDetails> ContactDetails { get; set; }
    }
}