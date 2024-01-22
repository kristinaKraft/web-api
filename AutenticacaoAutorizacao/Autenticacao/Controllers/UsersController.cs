using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autenticacao.Data;
using Autenticacao.Models;
using Microsoft.IdentityModel.Tokens;
using Autenticacao.Config;
using Autenticacao.Dtos;

namespace Autenticacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthContext _context;

        public UsersController(AuthContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusuarios()
        {
            return await _context.Users.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> login(UserDto user)
        {
            string token = "";
            var users = await _context.Users.ToListAsync();
            var userLogado = (from u in users
                              where u.Username == user.UserName & u.Password == user.Password
                              select u).ToList();
            if (!userLogado.IsNullOrEmpty())
            {
                token = TokenService.GenerateToken(userLogado[0]);
            }

            return new { token = token };
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}