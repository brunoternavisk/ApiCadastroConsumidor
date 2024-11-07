using System;
using System.Text.Json.Serialization;

namespace Api4RealAvaliacao.Domain.Entities;

public class RedeOptinEntity
{
    [JsonIgnore]
    public int Id { get; set; }
    public int Optin { get; set; }
}