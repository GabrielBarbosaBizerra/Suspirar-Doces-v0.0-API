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
    public class EntradasController : ControllerBase
    {
        private readonly DataContext _context;

        public EntradasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Entradas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entrada>>> GetEntradas()
        {
            var entradas = await _context.Entradas.ToListAsync();
            return StatusCode(200, entradas);
        }

        // GET: api/Entradas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrada>> GetEntrada(int id)
        {
            var entrada = await _context.Entradas.FindAsync(id);

            if (entrada == null)
            {
                return NotFound();
            }
            return StatusCode(200, entrada);
        }

        // PUT: api/Entradas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrada(int id, Entrada entrada)
        {
            if (id != entrada.Id)
            {
                return BadRequest();
            }
            if(entrada.Valor < 1)
            {
                return BadRequest("Entrada não pode ser negativa");
            }
            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(entrada.Data.Day) && x.Data.Month.Equals(entrada.Data.Month)).FirstOrDefaultAsync();
            var entradaAntiga = await _context.Entradas.FindAsync(id);
            if(entrada.Valor > entradaAntiga.Valor)
            {
                resultado.Entrada += (entrada.Valor - entradaAntiga.Valor);
                resultado.ResultadoFinanceiro += (entrada.Valor - entradaAntiga.Valor);
                _context.Entry(entradaAntiga).State = EntityState.Detached;
            }
            if(entrada.Valor < entradaAntiga.Valor)
            {
                resultado.Entrada -= (entradaAntiga.Valor - entrada.Valor);
                if(resultado.Entrada <= 0)
                {
                    resultado.Entrada = 0;
                }
                resultado.ResultadoFinanceiro -= (entradaAntiga.Valor - entrada.Valor);
                _context.Entry(entradaAntiga).State = EntityState.Detached;
            }
            _context.Resultados.Update(resultado);
            _context.Entry(entrada).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntradaExists(id))
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

        // POST: api/Entradas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entrada>> PostEntrada(Entrada entrada)
        {
            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(entrada.Data.Day) && x.Data.Month.Equals(entrada.Data.Month)).FirstOrDefaultAsync();
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

            return CreatedAtAction("GetEntrada", new { id = entrada.Id }, entrada);
        }

        // DELETE: api/Entradas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrada(int id)
        {
            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            var resultado = await _context.Resultados.Where(x => x.Data.Day.Equals(entrada.Data.Day) && x.Data.Month.Equals(entrada.Data.Month)).FirstOrDefaultAsync();
            resultado.Entrada -= entrada.Valor; 
            resultado.ResultadoFinanceiro -= entrada.Valor;
            if(resultado.Entrada <= 0)
            {
                resultado.Entrada = 0;
            }
            if(resultado.ResultadoFinanceiro <= 0 && resultado.Saida.Equals(0))
            {
                resultado.ResultadoFinanceiro = 0;
            }
            _context.Resultados.Update(resultado);
            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntradaExists(int id)
        {
            return _context.Entradas.Any(e => e.Id == id);
        }
    }
}
