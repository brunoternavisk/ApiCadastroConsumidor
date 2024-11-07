using System;

namespace Api4RealAvaliacao.Domain.Entities;

public class LogExecucaoEntity
{
    public DateTime DtHoraInicio { get; set; }
    public DateTime DtHoraFim { get; set; }
    public int NumLinhas { get; set; }
    public string NomeExecucao { get; set; }
    public string DetalheExecucao { get; set; }
    public string Status { get; set; }
    public string Error { get; set; }
}