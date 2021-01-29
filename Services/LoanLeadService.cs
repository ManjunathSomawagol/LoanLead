using System.Collections.Generic;
using System;
using System.Data;
using Dapper;
using LoanLeads.interfaces;
using LoanLeads.models;

namespace LoanLeads.Service
{
    public class LoanLeadService : ILoanLeadService
    {
        private IDbConnection _connection;
        public LoanLeadService(IDbConnection connection)
        {
            _connection = connection;
        }

        public string GetLead(int id)
        {
            var param = new DynamicParameters();
            param.Add("@leadId",id, DbType.Int32);

            using(IDbConnection con = _connection)
            {
                con.Open();
                var lead = con.QuerySingleOrDefault<string>("dbo.[sp_getLead]", param, commandType: CommandType.StoredProcedure);
                con.Close();
                return lead;
            }
            
        }

        public bool SaveLeadInformation(LeadInformation lead)
        {
            try
            {
                string communicationcmd = @"insert into ContactDetails(LeadId,ContactType,ContactName,Dob,Gender,Email,ContactNumber) values(@LeadId,@ContactType,@ContactName,@Dob,@Gender,@Email,@ContactNumber)";
                List<ContactDetails> contacts = new List<ContactDetails>();
                var leadParam = new DynamicParameters();
                leadParam.Add("@LoanAmount",lead.LoanAmount);
                leadParam.Add("@LeadSource",lead.LeadSource);
                leadParam.Add("@CommunicationMode",lead.CommunicationMode);
                leadParam.Add("@Status",lead.Status);
                leadParam.Add("@leadId", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                using (IDbConnection con = _connection)
                {
                    con.Open();
                    var insertedLead = con.Execute("dbo.[sp_saveLeadInfo]",leadParam, commandType: CommandType.StoredProcedure);
                    int insertedLeadId = leadParam.Get<int>("@leadId");

                    foreach (var item in lead.ContactDetails)
                    {
                        contacts.Add(new ContactDetails{
                            LeadId = insertedLeadId,
                            ContactType = item.ContactType,
                            ContactName = item.ContactName,
                            Dob = item.Dob,
                            Gender = item.Gender,
                            Email = item.Email,
                            ContactNumber = item.ContactNumber
                        });
                    }
                    var inesertedContacts = con.Execute(communicationcmd,contacts);
                    
                    con.Close();
                    
                    if(insertedLead > 0 && inesertedContacts > 0){
                        return true;
                    }else{
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool ILoanLeadService.DeleteLeadInformation(int id)
        {
            try
            {
                string deleteLeadCmd = @"delete from LoanLead where leadId = @LeadId";
                string deleteContactCmd = @"delete from ContactDetails where leadId = @LeadId";

                var param = new DynamicParameters();
                param.Add("@LeadId",id);

                using (IDbConnection con = _connection)
                {
                    con.Open();
                    var deletedLeadCount = con.Execute(deleteLeadCmd,param);
                    var deleteContactCount = con.Execute(deleteContactCmd,param);
                    con.Close();

                    if(deletedLeadCount > 0 && deleteContactCount > 0){
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool ILoanLeadService.UpdateLeadInformation(LeadInformation lead)
        {
            try
            {
                string leadCmd = @"update LoanLead set LoanAmount = @LoanAmount,LeadSource = @LeadSource,CommunicationMode = @CommunicationMode,Status = @Status
                                where LeadId = @LeadId";
                string communicationcmd = @"update ContactDetails set ContactType = @ContactType,ContactName = @ContactName,Dob = @Dob,Gender = @Gender,Email = @Email,ContactNumber =@ContactNumber
                                        where LeadId = @LeadId";
                List<ContactDetails> contacts = new List<ContactDetails>();
                var leadParam = new DynamicParameters();
                leadParam.Add("@LoanAmount",lead.LoanAmount);
                leadParam.Add("@LeadSource",lead.LeadSource);
                leadParam.Add("@CommunicationMode",lead.CommunicationMode);
                leadParam.Add("@Status",lead.Status);
                leadParam.Add("@leadId", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                using (IDbConnection con = _connection)
                {
                    con.Open();
                    var updatedLead = con.Execute(leadCmd,leadParam);

                    foreach (var item in lead.ContactDetails)
                    {
                        contacts.Add(new ContactDetails{
                            LeadId = item.LeadId,
                            ContactType = item.ContactType,
                            ContactName = item.ContactName,
                            Dob = item.Dob,
                            Gender = item.Gender,
                            Email = item.Email,
                            ContactNumber = item.ContactNumber
                        });
                    }
                    var updatedContacts = con.Execute(communicationcmd,contacts);
                    
                    con.Close();
                    
                    if(updatedLead > 0 || updatedContacts > 0){
                        return true;
                    }else{
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}