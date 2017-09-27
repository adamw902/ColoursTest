using ColoursTest.Data.Interfaces;
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
            return Ok(colours);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int colourId)
        {
            var colour = ColourRepository.GetById(colourId);
            return colour != null ? (IActionResult) Ok(colour) : NotFound();
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
