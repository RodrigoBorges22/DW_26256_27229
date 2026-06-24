namespace DW_26256_27229.Models;

public class Evento
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public DateTime DataHora { get; set; }
    public int VagasMaximas { get; set; }

    // Chave Estrangeira para a Categoria
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    // Propriedade de navegação
    public ICollection<Inscricao>? Inscricoes { get; set; }
}