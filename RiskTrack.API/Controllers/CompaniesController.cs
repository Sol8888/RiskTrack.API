using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskTrack.Infrastructure.Data; 
using RiskTrack.API.Domain.Entities; 


namespace RiskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly RiskTrackDbContext _context;

        public CompaniesController(RiskTrackDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(companies);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound($"Company with ID {id} not found.");

            return Ok(company);
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                RUC = dto.RUC,
                Sector = dto.Sector
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Company created", CompanyId = company.CompanyId });
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyDto dto)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound($"Company with ID {id} not found.");

            company.Name = dto.Name;
            company.RUC = dto.RUC;
            company.Sector = dto.Sector;

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Company updated", CompanyId = company.CompanyId });
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound($"Company with ID {id} not found.");

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Company deleted", CompanyId = id });
        }
    }

    public class CompanyDto
    {
        public string? Name { get; set; }
        public string? RUC { get; set; }
        public string Sector { get; set; } = default!;
    }
}

