using System.Text.Json.Serialization;

namespace Api4RealAvaliacao.Domain.Entities;

public class TelefoneEntity
{
    [JsonIgnore]
    public int Id { get; set; }
    // public int IdPessoa { get; set; }
    public string DDD { get; set; }
    public string Numero { get; set; }
}

