﻿namespace CRM.Domain.Models
{
    public record BaseFilterParams
    {
        public string? UsuarioCriacao { get; init; }
        public string? UsuarioAlteracao { get; init; }
        public DateTime? DataAlteracao { get; init; }
        public bool? Ativo { get; init; }
        public DateTime? DataCriacaoInicio { get; init; }
        public DateTime? DataCriacaoFim { get; init; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10; 
    }
}
