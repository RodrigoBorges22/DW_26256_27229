using System.ComponentModel.DataAnnotations;
namespace DW_26256_27229.Models;
public class Inscricao{
    public int Id { get; set; }
    public int UtilizadorId { get; set; }
    public Utilizador? Utilizador { get; set; }
    public int EventoId { get; set; }
    public Evento? Evento { get; set; }
    [Required]
    public DateTime DataInscricao { get; set; }
}