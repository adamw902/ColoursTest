using System;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ColoursTest.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        public PeopleController(IPersonRepository personRepository, IPersonService personService, ILogger<PeopleController> logger)
        {
            this.PersonRepository = personRepository;
            this.PersonService = personService;
            this.Logger = logger;
        }

        private ILogger<PeopleController> Logger { get; }
        private IPersonRepository PersonRepository { get; }
        private IPersonService PersonService { get; }

        [HttpGet]
        public IActionResult Get()
        {
            this.Logger.LogInformation("Get people called");
            var people = this.PersonRepository.GetAll();
            var peopleResult = people.ToPersonDto();
            return this.Ok(peopleResult);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            this.Logger.LogError("Get person called");
            try
            {
                var person = this.PersonRepository.GetById(id);
                var personResult = person.ToPersonDto();
                return this.Ok(personResult);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateUpdatePerson createPerson)
        {
            this.Logger.LogInformation("Create person called");
            try
            {
                var person = this.PersonService.CreatePerson(createPerson);
                var personResult = person.ToPersonDto();
                return this.Ok(personResult);
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CreateUpdatePerson updatePerson)
        {
            this.Logger.LogInformation("Update person called");
            try
            {
                var person = this.PersonService.UpdatePerson(id, updatePerson);
                var personResult = person.ToPersonDto();
                return this.Ok(personResult);
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation(ex.Message);
                return this.NotFound(ex.Message);
            }
        }
    }
}
