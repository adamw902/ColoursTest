using System;
using System.Threading.Tasks;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ColoursTest.Web.Controllers
{
    [Route("api/people")]
    public class PeopleController : Controller
    {
        public PeopleController(
            IPersonRepository personRepository, 
            IPersonService personService)
        {
            this.PersonRepository = personRepository;
            this.PersonService = personService;
        }
        
        private IPersonRepository PersonRepository { get; }

        private IPersonService PersonService { get; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var people = await this.PersonService.GetAllPeople();
            var peopleResult = people.ToPersonDto();

            return this.Ok(peopleResult);
        }

        [HttpGet("{id}", Name="PersonGet")]
        public async Task<IActionResult> Get(Guid id)
        {
            var person = await this.PersonService.GetPerson(id);

            if (person == null)
            {
                return this.NotFound();
            }

            var personResult = person.ToPersonDto();

            return this.Ok(personResult);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUpdatePerson createPerson)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var person = await this.PersonService.CreatePerson(createPerson);
            var personResult = person.ToPersonDto();

            return this.Ok(personResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]CreateUpdatePerson updatePerson)
        {
            var person = await this.PersonService.UpdatePerson(id, updatePerson);

            if (person == null)
            {
                return this.NotFound();
            }

            var personResult = person.ToPersonDto();

            return this.Ok(personResult);
        }
    }
}