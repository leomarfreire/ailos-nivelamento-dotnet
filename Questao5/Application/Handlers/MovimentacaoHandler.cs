using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.Context;

namespace Questao5.Application.Handlers
{
	public class MovimentacaoHandler : IRequestHandler<MovimentacaoRequest, string>
	{
		private readonly ContaCorrenteQueryService _contaCorrenteQueryService;
		private readonly IdempotenciaQueryService _idempotenciaQueryService;
		private readonly MovimentoQueryService _movimentoQueryService;
		private readonly DatabaseContext _dbContext;

		public MovimentacaoHandler(ContaCorrenteQueryService contaCorrenteQueryService, IdempotenciaQueryService idempotenciaQueryService, MovimentoQueryService movimentoQueryService, DatabaseContext dbContext)
		{
			_contaCorrenteQueryService = contaCorrenteQueryService;
			_idempotenciaQueryService = idempotenciaQueryService;
			_movimentoQueryService = movimentoQueryService;
			_dbContext = dbContext;
		}

		public async Task<string> Handle(MovimentacaoRequest request, CancellationToken cancellationToken)
		{
			var idenpotencia = await _idempotenciaQueryService.ObterPorId(request.IdentificacaoRequisicao);
			if (idenpotencia != null)
			{
				return idenpotencia.Resultado;
			}

			var contaCorrente = await _contaCorrenteQueryService.ObterPorId(request.IdentificacaoContaCorrente);
			if (contaCorrente == null)
			{
				throw new BusinessException("INVALID_ACCOUNT", "Apenas contas correntes cadastradas podem receber movimentação");
			}
			else if (!contaCorrente.Ativo)
			{
				throw new BusinessException("INVALID_ACCOUNT", "Apenas contas correntes ativas podem receber movimentação");
			}

			var movimento = new Movimento
			{
				IdMovimento = Guid.NewGuid().ToString(),
				IdContaCorrente = request.IdentificacaoContaCorrente,
				DataMovimento = DateTime.UtcNow,
				TipoMovimento = request.TipoMovimento.ToUpper(),
				Valor = request.Valor
			};

			var resultado = movimento.IdMovimento;

			var novaIdempotencia = new Idempotencia
			{
				ChaveIdempotencia = request.IdentificacaoRequisicao,
				Requisicao = request.ToString(),
				Resultado = resultado
			};

			await _movimentoQueryService.Salvar(movimento);
			await _idempotenciaQueryService.Salvar(novaIdempotencia);

			return resultado;
		}
	}
}