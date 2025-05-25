using CarManagement.Data;
using CarManagement.DTO;
using CarManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetCarsByFactory/{carFactoryId}")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCarsByFactory(int carFactoryId)
        {
            try
            {
                if (!await _context.CarFactories.AnyAsync(f => f.id == carFactoryId))
                    return NotFound();

                return await _context.Cars
                    .Where(c => c.carfactoryid == carFactoryId)
                    .AsNoTracking()
                    .Select(c => new CarDTO{ Id = c.id, Name = c.name, Type = c.type })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddCar")]
        public async Task<ActionResult<CarDTO>> AddCar(CarCreateDTO createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.CarFactories.AnyAsync(f => f.id == createDto.CarFactoryId))
                return BadRequest("Factory not found");

            bool exists = await _context.Cars
            .AnyAsync(c => c.name.ToLower() == createDto.Name.ToLower()
                    && c.type.ToLower() == createDto.Type.ToLower()
                    && c.carfactoryid == createDto.CarFactoryId);

            if (exists)
            {
                return Conflict(new
                {
                    Message = "A car with such characteristics already exists from this manufacturer."
                });
            }

            var car = new Car
            {
                name = createDto.Name,
                type = createDto.Type,
                carfactoryid = createDto.CarFactoryId
            };

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Car added successfully" });
        }

        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var carModel = await _context.Cars.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(carModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
