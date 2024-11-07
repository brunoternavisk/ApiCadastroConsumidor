using Api4RealAvaliacao.Domain.DTOs;
using Api4RealAvaliacao.Domain.Entities;

namespace Api4RealAvaliacao.Domain.Interfaces;

public interface IConsumerRepository
{
    Task<int> InsertPessoaAsync(PessoaEntity pessoa);
    Task InserirTelefoneAsync(TelefoneEntity telefone, int idPessoa);
    Task<int> InserirEstabelecimentoAsync(EstabelecimentoEntity estabel);
    Task InserirRedeOptinAsync(int idPessoa, int idEstabelecimento, int optIn);
    Task<int?> ObterEstabelecimentoPorNomeAsync(string nome);
    Task UpdateOptinAsync(int idPessoa, int newValue);

    Task<int?> GetCurrentOptinAsync(int idPessoa);
    Task<IEnumerable<ConsumerResponseDTO>> GetAllPessoasAsync();
    Task<ConsumerResponseDTO> GetPessoaByIdAsync(int idPessoa);
}