using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [Authorize()]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly DataContext _context;

        public PedidosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                            .Include(e => e.ProdutosPedidos)
                            .ThenInclude(e => e.Produto)
                            .Include(e => e.Cliente)
                           .ToListAsync();

            return StatusCode(200, pedidos);
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                            .Include(p => p.ProdutosPedidos)
                            .ThenInclude(p => p.Produto)
                            .Include(p => p.Cliente)
                            .FirstOrDefaultAsync(x => x.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }
            _context.Entry(pedido).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            var cliente = await _context.Clientes.Where(x => x.Id.Equals(pedido.IdCliente)).FirstOrDefaultAsync();
            var entrada = new Entrada
            {
                Nome = "Pedido",
                Descricao = "Cliente: " + cliente.Nome,
                Valor = pedido.ValorTotal,
                Data = pedido.DataDoPedido,
                PedidoId = pedido.Id
            };

            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(pedido.DataDoPedido.Day) && x.Data.Month.Equals(pedido.DataDoPedido.Month)).FirstOrDefaultAsync();
            if (resultado.Equals(null))
            {
                var resultadoFinanceiro = new Resultado
                {
                    Data = entrada.Data,
                    Entrada = entrada.Valor,
                    Saida = 0,
                    ResultadoFinanceiro = entrada.Valor
                };
                _context.Resultados.Add(resultadoFinanceiro);
            }
            else
            {
                resultado.Entrada += entrada.Valor;
                resultado.ResultadoFinanceiro = resultado.Entrada - resultado.Saida;
                _context.Resultados.Update(resultado);
            }
         
            _context.Entradas.Add(entrada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas.Where(x => x.PedidoId.Equals(id)).FirstOrDefaultAsync();
            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(entrada.Data.Day) && x.Data.Month.Equals(entrada.Data.Month)).FirstOrDefaultAsync();
            resultado.Entrada -= entrada.Valor;
            resultado.ResultadoFinanceiro -= entrada.Valor;

            _context.Entradas.Remove(entrada);
            _context.Resultados.Update(resultado);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
