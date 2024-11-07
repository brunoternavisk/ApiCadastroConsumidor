using Api4RealAvaliacao.Domain.DTOs;
using Api4RealAvaliacao.Domain.Entities;
using Api4RealAvaliacao.Domain.Interfaces;
using Dapper;

namespace Api4RealAvaliacao.Infra.Data;

public class ConsumerRepository : IConsumerRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public ConsumerRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async Task<int> InsertPessoaAsync(PessoaEntity pessoa)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
                string query = @"
                        INSERT INTO TB_Pessoa (Nome, CPF, Fl_Associado, Fl_Autoriza_Dados, DT_Inclusao)
                        OUTPUT INSERTED.Id
                        VALUES (@Nome, @Cpf, @Fl_Associado, @Fl_Autoriza_Dados, @DT_Inclusao)";

                var id = await connection.QuerySingleAsync<int>(query, new
                {
                    pessoa.Nome,
                    CPF = pessoa.Cpf,
                    Fl_Associado = pessoa.FLAssociado,
                    Fl_Autoriza_Dados = pessoa.FLAutorizaDados,
                    DT_Inclusao = DateTime.Now
                });

                return id;
        }
    }
    
    public async Task InserirTelefoneAsync(TelefoneEntity telefone, int idPessoa)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = @"
            INSERT INTO TB_Telefone (ID_Pessoa, DDD, Numero, DT_Inclusao)
            VALUES (@ID_Pessoa, @DDD, @Numero, @DT_Inclusao)";
        
            await connection.ExecuteAsync(query, new
            {
                ID_Pessoa = idPessoa,
                telefone.DDD,
                telefone.Numero,
                DT_Inclusao = DateTime.Now
            });
        }
    }
    
    public async Task<int> InserirEstabelecimentoAsync(EstabelecimentoEntity estabel)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = @"
            INSERT INTO TB_Estabelecimento (Nome, Nome_Fantasia, CNPJ_Grupo, CNPJ_Franquia, Status, DT_Inclusao)
            OUTPUT INSERTED.Id
            VALUES (@Nome, @Nome_Fantasia, @CNPJ_Grupo, @CNPJ_Franquia, @Status, @DT_Inclusao)";
        
            var id = await connection.QuerySingleAsync<int>(query, new
            {
                estabel.Nome,
                Nome_Fantasia = estabel.NomeFantasia,
                CNPJ_Grupo = estabel.CnpjGrupo,
                CNPJ_Franquia = estabel.CnpjFranquia,
                estabel.Status,
                DT_Inclusao = DateTime.Now
            });

            return id;
        }
    }
    
    public async Task InserirRedeOptinAsync(int idPessoa, int idEstabelecimento, int optIn)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            var dataInclusao = DateTime.Now;
            DateTime? dataOptout = optIn == 2 ? dataInclusao : (DateTime?)null;
            
            string query = @"
            INSERT INTO TB_Pessoa_Rede_OptIn (Id_pessoa, Optin, dt_inclusao, id_estabelecimento, dt_optout)
            VALUES (@Id_Pessoa, @Optin, @dt_inclusao, @id_estabelecimento, @dt_optout)";
        
            await connection.ExecuteAsync(query, new
            {
                Id_pessoa = idPessoa,
                Optin = optIn,
                dt_inclusao = dataInclusao,
                id_estabelecimento = idEstabelecimento,
                dt_optout = dataOptout
            });
        }
    }
    
    public async Task<int?> ObterEstabelecimentoPorNomeAsync(string nome)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = @"
            SELECT Id 
            FROM TB_Estabelecimento 
            WHERE Nome LIKE @Nome";
        
            var estabelecimentoId = await connection.QueryFirstOrDefaultAsync<int?>(query, new { Nome = $"%{nome}%" });
        
            return estabelecimentoId;
        }
    }
    
    public async Task UpdateOptinAsync(int idPessoa, int newValue)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = @"UPDATE TB_Pessoa_Rede_Optin SET Optin = @newValue, dt_optout = @dt_optout WHERE Id_pessoa = @idPessoa";
            
            await connection.ExecuteAsync(query, new
            {
                newValue,
                idPessoa,
                dt_optout = DateTime.Now
            });
        }
    }
    
    public async Task<int?> GetCurrentOptinAsync(int idPessoa)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            string query = "SELECT Optin FROM TB_Pessoa_Rede_Optin WHERE Id_pessoa = @Id_pessoa";
            return await connection.QuerySingleOrDefaultAsync<int?>(query, new { Id_pessoa = idPessoa });
        }
    }

    public async Task<IEnumerable<ConsumerResponseDTO>> GetAllPessoasAsync()
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            // Consulta SQL para buscar dados de todas as pessoas e suas informações relacionadas
            string query = @"
            SELECT 
                p.Id, 
                p.Nome, 
                p.CPF AS Cpf, 
                p.Fl_Associado AS FLAssociado, 
                p.Fl_Autoriza_Dados AS FLAutorizaDados,
                t.DDD,
                t.Numero,
                e.Nome AS Nome, 
                e.Nome_Fantasia AS NomeFantasia, 
                e.CNPJ_Grupo AS CnpjGrupo, 
                e.CNPJ_Franquia AS CnpjFranquia, 
                e.Status,
                o.Optin,
                o.dt_optout as DataOptout
            FROM 
                TB_Pessoa p
            LEFT JOIN 
                TB_Telefone t ON p.Id = t.Id_Pessoa
            LEFT JOIN
                TB_Pessoa_Rede_OptIn o ON p.Id = o.Id_pessoa
            LEFT JOIN 
                TB_Estabelecimento e ON o.Id_estabelecimento = e.Id;";

            // Executa a consulta e mapeia o resultado para uma lista de ConsumerDTO
            var consumerResponses = await connection.QueryAsync<ConsumerPessoaDTO, TelefoneEntity, EstabelecimentoEntity, ConsumerRedeOptinDTO, ConsumerResponseDTO>(
                query,
                (pessoa, telefone, estabelecimento, redeOptin) =>
                {
                    return new ConsumerResponseDTO
                    {
                        Pessoa = pessoa,
                        Telefone = telefone,
                        Estabelecimento = estabelecimento,
                        RedeOptin = redeOptin
                    };
                },
                splitOn: "Id,DDD,Nome,Optin"
            );

            return consumerResponses;
        }
    }
    
    public async Task<ConsumerResponseDTO> GetPessoaByIdAsync(int idPessoa)
    {
        using (var connection = _databaseConnection.CreateConnection())
        {
            // Consulta SQL para buscar dados de uma pessoa específica e suas informações relacionadas
            string query = @"
        SELECT 
            p.Id, 
            p.Nome, 
            p.CPF AS Cpf, 
            p.Fl_Associado AS FLAssociado, 
            p.Fl_Autoriza_Dados AS FLAutorizaDados,
            t.DDD,
            t.Numero,
            e.Nome AS Nome, 
            e.Nome_Fantasia AS NomeFantasia, 
            e.CNPJ_Grupo AS CnpjGrupo, 
            e.CNPJ_Franquia AS CnpjFranquia, 
            e.Status,
            o.Optin,
            o.dt_optout as DataOptout
        FROM 
            TB_Pessoa p
        LEFT JOIN 
            TB_Telefone t ON p.Id = t.Id_Pessoa
        LEFT JOIN
            TB_Pessoa_Rede_OptIn o ON p.Id = o.Id_pessoa
        LEFT JOIN 
            TB_Estabelecimento e ON o.Id_estabelecimento = e.Id
        WHERE 
            p.Id = @idPessoa;";

            // Executa a consulta e mapeia o resultado para um objeto ConsumerResponseDTO
            var consumerResponse = await connection.QueryAsync<ConsumerPessoaDTO, TelefoneEntity, EstabelecimentoEntity, ConsumerRedeOptinDTO, ConsumerResponseDTO>(
                query,
                (pessoa, telefone, estabelecimento, redeOptin) =>
                {
                    return new ConsumerResponseDTO
                    {
                        Pessoa = pessoa,
                        Telefone = telefone,
                        Estabelecimento = estabelecimento,
                        RedeOptin = redeOptin
                    };
                },
                new { idPessoa },  // Passa o parâmetro para a consulta
                splitOn: "Id,DDD,Nome,Optin"
            );

            // Retorna o primeiro item ou nulo se não houver resultado
            return consumerResponse.FirstOrDefault();
        }
    }
}