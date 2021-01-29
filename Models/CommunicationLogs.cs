using System;
namespace LoanLeads.models
{
    public class CommunicationLog
    {
        public int LeadId { get; set; }
        public string CommunicationMode { get; set; }
        public DateTime CommunicationDate { get; set; }
        public string Status { get; set; }
    }
}