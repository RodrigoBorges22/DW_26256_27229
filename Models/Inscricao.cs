using System.ComponentModel.DataAnnotations;
namespace DW_26256_27229.Models;

public class Inscricao
{
    // Chaves estrangeiras para ligar aluno e evento
    public int UtilizadorId { get; set; }
    public Utilizador? Utilizador { get; set; }
    public int EventoId { get; set; }
    public Evento? Evento { get; set; }
    // Registo obrigatório da data de inscrição
    [Required]
    public DateTime DataInscricao { get; set; }
}