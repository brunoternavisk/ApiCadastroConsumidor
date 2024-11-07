using Api4RealAvaliacao.Domain.Entities;

namespace Api4RealAvaliacao.Domain.DTOs;

public class ConsumerDTO
{
    public PessoaEntity Pessoa { get; set; }
    public TelefoneEntity Telefone { get; set; }
    public EstabelecimentoEntity Estabelecimento { get; set; }
    public RedeOptinEntity RedeOptin { get; set; }
}