using Api4RealAvaliacao.Domain.Entities;
using Api4RealAvaliacao.Domain.Interfaces;
using Dapper;

namespace Api4RealAvaliacao.Infra.Data;

public class LogRepository : ILogRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public LogRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async Task GravarLogExecucaoAsync(LogExecucaoEntity log)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = @"
            INSERT INTO TB_Log_Execucao (DtHoraInicio, DtHoraFim, NumLinhas, NomeExecucao, DetalheExecucao, Status, Error, dt_inclusao)
            VALUES (@DtHoraInicio, @DtHoraFim, @NumLinhas, @NomeExecucao, @DetalheExecucao, @Status, @Error, @dt_inclusao)";

            await connection.ExecuteAsync(query, new
            {
                log.DtHoraInicio,
                log.DtHoraFim,
                log.NumLinhas,
                log.NomeExecucao,
                log.DetalheExecucao,
                log.Status,
                log.Error,
                dt_inclusao = DateTime.Now
            });
        }
    }
    
    public async Task GravarLogAlterTabelaAsync(LogAltTabelaEntity logEntry)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = @"
                INSERT INTO TB_Log_Altera_Tabela (Campo_Alterado, OldValue, NewValue, DataAlteracao)
                VALUES (@Campo_Alterado, @OldValue, @NewValue, @DataAlteracao)";
                
            await connection.ExecuteAsync(query, new
            {
                Campo_Alterado = logEntry.NomeCampo,
                OldValue = logEntry.ValorInicial,
                NewValue = logEntry.ValorAtualizado,
                DataAlteracao = logEntry.DataAtualizacao
            });
        }
    }

}