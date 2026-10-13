using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorDirectoryApi.Data;
using DoctorDirectoryApi.DTOs;
using DoctorDirectoryApi.Models;

namespace DoctorDirectoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/doctors OR api/doctors?specialization=Cardiology
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors([FromQuery] string? specialization)
        {
            var query = _context.Doctors.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(specialization))
            {
                query = query.Where(d => d.Specialization.ToLower() == specialization.Trim().ToLower());
            }

            return Ok(await query.ToListAsync());
        }

        // GET: api/doctors/1
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {id} not found." });
            }
            return Ok(doctor);
        }

        // POST: api/doctors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool emailExists = await _context.Doctors.AnyAsync(d => d.Email.ToLower() == dto.Email.Trim().ToLower());
            if (emailExists)
            {
                return Conflict(new { message = $"A doctor with the email '{dto.Email}' already exists." });
            }

            var doctor = new Doctor
            {
                FullName = dto.FullName.Trim(),
                Specialization = dto.Specialization.Trim(),
                Email = dto.Email.Trim(),
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        // PUT: api/doctors/1
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Doctor>> UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {id} not found." });
            }

            bool emailExists = await _context.Doctors
                .AnyAsync(d => d.Email.ToLower() == dto.Email.Trim().ToLower() && d.Id != id);

            if (emailExists)
            {
                return Conflict(new { message = $"A doctor with the email '{dto.Email}' already exists." });
            }

            doctor.FullName = dto.FullName.Trim();
            doctor.Specialization = dto.Specialization.Trim();
            doctor.Email = dto.Email.Trim();
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.ConsultationFee = dto.ConsultationFee;

            await _context.SaveChangesAsync();

            return Ok(doctor);
        }

        // DELETE: api/doctors/1
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {id} not found." });
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}