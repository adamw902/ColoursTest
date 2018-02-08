using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
namespace ColoursTest.Web.Controllers
{
    [Route("api/colours")]
    public class ColoursController : Controller
    {
        public ColoursController(
            IColourRepository colourRepository, 
            IColourService colourService)
        {
            this.ColourRepository = colourRepository;
            this.ColourService = colourService;
        }
        
        private IColourRepository ColourRepository { get; }

        private IColourService ColourService { get; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var colours = await this.ColourRepository.GetAll();
            var colourResult = colours.ToColourDto();

            return this.Ok(colourResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var colour = await this.ColourRepository.GetById(id);

            if (colour == null)
            {
                return this.NotFound();
            }

            var colourResult = colour.ToColourDto();

            return this.Ok(colourResult);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUpdateColour createColourRequest)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var colour = await this.ColourService.CreateColour(createColourRequest);
            var colourResult = colour.ToColourDto();

            return this.Ok(colourResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]CreateUpdateColour updateColourRequest)
        {
            var colour = await this.ColourService.UpdateColour(id, updateColourRequest);

            if (colour == null)
            {
                return this.NotFound();
            }

            var colourResult = colour.ToColourDto();

            return this.Ok(colourResult);
        }
    }
}
