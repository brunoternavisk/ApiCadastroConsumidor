using System.Text.Json.Serialization;

namespace Api4RealAvaliacao.Domain.Entities;

public class EstabelecimentoEntity
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Nome { get; set; }
    public string NomeFantasia { get; set; }
    public string CnpjGrupo { get; set; }
    public string CnpjFranquia { get; set; }
    public int Status { get; set; }
}