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
    public class SaidasController : ControllerBase
    {
        private readonly DataContext _context;

        public SaidasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Saidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Saida>>> GetEstoques()
        {
            var saidas = await _context.Saidas.ToListAsync();
            return StatusCode(200, saidas);
        }

        // GET: api/Saidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Saida>> GetSaida(int id)
        {
            var saida = await _context.Saidas.FindAsync(id);

            if (saida == null)
            {
                return NotFound();
            }
            return StatusCode(200, saida);
        }

        // PUT: api/Saidas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaida(int id, Saida saida)
        {
            if (id != saida.Id)
            {
                return BadRequest();
            }
            if (saida.Valor < 1)
            {
                return BadRequest("Saída não pode ser negativa");
            }
            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(saida.Data.Day) && x.Data.Month.Equals(saida.Data.Month)).FirstOrDefaultAsync();
            var saidaAntiga = await _context.Saidas.FindAsync(id);
            if (saida.Valor > saidaAntiga.Valor)
            {
                resultado.Saida += (saida.Valor - saidaAntiga.Valor);
                resultado.ResultadoFinanceiro -= (saida.Valor - saidaAntiga.Valor);
                _context.Entry(saidaAntiga).State = EntityState.Detached;
            }
            if (saida.Valor < saidaAntiga.Valor)
            {
                resultado.Saida -= (saidaAntiga.Valor - saida.Valor);
                resultado.ResultadoFinanceiro += (saidaAntiga.Valor - saida.Valor);
                _context.Entry(saidaAntiga).State = EntityState.Detached;
            }
            _context.Resultados.Update(resultado);
            _context.Entry(saida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaidaExists(id))
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

        // POST: api/Saidas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Saida>> PostSaida(Saida saida)
        {
            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(saida.Data.Day) && x.Data.Month.Equals(saida.Data.Month)).FirstOrDefaultAsync();
            if (resultado.Equals(null))
            {
                var resultadoFinanceiro = new Resultado
                {
                    Data = saida.Data,
                    Entrada = 0,
                    Saida = saida.Valor,
                    ResultadoFinanceiro = saida.Valor
                };
                _context.Resultados.Add(resultadoFinanceiro);
            }
            else
            {
                resultado.Saida += saida.Valor;
                resultado.ResultadoFinanceiro = resultado.Entrada - resultado.Saida;
                _context.Resultados.Update(resultado);
            }

            _context.Saidas.Add(saida);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaida", new { id = saida.Id }, saida);
        }

        // DELETE: api/Saidas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaida(int id)
        {
            var saida = await _context.Saidas.FindAsync(id);
            if (saida == null)
            {
                return NotFound();
            }

            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(saida.Data.Day) && x.Data.Month.Equals(saida.Data.Month)).FirstOrDefaultAsync();
            resultado.Saida -= saida.Valor;
            resultado.ResultadoFinanceiro += saida.Valor;
            _context.Resultados.Update(resultado);

            _context.Saidas.Remove(saida);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaidaExists(int id)
        {
            return _context.Saidas.Any(e => e.Id == id);
        }
    }
}
