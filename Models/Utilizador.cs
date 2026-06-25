namespace DW_26256_27229.Models;

public class Utilizador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string TipoUtilizador { get; set; } // Ex: "Professor" ou "Aluno"
    
    // Propriedade de navegação (Um Utilizador faz várias Inscrições)
    public ICollection<Inscricao>? Inscricoes { get; set; }
}