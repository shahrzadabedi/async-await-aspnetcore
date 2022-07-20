using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
	[Route("api/companies")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly ICompanyRepository _repository;
		private readonly IMapper _mapper;

		public CompaniesController(ICompanyRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			try
			{
				var companies = await _repository.GetAllCompanies();

				var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

				return Ok(companiesDto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}", Name = "CompanyById")]
		public async Task<IActionResult> GetCompany(Guid id)
		{
			var company = await _repository.GetCompany(id);
			if (company == null)
			{
				return NotFound();
			}
			else
			{
				var companyDto = _mapper.Map<CompanyDto>(company);
				return Ok(companyDto);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
		{
			if (company == null)
			{				
				return BadRequest("CompanyForCreationDto object is null");
			}

			var companyEntity = _mapper.Map<Company>(company);

			await _repository.CreateCompany(companyEntity);

			var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

			return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
		}
	}
}