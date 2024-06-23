using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefas.Context;
using ListaDeTarefas.Models;

namespace ListaDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
            var tarefas = await _context.Tarefas.ToListAsync();

            var result = tarefas.Select(tarefa => new
            {
                tarefa.Id,
                tarefa.Nome,
                DataTarefa = tarefa.DataTarefa.ToString("dd/MM/yy"),
                tarefa.HoraTarefa
            });

            if (_context.Tarefas == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (_context.Tarefas == null)
            {
                return NotFound();
            }

            if (tarefa == null)
            {
                return NotFound();
            }

            var result = new
            {
                tarefa.Id,
                tarefa.Nome,
                DataTarefa = tarefa.DataTarefa.ToString("dd/MM/yy"),
                tarefa.HoraTarefa
            };

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
        {
            if (_context.Tarefas == null)
            {
                return Problem("Erro ao criar o produto, tabela de Tarefas inexistente no banco de dados. Contate o suporte");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Dados incompletos",
                });
            }

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            var result = new
            {
                tarefa.Id,
                tarefa.Nome,
                DataTarefa = tarefa.DataTarefa.ToString("dd/MM/yy"),
                tarefa.HoraTarefa
            };

            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, result);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTarefa(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Dados incompletos",
                });
            }

            _context.Entry(tarefa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefaExists(id))
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


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            if (_context.Tarefas == null)
            {
                return Problem("Erro ao criar o produto, tabela de Tarefas inexistente no banco de dados. Contate o suporte");
            }

            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}
