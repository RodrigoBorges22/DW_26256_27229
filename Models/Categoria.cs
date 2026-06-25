using System.ComponentModel.DataAnnotations;
namespace DW_26256_27229.Models;
public class Categoria{
    public int Id { get; set; }
    [Required(ErrorMessage="O nome é obrigatório")]
    public string Nome { get; set; }
    public ICollection<Evento>? Eventos { get; set; }
}