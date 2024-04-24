using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST.Domain;
using REST.Service;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        List<Person> people = new List<Person>()
        {
            new Person { Id = 1, Name = "John", Lastname = "Doe", Age = 30, City = "New York" },
            new Person { Id = 2, Name = "Jane", Lastname = "Smith", Age = 30, City = "Los Angeles" },
            new Person { Id = 3, Name = "Alice", Lastname = "Johnson", Age = 35, City = "Chicago" },
            new Person { Id = 4, Name = "Michael", Lastname = "Brown", Age = 15, City = "Los Angeles" },
            new Person { Id = 5, Name = "Emily", Lastname = "Williams", Age = 28, City = "New York" },
            new Person { Id = 6, Name = "Daniel", Lastname = "Jones", Age = 15, City = "Chicago" },
            new Person { Id = 7, Name = "Shein", Lastname = "Morgan", Age = 20, City = "Chicago" }
        };

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        // 4.Query Processing
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _personService.GetPeople(people);
            return Ok(response);
        }

        // 1.Filter
        [HttpGet("adults")]
        public async Task<IActionResult> GetAdults()
        {
            var response = await _personService.GetAdults(people);
            return Ok(response);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginated([FromQuery] int page, [FromQuery] int size)
        {
            var response = await _personService.PeoplePagination(people, page, size);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById([FromRoute] int id)
        {
            var response = await _personService.GetPersonById(people, id);
            if (response == null) return BadRequest("User with this id was not found");

            return Ok(response);

        }

        // 2.Sort
        [HttpGet("sorted-by-age")]
        public async Task<IActionResult> GetSortedByAge()
        {
            var response = await _personService.SortByAge(people);
            return Ok(response);
        }

        [HttpGet("sorted-by-age-and-name")]
        public async Task<IActionResult> GetSortedByAgeAndName()
        {
            var response = await _personService.SortByAgeAndName(people);
            return Ok(response);
        }        

        [HttpGet("grouped-by-city")]
        public async Task<IActionResult> GetGrouped()
        {
            var response = await _personService.GroupByCity(people);
            return Ok(response);
        }

        // 3.Aggregation
        [HttpGet("avg-age")]
        public async Task<IActionResult> GetAvg()
        {
            var response = await _personService.AvgAge(people);
            return Ok(response);
        }

        [HttpGet("min-and-max-age")]
        public async Task<IActionResult> GetMinMax()
        {
            var response = await _personService.MinMaxAge(people);
            return Ok(response);
        }
    }
}
