using FastLead.DTO;
using FastLead.Interfaces;
using FastLead.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastLead.Controllers
{
    [Authorize]
    public class DebtController : Controller
    {
        private readonly IDebtRepository _debtRepository;
        private readonly IAccountRepository _accountRepository;

        public DebtController(IDebtRepository debtRepository, IAccountRepository accountRepository)
        {
            _debtRepository = debtRepository;
            _accountRepository = accountRepository;
        }

        public IActionResult Debts() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var debts = await _debtRepository.GetAllAsync();
            var result = debts.Select(d => new
            {
                d.Id,
                d.Amount,
                d.IsOverdue,
                d.ContractNumber,
                ServiceType = d.ServiceType.ToString(),
                AccountName = d.Account?.Name
            });
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var debt = await _debtRepository.GetByIdAsync(id);
            if (debt == null) return NotFound();
            return Ok(debt);
        }

        [HttpPost("[controller]/Add")]
        public async Task<IActionResult> Add([FromBody] Debt debt)
        {
            await _debtRepository.AddAsync(debt);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Debt debt)
        {
            await _debtRepository.UpdateAsync(debt);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _debtRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRange([FromBody] List<Guid> ids)
        {
            await _debtRepository.DeleteRangeAsync(ids);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetFilters(string field, string value)
        {
            var res = await _debtRepository.GetFiltersAsync(field, value);
            var result = res.Select(d => new
            {
                d.Id,
                d.Amount,
                d.IsOverdue,
                d.ContractNumber,
                ServiceType = d.ServiceType.ToString(),
                AccountName = d.Account?.Name
            });
            return Ok(result);
        }
    }
}
