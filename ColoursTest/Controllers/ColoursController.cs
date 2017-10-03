using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ColoursTest.Web.Controllers
{
    [Route("api/[controller]")]
    public class ColoursController : Controller
    {
        public ColoursController(IColourRepository colourRepository)
        {
            this.ColourRepository = colourRepository;
        }

        private IColourRepository ColourRepository { get; }

        [HttpGet]
        public IActionResult Get()
        {
            var colours = this.ColourRepository.GetAll();
            var coloursResult = colours.ToColoursDto();
            return this.Ok(coloursResult);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var colour = this.ColourRepository.GetById(id);
            if (colour == null)
            {
                return this.NotFound();
            }
            var colourResult = colour.ToColourDto();
            return this.Ok(colourResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return this.NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return this.NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.NotFound();
        }
    }
}
