using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;      
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeRepository repository            
            , IMapper mapper)
        {
            _repository = repository;         
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var companies = await _repository.GetAllEmployees();

                var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(companies);
               

                return Ok(employeesDto);
            }
            catch (Exception ex)
            {               
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "EmployeeById")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var company = await _repository.GetEmployee(id);
            if (company == null)
            {             
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<EmployeeDto>(company);
                return Ok(companyDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
            {               
                return BadRequest("EmployeeForCreationDto object is null");
            }

            var employeeEntity = _mapper.Map<Employee>(employee);

            await _repository.CreateEmployee(employeeEntity);

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return CreatedAtRoute("EmployeeById", new { id = employeeToReturn.Id }, employeeToReturn);
        }
    }
}
