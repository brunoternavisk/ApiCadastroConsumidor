namespace Api4RealAvaliacao.Domain.DTOs;

public class ConsumerPessoaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Cidade { get; set; }
    public int Idade { get; set; }
    public string FLAssociado { get; set; }
    public string FLAutorizaDados { get; set; }
}