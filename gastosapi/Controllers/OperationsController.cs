using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gastosapi.Models;
using Microsoft.AspNetCore.Authorization;

namespace gastosapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly GastosappContext _context;

        public OperationController(GastosappContext context)
        {
            _context = context;
        }

        // GET: api/Operation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperation()
        {
          if (_context.Operation == null)
          {
              return NotFound();
          }
            return await _context.Operation.Include(o => o.IdCategoryNavigation).ToListAsync();
        }

        // GET: api/Operation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
          if (_context.Operation == null)
          {
              return NotFound();
          }
            var operation = await _context.Operation.FindAsync(id);

            if (operation == null)
            {
                return NotFound();
            }

            return operation;
        }

        // GET: api/Operation/
        [HttpGet("user/{userId}"), Authorize]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperationByUser(int userId)
        {
            var operationsUser = _context.Operation.Where(o => o.IdUser == userId);

            if (await _context.Users.FindAsync(userId) == null)
            {
                return NotFound("No existe este usuario");
            }

            if (operationsUser.Count() == 0)
            {
                return NotFound();
            }
            return await operationsUser.ToListAsync();
        }

        // PUT: api/Operation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperation(int id, Operation operation)
        {
            if (id != operation.IdOperation)
            {
                return BadRequest();
            }

            _context.Entry(operation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationExists(id))
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

        // POST: api/Operation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Operation>> PostOperation(Operation operation)
        {
          if (_context.Operation == null)
          {
              return Problem("Entity set 'GastosappContext.Operation'  is null.");
          }
            _context.Operation.Add(operation);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOperation", new { id = operation.IdOperation }, operation);
        }

        // DELETE: api/Operation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperation(int id)
        {
            if (_context.Operation == null)
            {
                return NotFound();
            }
            var operation = await _context.Operation.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            _context.Operation.Remove(operation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperationExists(int id)
        {
            return (_context.Operation?.Any(e => e.IdOperation == id)).GetValueOrDefault();
        }
    }
}
