using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DW_26256_27229.Models;

public class Evento
{
    public int Id { get; set; }

    // Título do evento com validação obrigatória
    [Required(ErrorMessage = "O título é obrigatório")]
    public string Titulo { get; set; } = string.Empty;

    // Data e hora do evento (Garante que é obrigatória e no futuro)
    [Required(ErrorMessage = "A data e hora são obrigatórias")]
    [Display(Name = "Data e Hora")]
    [DataFutura]
    public DateTime DataHora { get; set; }

    // Validação de intervalo para garantir vagas positivas
    [Range(1, int.MaxValue, ErrorMessage = "Tem de ter pelo menos 1 vaga")]
    [Display(Name = "Vagas Máximas")]
    public int VagasMaximas { get; set; }

    // Chaves estrangeiras para Categoria e Professor (Utilizador)
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public int UtilizadorId { get; set; }
    public Utilizador? Utilizador { get; set; }

    // Relacionamento com as inscrições dos alunos
    public ICollection<Inscricao>? Inscricoes { get; set; }
}

// validacao
public class DataFuturaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime data && data <= DateTime.Now)
        {
            return new ValidationResult("A data e hora do evento têm de ser no futuro.");
        }
        return ValidationResult.Success;
    }
}