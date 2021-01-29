using LoanLeads.models;

namespace LoanLeads.interfaces
{
    public interface ILoanLeadService
    {
        string GetLead(int id);
        bool SaveLeadInformation(LeadInformation lead);
        bool UpdateLeadInformation(LeadInformation lead);
        bool DeleteLeadInformation(int id);
    }
}