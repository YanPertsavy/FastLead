using FastLead.Interfaces;
using FastLead.Models;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using FastLead.DTO;

namespace FastLead.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public HomeController(IAccountRepository accountRepository) 
        { 
            _accountRepository = accountRepository;
        }
        public IActionResult Accounts()
        {
            return View();
        }

        [HttpPost("/[controller]/getAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            List<AccountDto> accounts = await _accountRepository.GetAllDtoAsync();
            return Ok(accounts);
        }

        [HttpGet("/[controller]/details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            Account account = await _accountRepository.GetByIdAsync(id);
            return Ok(account);
        }

        [HttpPost("/[controller]/updateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] Account data)
        {
            await _accountRepository.UpdateAsync(data);
            return Ok();
        }

        [HttpDelete("/[controller]/delete/{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            await _accountRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete("/[controller]/bulkDelete")]
        public async Task<IActionResult> BulkDelete([FromBody] List<Guid> ids)
        {
            if(ids == null || ids.Count == 0) return BadRequest();
            await _accountRepository.BulkDelete(ids);
            return Ok();
        }

        [HttpPost("/[controller]/getExel")]
        public async Task<IActionResult> GetExel([FromBody] List<Guid> ids)
        {
            List<Account> accs = await _accountRepository.GetRangeAsync(ids);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Контрагенты");

                worksheet.Cell(1, 1).InsertTable(accs);
                worksheet.Columns().AdjustToContents(); // Автоширина колонок

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Возвращаем файл: массив байтов, тип контента и имя файла
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Accounts_Export.xlsx"
                    );
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFilters(string field, string value)
        {
            List<AccountDto> res = await _accountRepository.GetFiltersAsync(field, value);
            return Ok(res);
        }
    }
}
