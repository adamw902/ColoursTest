using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ColoursTest.Web.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private IPersonRepository PersonRepository { get; }
        private IPersonService PersonService { get; }
        
        public PeopleController(IPersonRepository personRepository, IPersonService personService)
        {
            PersonRepository = personRepository;
            PersonService = personService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var people = PersonRepository.GetAll();
            var peopleResult = people.ToPersonDto();
            return Ok(peopleResult);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var person = PersonRepository.GetById(id);

            if (person == null)
            {
                return NotFound();
            }
            var personResult = person.ToPersonDto();
            return Ok(personResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody]PersonDto person)
        {
            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdatePerson updatePerson)
        {
            var person = this.PersonService.UpdatePerson(id, updatePerson);
            if (person == null)
            {
                return NotFound();
            }
            var personResult = person.ToPersonDto();
            return Ok(personResult);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NotFound();
        }
    }
}
