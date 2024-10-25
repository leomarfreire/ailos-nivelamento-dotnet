using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Services.Controllers
{
	[Route("api/conta-corrente")]
	public class ContaCorrenteController : ControllerBase
	{
		private readonly ILogger<ContaCorrenteController> _logger;
		private readonly IMediator _mediator;

		public ContaCorrenteController(ILogger<ContaCorrenteController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		[HttpPost("movimentar")]
		public async Task<ActionResult<string>> PostMovimentacoes([FromBody] MovimentacaoRequest request)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errorMessage = string.Join("; ", ModelState.Values
						.SelectMany(x => x.Errors)
						.Select(x => x.ErrorMessage));

					throw new BusinessException("VALIDATION_ERROR", $"A requisição contém dados inválidos: {errorMessage}");
				}

				var resultado = await _mediator.Send(request);
				return Ok(new MovimentacaoResponse { IdMovimento = resultado });
			}
			catch (BusinessException businessEx)
			{
				_logger.LogWarning(businessEx, "Erro de negócio: {Message}", businessEx.Mensagem);

				return StatusCode((int)HttpStatusCode.BadRequest, new
				{
					error = businessEx.Type,
					message = businessEx.Mensagem
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao processar movimentação.");
				return StatusCode(500, new { error = "INTERNAL_SERVER_ERROR", message = "Ocorreu um erro ao processar a movimentação." });
			}
		}

		[HttpGet("{idContaCorrente}/saldo")]
		public async Task<ActionResult<SaldoResponse>> GetSaldo(string idContaCorrente)
		{
			try
			{
				var saldo = await _mediator.Send(new SaldoRequest { IdContaCorrente = idContaCorrente });

				if (saldo == null)
				{
					return NotFound(new { message = "Conta corrente não encontrada." });
				}

				saldo.DataHoraResposta = DateTime.UtcNow;

				return Ok(saldo);
			}
			catch (BusinessException businessEx)
			{
				_logger.LogWarning(businessEx, "Erro de negócio: {Message}", businessEx.Mensagem);

				return StatusCode((int)HttpStatusCode.BadRequest, new
				{
					error = businessEx.Type,
					message = businessEx.Mensagem
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao obter saldo da conta corrente.");
				return StatusCode(500, new { error = "INTERNAL_SERVER_ERROR", message = "Ocorreu um erro ao obter o saldo." });
			}
		}
	}
}
