using HotelWebApi.Dtos.Company;
using HotelWebApi.Dtos.Currency;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public CompanyController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        {

            var company = await _context.Companies.ToListAsync();

            return Ok(company);
        }
        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(CompanyCreateDto companyDto)
        {

            var company = mapper.Map<Company>(companyDto);
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return Ok(company.Id);
        }
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Company>>> PutCompany(CompanyPutDto companyPut)
        {
            //var tax = _context.TaxTables.Where(x => x.Id == taxPut.Id).ToList();
            var company = mapper.Map<Company>(companyPut);

            //var mapTaxes = mapper.Map
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            return Ok(company.Id);
        }
        [HttpPut("companyDeposit")]
        public async Task<ActionResult<IEnumerable<Company>>> CompanyDeposit(CompanyPutDto companyData)
        {
            var existingCompanyData = await _context.Companies.Where(x => x.Id == companyData.Id).FirstOrDefaultAsync();
            
            existingCompanyData.Credit = existingCompanyData.Credit+ companyData.Credit;
           

            //var mapTaxes = mapper.Map
            _context.Companies.Update(existingCompanyData);
            await _context.SaveChangesAsync();

            return Ok(existingCompanyData.Id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
