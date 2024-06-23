using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaDeTarefas.Models;

public class Tarefa
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    public string Nome { get; set; }

    
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "O campo DataTarefa é obrigatório")]
    public DateTime DataTarefa { get; set; }

    public string HoraTarefa { get; set; }


}