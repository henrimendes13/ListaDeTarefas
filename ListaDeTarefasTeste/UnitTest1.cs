using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using System.Threading.Tasks;
using ListaDeTarefas.Context;
using ListaDeTarefas.Controllers;
using ListaDeTarefas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefasTeste;

public class TarefasControllerTest
{
    private readonly DbContextOptions<AppDbContext> _options;
    private readonly AppDbContext _context;

    public TarefasControllerTest()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ListaDeTarefas")
            .Options;
        _context = new AppDbContext(_options);
    }

    [Fact]
    public async Task GetTarefas_DeveRetornarTarefas()
    {
        _context.Tarefas.Add(new Tarefa { Id = 1, Nome = "Tarefa 1", HoraTarefa = "10:00" });
        _context.Tarefas.Add(new Tarefa { Id = 2, Nome = "Tarefa 2", HoraTarefa = "11:00" });
        _context.SaveChanges();

        var controller = new TarefasController(_context);

        var result = await controller.GetTarefas();

        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<dynamic>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetTarefas_DeveRetornarNotFound_SeNaoHouverTarefas()
    {
        var controller = new TarefasController(_context);

        var result = await controller.GetTarefas();

        var actionResult = Assert.IsType<NotFoundResult>(result.Result);

    }

}