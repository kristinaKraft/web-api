using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProdutos.Data;
using ApiProdutos.Entidade;

namespace ApiProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly Context _context;

        public ProdutosController(Context context)
        {
            _context = context;
        }

        [HttpGet("/api/[controller]/pages")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPagesProdutos(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize
            )
        {
            List<Produto> produtos = await _context.Produtos
                .AsNoTracking()
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
            foreach (Produto produto in produtos)
            {
                produto.Categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);
            }
            return produtos;
        }

        //api/produtos/filtro?descricao=cola
        [HttpGet("/api/[controller]/filtro")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosByDescricao(
          [FromQuery] string descricao)
        {
            List<Produto> lista = await _context.Produtos.ToListAsync();
            var produtos = (from prod in lista
                            where prod.Nome.ToLower().Contains(descricao.ToLower())
                            select prod).ToList();

            foreach (Produto produto in produtos)
            {
                produto.Categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);
            }
            return produtos;
        }

        // GET: api/Produtoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            foreach (Produto produto in produtos)
            {
                produto.Categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);
            }
            return produtos;
        }

        [HttpGet("/api/[controller]/categoria/{id}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosByCategoria(
            [FromRoute] int id)
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            var prodByCategoria =
                (from prod in produtos
                 where prod.CategoriaId == id
                 select prod).ToList();

            Categoria categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            foreach (Produto produto in prodByCategoria)
            {
                produto.Categoria = categoria;
            }
            return prodByCategoria;
            /* return (from prod in produtos
                     where prod.CategoriaId == id
                     select prod).ToList();*/

        }

        // GET: api/Produtoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // PUT: api/Produtoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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

        // POST: api/Produtoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            produto.CategoriaId = produto.Categoria.Id;
            produto.Categoria =
                await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == produto.CategoriaId);

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}