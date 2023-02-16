using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIAbyster.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIBudget.dto;

namespace WebAPIBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BudgetContext _context;

        public UsersController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Route("listUsers")]
        public async Task<ActionResult<IEnumerable<User>>> listUsers()
        {
            return await _context.Users.Where(p=>p.ArchivedUser==1).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet]
        [Route("GetUser")]
        public async Task<ActionResult<Categorie>> GetUser(decimal id)
        {
            var user = _context.Users.Where(p => p.ArchivedUser == 1 && p.IdUser == id).FirstOrDefault();

            if (user == null)
            {
                return new JsonResult(new { statusCode = BadRequest(), message = "Cette utilisateur  n'existe pas" });
            }

            return new JsonResult(new { statusCode = Ok(), data = user });

        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
     /*   [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/


        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<User>> AddUser(userDto userDto)
        {
            try
            {
                if (!userDto.MdpUser.Equals(userDto.CMdpUser))
                {
                    return new JsonResult(new { statusCode = BadRequest(), message = "Mot de passe de confirmation incorrect" });
                }

                var currentUser = _context.Users.Where(p => p.EmailUser == userDto.EmailUser).FirstOrDefault();
                if (currentUser != null)
                {
                    return new JsonResult(new { statusCode = BadRequest(), message = "Cette utilisateur existe déja dans le systeme" });
                }

                User user = new User();
                // marque si user est supprimer ou pas
                user.ArchivedUser = 1;

                //marque si l'user est actif ou pas 
                user.StatusUser = true;
                user.NomUser = userDto.NomUser;
                user.PrenomUser = userDto.PrenomUser;
                user.EmailUser = userDto.EmailUser;
                user.RoleUser = Constante.roleUser;
                user.CreatedUser = DateTime.Now;

                user.MdpUser = Constante.Encrypt(userDto.MdpUser);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new JsonResult(new { statusCode = Ok(), message = "Utilisateur ajouter avec succès", role = user.RoleUser });

            }
            catch (Exception e)
            {
                return new JsonResult(new { statusCode = BadRequest() });
            } 

        }

    
        //Supprimer un utilisateur
        [HttpGet]
        [Route("DeleteUtilisateur")]
        public async Task<IActionResult> DeleteUtilisateur(decimal id)
        {
            var user = await _context.Users.FindAsync(id);
            user.ArchivedUser = 0;
            if (user == null)
            {
                return new JsonResult(new { statusCode = BadRequest(), message = "Cette utilisateur  n'existe pas" });
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new JsonResult(new { statusCode = Ok(), message = "utilisateur  supprimé avec succès" });
        }

        //Fonction de Connexion
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(String email, String pwd)
        {

            try
            {
                var user = _context.Users.Where(p => p.EmailUser == email).FirstOrDefault();

                if (user == null)
                {
                    return new JsonResult(new { statusCode = Unauthorized(), message = "Login ou mot de passe incorrect" });
                }
                else
                {
                    if (user.StatusUser != true)
                    {
                        return new JsonResult(new { statusCode = Unauthorized(), message = "Impossible de vous connecter car votre compte n'est pas actif" });
                    }
                    // Verification du hachage en BD
                    if (Constante.Decrypt(user.MdpUser).Equals(pwd))
                    {
                        return new JsonResult(new { statusCode = Ok(), message = "Utilisateur connecter avec succès", role = user.RoleUser });

                    }
                    else
                    {
                        return new JsonResult(new { statusCode = Unauthorized(), message = "Login ou mot de passe incorrect" });

                    }
                }
            }
            catch (Exception )
            {
                return new JsonResult(new { statusCode = BadRequest() });

            }

        }

        // GET: api/Users/5
        [HttpGet]
        [Route("activeUser")]
        public async Task<ActionResult<User>> statusUser(int id)
        {
            var user = _context.Users.Where(p=>p.ArchivedUser==1 && p.IdUser==id).FirstOrDefault();

            if (user == null)
            {
                return new JsonResult(new { statusCode = BadRequest(), message = "Cette utilisateur  n'existe pas" });
            }
            user.StatusUser = user.StatusUser == true ? false : true; 
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new JsonResult(new { statusCode = Ok(), message = "status utilisateur modifié avec succès" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }
    }
}
