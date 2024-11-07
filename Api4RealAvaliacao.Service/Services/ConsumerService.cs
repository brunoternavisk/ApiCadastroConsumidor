using Api4RealAvaliacao.Domain.DTOs;
using Api4RealAvaliacao.Domain.Entities;
using Api4RealAvaliacao.Domain.Interfaces;

namespace Api4RealAvaliacao.Service.Services;

public class ConsumerService : IConsumerService
{
    private readonly IConsumerRepository _consumerRepository;
    private readonly ILogRepository _logRepository;
    
    public ConsumerService(IConsumerRepository consumerRepository, ILogRepository logRepository)
    {
        _consumerRepository = consumerRepository;
        _logRepository = logRepository;
    }

    public async Task<int> AddPessoaAsync(ConsumerDTO consumer)
    {
        var log = new LogExecucaoEntity
        {
            DtHoraInicio = DateTime.Now,
            NomeExecucao = nameof(AddPessoaAsync),
            Status = "Erro"
        };
        
        try
        {
            var pessoaId = await _consumerRepository.InsertPessoaAsync(consumer.Pessoa);
            await _consumerRepository.InserirTelefoneAsync(consumer.Telefone, pessoaId);
            var estabelecimentoId = await InserirOuObterEstabelecimentoAsync(consumer.Estabelecimento);
            await _consumerRepository.InserirRedeOptinAsync(pessoaId, estabelecimentoId, consumer.RedeOptin.Optin);

            return pessoaId;
        }
        catch (Exception ex)
        {
            // Adiciona detalhes ao log
            log.DtHoraFim = DateTime.Now;
            log.Error = ex.Message;
            log.DetalheExecucao = "Erro ao adicionar pessoa";
            log.NumLinhas = 0;

            // Grava o log de execução
            await _logRepository.GravarLogExecucaoAsync(log);

            throw new Exception("Erro ao adicionar a pessoa", ex);
        }
    }
    
    public async Task<bool> UpdateOptinAsync(int idPessoa, int newValue)
    {
        var log = new LogExecucaoEntity
        {
            DtHoraInicio = DateTime.Now,
            NomeExecucao = nameof(UpdateOptinAsync),
            Status = "Erro"
        };

        try
        {
            var currentOptin = await _consumerRepository.GetCurrentOptinAsync(idPessoa);
            if (currentOptin == null)
            {
                throw new Exception("Registro Optin não encontrado.");
            }

            // Atualiza o campo optin
            await _consumerRepository.UpdateOptinAsync(idPessoa, newValue);
            
            //Log
            var logEntry = new LogAltTabelaEntity()
            {
                NomeCampo = "Optin",
                ValorInicial = currentOptin.ToString(),
                ValorAtualizado = newValue.ToString(),
                DataAtualizacao = DateTime.Now
            };

            await _logRepository.GravarLogAlterTabelaAsync(logEntry);

            return true;
        }
        catch (Exception ex)
        {
            // Adiciona detalhes ao log
            log.DtHoraFim = DateTime.Now;
            log.Error = ex.Message;
            log.DetalheExecucao = "Erro ao atualizar optin";
            log.NumLinhas = 0;

            // Grava o log de execução
            await _logRepository.GravarLogExecucaoAsync(log);

            throw new Exception("Erro ao atualizar optin", ex);
        }    
    }
    
    public async Task<IEnumerable<ConsumerResponseDTO>> GetAllConsumersAsync()
    {
        var log = new LogExecucaoEntity
        {
            DtHoraInicio = DateTime.Now,
            NomeExecucao = nameof(GetAllConsumersAsync),
            Status = "Erro"
        };

        try
        {
            return await _consumerRepository.GetAllPessoasAsync();
        }
        catch (Exception ex)
        {
            log.DtHoraFim = DateTime.Now;
            log.Error = ex.Message;
            log.DetalheExecucao = "Erro ao obter todos os consumidores";
            log.NumLinhas = 0;

            await _logRepository.GravarLogExecucaoAsync(log);
            throw new Exception("Erro ao obter todos os consumidores", ex);
        }
    }
    
    public async Task<ConsumerResponseDTO> GetPessoaByIdAsync(int idPessoa)
    {
        var log = new LogExecucaoEntity
        {
            DtHoraInicio = DateTime.Now,
            NomeExecucao = nameof(GetPessoaByIdAsync),
            Status = "Erro"
        };

        try
        {
            return await _consumerRepository.GetPessoaByIdAsync(idPessoa);
        }
        catch (Exception ex)
        {
            log.DtHoraFim = DateTime.Now;
            log.Error = ex.Message;
            log.DetalheExecucao = "Erro ao obter consumidor por ID";
            log.NumLinhas = 0;

            await _logRepository.GravarLogExecucaoAsync(log);
            throw new Exception("Erro ao obter consumidor por ID", ex);
        }
    }
    
    private async Task<int> InserirOuObterEstabelecimentoAsync(EstabelecimentoEntity estabel)
    {
        // Verifica se o estabelecimento já existe no banco de dados
        var estabelecimentoExistenteId = await _consumerRepository.ObterEstabelecimentoPorNomeAsync(estabel.Nome);
        
        if (estabelecimentoExistenteId.HasValue)
        {
            // Retorna o ID do estabelecimento existente se encontrado
            return estabelecimentoExistenteId.Value;
        }
        
        // Se não existir, realiza a inserção e retorna o novo ID
        return await _consumerRepository.InserirEstabelecimentoAsync(estabel);
    }
}