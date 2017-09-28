using System.Collections.Generic;
using AutoMapper;
using ColoursTest.Data.DTOs;
using ColoursTest.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ColoursTest.Web.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private IPersonRepository PersonRepository { get; }
        
        public PeopleController(IPersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var people = PersonRepository.GetAll();
            var peopleResult = Mapper.Map<IEnumerable<PersonDto>>(people);
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
            var personResult = Mapper.Map<PersonDto>(person);
            return Ok(personResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody]PersonDto person)
        {
            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdatePersonDto updatePersonDto)
        {
            var person = PersonRepository.Update(id, updatePersonDto);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NotFound();
        }
    }
}
