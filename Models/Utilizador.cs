using System.ComponentModel.DataAnnotations;
namespace DW_26256_27229.Models;
public class Utilizador{
    public int Id { get; set; }
    [Required(ErrorMessage="O nome é obrigatório")]
    public string Nome { get; set; }
    [Required(ErrorMessage="O email é obrigatório"),EmailAddress(ErrorMessage="Email inválido")]
    public string Email { get; set; }
    [Required(ErrorMessage="A password é obrigatória")]
    public string Password { get; set; }
    [Required]
    public string TipoUtilizador { get; set; }
    public ICollection<Inscricao>? Inscricoes { get; set; }
}