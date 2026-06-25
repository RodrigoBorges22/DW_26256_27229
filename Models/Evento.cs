using System.ComponentModel.DataAnnotations;
namespace DW_26256_27229.Models;
public class Evento{
    public int Id { get; set; }
    [Required(ErrorMessage="O título é obrigatório")]
    public string Titulo { get; set; }
    [Required]
    public DateTime DataHora { get; set; }
    [Range(1,int.MaxValue,ErrorMessage="Tem de ter pelo menos 1 vaga")]
    public int VagasMaximas { get; set; }
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public ICollection<Inscricao>? Inscricoes { get; set; }
}