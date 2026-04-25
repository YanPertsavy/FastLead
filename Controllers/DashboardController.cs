using FastLead.DTO;
using FastLead.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastLead.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILeadRepository _leadRepository;
        public DashboardController(IAccountRepository accountRepository, ILeadRepository leadRepository) 
        { 
            _accountRepository = accountRepository;
            _leadRepository = leadRepository;
        }
        public IActionResult Info()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetGraficsData()
        {
            DashboardDTO leadDTO = await _leadRepository.GetDashboardDTOAsync();
            DashboardDTO accDTO = await _accountRepository.GetDashboardDTOAsync();
            return Ok(new { leadData = leadDTO, accData = accDTO });
        }
    }
}
