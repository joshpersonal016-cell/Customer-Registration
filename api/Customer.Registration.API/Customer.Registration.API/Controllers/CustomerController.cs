using Customer.Registration.Application.Dtos;
using Customer.Registration.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Customer.Registration.API.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        private readonly ICustomerService _service = service;

        // GET: api/customer
        [HttpGet(Name = "GetCustomerList")]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllCustomerAsync();
            return Ok(result);
        }

        // GET: api/customer/{id}
        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid Id");

            var result = await _service.GetCustomerByIdAsync(id);

            if (result == null)
                return NotFound("Customer not found");

            return Ok(result);
        }

        // POST: api/customer
        [HttpPost(Name = "CreateCustomer")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateCustomerAsync(dto);

            return Ok("Customer created successfully");
        }
    }
}
