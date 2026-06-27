using System.ComponentModel.DataAnnotations;
namespace DW_26256_27229.Models;

public class Utilizador
{
    public int Id { get; set; }
    // Dados de identificação do utilizador
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }
    // Validação de formato de email
    [Required(ErrorMessage = "O email é obrigatório"), EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    // Password para autenticação
    [Required(ErrorMessage = "A password é obrigatória")]
    public string Password { get; set; }
    // Define se é 'Admin', 'Professor' ou 'Aluno'
    [Required]
    public string TipoUtilizador { get; set; }
    // Lista das inscrições associadas a este utilizador
    public ICollection<Inscricao>? Inscricoes { get; set; }
}