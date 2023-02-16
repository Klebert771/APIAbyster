using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIAbyster.Models;
using APIAbyster.dto;

namespace APIAbyster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public OperationsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperations()
        {
            return await _context.Operations.ToListAsync();
        }

        // GET: api/Operations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(decimal id)
        {
            var operation = await _context.Operations.FindAsync(id);

            if (operation == null)
            {
                return NotFound();
            }

            return operation;
        }

     

        // PUT: api/Operations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperation(decimal id, Operation operation)
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

        // POST: api/Operations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Operation>> PostOperation(Operation operation)
        {
            _context.Operations.Add(operation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOperation", new { id = operation.IdOperation }, operation);
        }

        // DELETE: api/Operations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperation(decimal id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("AddOperation")]
        public async Task<ActionResult<Operation>> AddOperation(OperationDto operationDto)
        {
            try
            {

                Operation operation = new Operation();
                operation.IdCategorie = operationDto.IdCategorie;
                operation.IdUser = operationDto.IdUser;
                operation.LibelleOperation = operationDto.LibelleOperation;
                operation.MontantOperation = operationDto.MontantOperation;
                operation.DateOperation = DateTime.Now;
                operation.IdCategorie = operationDto.IdCategorie;

                _context.Operations.Add(operation);
                await _context.SaveChangesAsync();

                return new JsonResult(new { statusCode = Ok(), message = "opération ajouter avec succès" });
            }
            catch (Exception)
            {
                return new JsonResult(new { statusCode = BadRequest() });
            }
        }

        // GET: api/Operations/5
        [HttpGet]
        [Route("listOperationUser")]
        public async Task<ActionResult<IEnumerable<Operation>>> listOperationUser(decimal idUser)
        {

            return await _context.Operations.Where(p => p.IdUser == idUser).ToListAsync();
        }
        // Get : Liste de tous les operations effectuer
        [HttpGet]
        [Route("listOperations")]
        public async Task<ActionResult<IEnumerable<Operation>>> listOperations()
        {
            return await _context.Operations.ToListAsync();
        }

        // Get : Liste des operations effectuer par Categorie
        [HttpGet]
        [Route("listOperationCategorie")]
        public async Task<ActionResult<IEnumerable<Operation>>> listOperationCategorie(decimal idCategorie)
        {

            return await _context.Operations.Where(p => p.IdCategorie == idCategorie).ToListAsync();
        }

        private bool OperationExists(decimal id)
        {
            return _context.Operations.Any(e => e.IdOperation == id);
        }
    }
}
