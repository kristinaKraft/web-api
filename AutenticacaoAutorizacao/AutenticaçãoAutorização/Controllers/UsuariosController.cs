using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutenticacaoAutorizacao.Data;
using AutenticacaoAutorizacao.Models;
using Microsoft.IdentityModel.Tokens;
using AutenticacaoAutorizacao.Config;
using AutenticacaoAutorizacao.Dbos;

namespace AutenticacaoAutorizacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AuthContext _context;

        public UsuariosController(AuthContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
              

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // POST: api/Usuarios
        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> login(UserDtos user)
        {
            string token = "";
            var users = await _context.Usuarios.ToListAsync();
            var userLogado = (from u in users
                              where u.NomeUsuario == user.NomeUsuario & u.Senha == user.Senha
                              select u).ToList();
            if (!userLogado.IsNullOrEmpty())
            {
                token = TokenService.GenerateToken(userLogado[0]);
            }

            return new { token = token };
        }
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
