using System;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ColoursTest.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ColoursController : Controller
    {
        public ColoursController(ILogger<ColoursController> logger, IColourRepository colourRepository, IColourService colourService)
        {
            this.Logger = logger;
            this.ColourRepository = colourRepository;
            this.ColourService = colourService;
        }

        private ILogger<ColoursController> Logger { get; }
        private IColourRepository ColourRepository { get; }
        private IColourService ColourService { get; }

        [HttpGet]
        public IActionResult Get()
        {
            this.Logger.LogInformation("Get colours called");
            var colours = this.ColourRepository.GetAll();
            return this.Ok(colours);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            this.Logger.LogInformation("Get colour called");
            try
            {
                var colour = this.ColourRepository.GetById(id);
                return this.Ok(colour);
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateUpdateColour createColourRequest)
        {
            this.Logger.LogInformation("Create colour called");
            try
            {
                var colour = this.ColourService.CreateColour(createColourRequest);
                return this.Ok(colour);
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CreateUpdateColour updateColourRequest)
        {
            this.Logger.LogInformation("Update colour called");
            try
            {
                var colour = this.ColourService.UpdateColour(id, updateColourRequest);
                return this.Ok(colour);
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation(ex.Message);
                return this.NotFound(ex.Message);
            }
        }
    }
}
