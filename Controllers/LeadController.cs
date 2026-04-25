using ClosedXML.Excel;
using FastLead.DTO;
using FastLead.Interfaces;
using FastLead.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastLead.Controllers
{
    [Authorize]
    public class LeadController : Controller
    {
        private readonly ILeadRepository _leadRepository;

        public LeadController(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        public IActionResult Leads()
        {
            return View();
        }

        [HttpPost("/[controller]/getLeads")]
        public async Task<IActionResult> GetLeads()
        {
            List<LeadDto> leads = await _leadRepository.GetAllDtoAsync();
            return Ok(leads);
        }

        [HttpGet("/[controller]/details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            Lead lead = await _leadRepository.GetByIdAsync(id);
            return Ok(lead);
        }

        [HttpPost("/[controller]/updateLead")]
        public async Task<IActionResult> UpdateLead([FromBody] Lead data)
        {
            await _leadRepository.UpdateAsync(data);
            return Ok();
        }

        [HttpDelete("/[controller]/delete/{id}")]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            await _leadRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete("/[controller]/bulkDelete")]
        public async Task<IActionResult> BulkDelete([FromBody] List<Guid> ids)
        {
            if (ids == null || ids.Count == 0) return BadRequest();
            await _leadRepository.BulkDelete(ids);
            return Ok();
        }

        [HttpPost("/[controller]/getExcel")]
        public async Task<IActionResult> GetExcel([FromBody] List<Guid> ids)
        {
            List<Lead> leads = await _leadRepository.GetRangeAsync(ids);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Лиды");
                worksheet.Cell(1, 1).InsertTable(leads);
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Leads_Export.xlsx"
                    );
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFilters(string field, string value)
        {
            List<LeadDto> res = await _leadRepository.GetFiltersAsync(field, value);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddLead([FromBody] Lead lead)
        {
            await _leadRepository.CreateAsync(lead);
            return Ok();
        }
    }
}
