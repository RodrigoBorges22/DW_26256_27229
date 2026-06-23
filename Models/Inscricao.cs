namespace DW_26256_27229.Models;

public class Inscricao
{
    public int Id { get; set; }
    
    // Chave Estrangeira para o Utilizador
    public int UtilizadorId { get; set; }
    public Utilizador Utilizador { get; set; }
    
    // Chave Estrangeira para o Evento
    public int EventoId { get; set; }
    public Evento Evento { get; set; }
    
    public DateTime DataInscricao { get; set; }
}