using Api4RealAvaliacao.Domain.Entities;

namespace Api4RealAvaliacao.Domain.DTOs;

public class ConsumerResponseDTO
{
    public ConsumerPessoaDTO Pessoa { get; set; }
    public TelefoneEntity Telefone { get; set; }
    public EstabelecimentoEntity Estabelecimento { get; set; }
    public ConsumerRedeOptinDTO RedeOptin { get; set; }
}