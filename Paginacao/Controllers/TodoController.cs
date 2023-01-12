using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paginacao.Data;
using Paginacao.Models;
using Paginacao.ViewModels;

namespace Paginacao.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route("load")]
        public async Task<IActionResult> LoadAsync([FromServices] AppDbContext context)
        {
            for (var i = 0; i < 1348; i++)
            {
                var todo = new Todo()
                {
                    Id = i,
                    Done = false,
                    Date = DateTime.Now,
                    Title = $"Tarefa {i}"
                };
                await context.AddAsync(todo);
                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context, [FromQuery] int skip = 0, [FromQuery] int take = 25)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            
            return Ok(todos);
        }

        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var todo = await context
                .Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPost("todos")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };

            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("todos/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();

            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromBody] CreateTodoViewModel model, [FromRoute] int id)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }



    }
}
