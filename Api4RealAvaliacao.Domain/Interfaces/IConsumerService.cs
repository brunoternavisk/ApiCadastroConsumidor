using Api4RealAvaliacao.Domain.DTOs;

namespace Api4RealAvaliacao.Domain.Interfaces;

public interface IConsumerService
{
    Task<int> AddPessoaAsync(ConsumerDTO consumer);
    Task<bool> UpdateOptinAsync(int idPessoa, int newValue);
    Task<IEnumerable<ConsumerResponseDTO>> GetAllConsumersAsync();
    Task<ConsumerResponseDTO> GetPessoaByIdAsync(int idPessoa);
}