using System;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using ColoursTest.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ColoursTest.Web.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        public PeopleController(IPersonRepository personRepository, IPersonService personService, ILogger<PeopleController> logger, ILogger<CustomExceptionFilterAttribute> exceptionLogger)
        {
            this.PersonRepository = personRepository;
            this.PersonService = personService;
            this.Logger = logger;
            this.ExceptionLogger = exceptionLogger;
        }

        private ILogger<CustomExceptionFilterAttribute> ExceptionLogger { get; }
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
            this.Logger.LogInformation("Get person called");
            var person = this.PersonRepository.GetById(id);
            var personResult = person.ToPersonDto();
            return this.Ok(personResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateUpdatePerson createPerson)
        {
            this.Logger.LogInformation("Create person called");
            var person = this.PersonService.CreatePerson(createPerson);
            var personResult = person.ToPersonDto();
            return this.Ok(personResult);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CreateUpdatePerson updatePerson)
        {
            this.Logger.LogInformation("Update person called");
            var person = this.PersonService.UpdatePerson(id, updatePerson);
            var personResult = person.ToPersonDto();
            return this.Ok(personResult);
        }
    }
}
