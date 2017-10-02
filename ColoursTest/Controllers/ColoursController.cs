using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ColoursTest.Web.Controllers
{
    [Route("api/[controller]")]
    public class ColoursController : Controller
    {
        private IColourRepository ColourRepository { get; }

        public ColoursController(IColourRepository colourRepository)
        {
            ColourRepository = colourRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var colours = ColourRepository.GetAll();
            var coloursResult = colours.ToColoursDto();
            return Ok(coloursResult);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var colour = ColourRepository.GetById(id);
            if (colour == null)
            {
                return NotFound();
            }
            var colourResult = colour.ToColourDto();
            return Ok(colourResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NotFound();
        }
    }
}
