namespace ListaDeTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool? Finalizada { get; set; }


    }
}
