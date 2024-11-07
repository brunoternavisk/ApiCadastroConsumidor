using Api4RealAvaliacao.Domain.Entities;

namespace Api4RealAvaliacao.Domain.Interfaces;

public interface ILogRepository
{
    Task GravarLogExecucaoAsync(LogExecucaoEntity log);
    Task GravarLogAlterTabelaAsync(LogAltTabelaEntity logEntry);
}