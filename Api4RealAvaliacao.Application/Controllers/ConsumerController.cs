using Api4RealAvaliacao.Domain.DTOs;
using Api4RealAvaliacao.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api4RealAvaliacao.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumerController : ControllerBase
{
    private readonly IConsumerService _consumerService;

    public ConsumerController(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }
    
    /// <summary>
    /// Adiciona uma nova pessoa ao sistema.
    /// </summary>
    /// <param name="consumer">Os dados da pessoa a serem adicionados.</param>
    /// <returns>Uma ação que representa a operação de adição, incluindo o ID da nova pessoa.</returns>
    [HttpPost]
    public async Task<IActionResult> AddPessoa([FromBody] ConsumerDTO consumer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var id = await _consumerService.AddPessoaAsync(consumer);
            return Ok(new { Id = id, Message = "Pessoa adicionada com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro ao adicionar a pessoa", Error = ex.Message });
        }
    }
    
    /// <summary>
    /// Atualiza o campo Optin para um usuário específico.
    /// </summary>
    /// <param name="idPessoa">ID da pessoa cujo optin será atualizado.</param>
    /// <param name="newValue">Novo valor para o campo optin.</param>
    /// <returns>Uma ação que representa a operação de atualização.</returns>
    [HttpPut("optin/{idPessoa}")]
    public async Task<IActionResult> UpdateOptin(int idPessoa, [FromBody] int newValue)
    {
        // Verifica se o valor de 'newValue' é válido (1 ou 2)
        if (newValue < 1 || newValue > 2)
        {
            return BadRequest(new { Message = "Valor inválido. Optin deve ser 1 (Optin) ou 2 (Optout)." });
        }
        
        try
        {
            var result = await _consumerService.UpdateOptinAsync(idPessoa, newValue);
            if (result)
            {
                return Ok(new { Message = "Optin atualizado com sucesso." });
            }
            return NotFound(new { Message = "Registro Optin não encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro ao atualizar optin.", Error = ex.Message });
        }
    }
    
    /// <summary>
    /// Obtém todos os consumidores registrados no sistema.
    /// </summary>
    /// <returns>Uma ação que representa a operação de recuperação de todos os consumidores.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllConsumers()
    {
        var consumers = await _consumerService.GetAllConsumersAsync();
        
        if (consumers == null || !consumers.Any())
        {
            return NotFound(new { Message = "Nenhum registro encontrado." });
        }

        return Ok(consumers);
    }
    
    /// <summary>
    /// Obtém uma pessoa específica pelo ID.
    /// </summary>
    /// <param name="idPessoa">ID da pessoa a ser recuperada.</param>
    /// <returns>Uma ação que representa a operação de recuperação da pessoa, se encontrada.</returns>
    [HttpGet("{idPessoa}")]
    public async Task<IActionResult> GetPessoaById(int idPessoa)
    {
        var result = await _consumerService.GetPessoaByIdAsync(idPessoa);
        
        if (result == null)
        {
            return NotFound(new { Message = "Nenhum registro encontrado." });
        }

        return Ok(result);
    }
}