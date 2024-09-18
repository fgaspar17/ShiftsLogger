using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger;

namespace ShiftsLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftContext _context;

        public ShiftsController(ShiftContext context)
        {
            _context = context;
        }

        // TODO: Usar DTOs

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShifts()
        {
            return await _context.Shifts
                .Include(s => s.Employee) // Include the related Employee entity
                .Select(s => ShiftMapper.MapToDto(s))
                .ToListAsync();
        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftDto>> GetShift(int id)
        {
            var shift = await _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.ShiftId == id)
                .FirstOrDefaultAsync();

            if (shift == null)
            {
                return NotFound();
            }

            return ShiftMapper.MapToDto(shift);
        }

        // PUT: api/Shifts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(int id, Shift shift)
        {
            if (id != shift.ShiftId)
            {
                return BadRequest();
            }

            var employee = await _context.Employees.FindAsync(shift.EmployeeId);
            if (employee == null)
            {
                return NotFound($"Employee with ID {shift.EmployeeId} not found.");
            }
            shift.Employee = await _context.Employees.Where(e => e.EmployeeId == shift.EmployeeId).FirstOrDefaultAsync();

            _context.Employees.Attach(shift.Employee);

            if (!EndHigherThanStart(shift))
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity,
                    new { Error = "The End time must be higher than Start time." });
            }

            _context.Entry(shift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shifts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(Shift shift)
        {
            var employee = await _context.Employees.FindAsync(shift.EmployeeId);
            if (employee == null)
            {
                return NotFound($"Employee with ID {shift.EmployeeId} not found.");
            }

            if (!EndHigherThanStart(shift))
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity,
                    new { Error = "The End time must be higher than Start time." });
            }

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShift), new { id = shift.ShiftId }, shift);
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.ShiftId == id);
        }

        private bool EndHigherThanStart(Shift shift)
        {
            return shift.End > shift.Start;
        }
    }
}
