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
    public class CategoriesController : ControllerBase
    {
        private readonly BudgetContext _context;

        public CategoriesController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        [Route("ListCategories")]
        public async Task<ActionResult<IEnumerable<Categorie>>> ListCategories()
        {
            return await _context.Categories.Where(p=>p.ArchivedCategorie==1).ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categorie>> GetCategorie(decimal id)
        {
            var categorie =  _context.Categories.Where(p=>p.ArchivedCategorie==1 && p.IdCategorie==id).FirstOrDefault();

            if (categorie == null)
            {
                return new JsonResult(new { statusCode = BadRequest(), message = "Cette catégorie  n'existe pas" });
            }

            return new JsonResult(new { statusCode = Ok(), data= categorie });

        }


        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("AddCategories")]
        public async Task<ActionResult<Categorie>> AddCategories(CategorieDto categorieDto)
        {
            Categorie categorie = new Categorie();
            categorie.LibelleCategorie = categorieDto.LibelleCategorie;
            categorie.ArchivedCategorie = 1;
            categorie.CreateCategorie=DateTime.Now;
            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();

            return new JsonResult(new { statusCode = Ok(), message = "Catégorie opération ajouter avec succès" });
        }

        // DELETE: api/Categories/5
        [HttpGet]
        [Route("DeleteCategorie")]
        public async Task<IActionResult> DeleteCategorie(decimal id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            categorie.ArchivedCategorie = 0;
            if (categorie == null)
            {
                return new JsonResult(new { statusCode = BadRequest(), message = "Cette catégorie  n'existe pas" });
            }

            _context.Entry(categorie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new JsonResult(new { statusCode = Ok(), message = "Catégorie  supprimé avec succès" });
        }

        private bool CategorieExists(decimal id)
        {
            return _context.Categories.Any(e => e.IdCategorie == id);
        }
    }
}
