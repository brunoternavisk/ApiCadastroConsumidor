using System;
using System.Text.Json.Serialization;

namespace Api4RealAvaliacao.Domain.Entities;

public class LogAltTabelaEntity
{
    [JsonIgnore]
    public int Id { get; set; }
    public string NomeCampo { get; set; }
    public string ValorInicial { get; set; }
    public string ValorAtualizado { get; set; }
    public DateTime DataAtualizacao { get; set; }
}