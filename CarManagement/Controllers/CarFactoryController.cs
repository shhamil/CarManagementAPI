using CarManagement.Data;
using CarManagement.DTO;
using CarManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarFactoryController : Controller
    {
        private readonly AppDbContext _context;

        public CarFactoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetCarFactories")]
        public async Task<ActionResult<IEnumerable<CarFactoryDTO>>> GetCarFactories()
        {
            try
            {
                return await _context.CarFactories
                    .AsNoTracking()
                    .Select(f => new CarFactoryDTO { Id = f.id, Name = f.name, Country = f.country })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddCarFactory")]
        public async Task<ActionResult<CarFactoryDTO>> AddCarFactory(CarFactoryCreateDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool exists = await _context.CarFactories
                .AnyAsync(f => f.name.ToLower() == createDto.Name.ToLower()
                             && f.country.ToLower() == createDto.Country.ToLower());

            if (exists)
            {
                return Conflict(new
                {
                    Message = "A manufacturer with the same name and country already exists"
                });
            }

            var factory = new CarFactory
            {
                name = createDto.Name,
                country = createDto.Country
            };

            await _context.CarFactories.AddAsync(factory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCarFactories),
                new CarFactoryDTO
                {
                    Id = factory.id,
                    Name = factory.name,
                    Country = factory.country
                });
        }

        [HttpDelete("DeleteCarFactory/{id}")]
        public async Task<IActionResult> DeleteCarFactory(int id)
        {
            var carFactory = await _context.CarFactories.FindAsync(id);
            if (carFactory == null)
            {
                return NotFound();
            }

            _context.CarFactories.Remove(carFactory);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
