namespace DW_26256_27229.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; }
    
    // Propriedade de navegação (Uma Categoria tem vários Eventos)
    public ICollection<Evento> Eventos { get; set; }
}