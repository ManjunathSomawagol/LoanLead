using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoanLeads.models;
using LoanLeads.interfaces;
using Microsoft.AspNetCore.Http;

namespace loanleadserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanLeadController : ControllerBase
    {
        private readonly ILogger<LoanLeadController> _logger;
        private ILoanLeadService _loanService;

        public LoanLeadController(ILogger<LoanLeadController> logger,ILoanLeadService loanService)
        {
            _logger = logger;
            _loanService = loanService;
        }

        [HttpGet]
        [Route("/getlead/{id}")]
        public IActionResult GetLeadById(int id)
        {
            if(id <= 0){
                return BadRequest("Invalid Loan Lead Id");
            }
            return Ok(_loanService.GetLead(id));
        }

        [HttpPost]
        [Route("/savelead")]
        public IActionResult SaveLeadInfo([FromBody] LeadInformation lead)
        {
            try
            {
                foreach (var item in lead.ContactDetails)
                {
                    if(string.IsNullOrEmpty(item.ContactNumber) || item == null){
                        return BadRequest("Contact number cannot be empty");
                    }
                }
                var res = _loanService.SaveLeadInformation(lead);
                if(res){
                    return Ok("Lead created");
                }else{
                    return Conflict("Same record already found");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("/deletelead/{id}")]
        public IActionResult DeleteLeadInfo(int id)
        {
            try
            {
                if(id <= 0){
                    return BadRequest("Lead id cannot be less than zero");
                }
                var res = _loanService.DeleteLeadInformation(id);

                if(res){
                    return Ok("Lead info deleted");
                }else{
                    return NotFound($"Lead information not found for id:{id}");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("/updatelead")]
        public IActionResult UpdateLeadInfo([FromBody] LeadInformation lead)
        {
            try
            {
                if(lead.LeadId <= 0){
                    return BadRequest("Lead id cannot be less than zero");
                }
                var res = _loanService.UpdateLeadInformation(lead);

                if(res){
                    return Ok("Lead info updated");
                }else{
                    return NotFound($"Lead information not found for id:{lead.LeadId}");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
