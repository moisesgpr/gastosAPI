using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gastosapi.Models;

namespace gastosapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly GastosappContext _context;

        public OperationsController(GastosappContext context)
        {
            _context = context;
        }

        // GET: api/Operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperations()
        {
          if (_context.Operations == null)
          {
              return NotFound();
          }
            return await _context.Operations.ToListAsync();
        }

        // GET: api/Operations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
          if (_context.Operations == null)
          {
              return NotFound();
          }
            var operation = await _context.Operations.FindAsync(id);

            if (operation == null)
            {
                return NotFound();
            }

            return operation;
        }

        // GET: api/Operations/
        [HttpGet("/user/{userId}")]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperationsByUser(int userId)
        {
            var operationsUser = _context.Operations.Where(o => o.IdUser == userId);

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

        // PUT: api/Operations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperation(int id, Operation operation)
        {
            if (id != operation.IdOperations)
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

        // POST: api/Operations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Operation>> PostOperation(Operation operation)
        {
          if (_context.Operations == null)
          {
              return Problem("Entity set 'GastosappContext.Operations'  is null.");
          }
            _context.Operations.Add(operation);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOperation", new { id = operation.IdOperations }, operation);
        }

        // DELETE: api/Operations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperation(int id)
        {
            if (_context.Operations == null)
            {
                return NotFound();
            }
            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperationExists(int id)
        {
            return (_context.Operations?.Any(e => e.IdOperations == id)).GetValueOrDefault();
        }
    }
}
