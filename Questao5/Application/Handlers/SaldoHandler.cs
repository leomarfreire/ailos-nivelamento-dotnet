using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.Context;

namespace Questao5.Application.Handlers
{
	public class SaldoHandler : IRequestHandler<SaldoRequest, SaldoResponse>
	{
		private readonly ContaCorrenteQueryService _contaCorrenteQueryService;
		private readonly MovimentoQueryService _movimentoQueryService;
		private readonly DatabaseContext _dbContext;

		public SaldoHandler(ContaCorrenteQueryService contaCorrenteQueryService, MovimentoQueryService movimentoQueryService, DatabaseContext dbContext)
		{
			_contaCorrenteQueryService = contaCorrenteQueryService;
			_movimentoQueryService = movimentoQueryService;
			_dbContext = dbContext;
		}

		public async Task<SaldoResponse> Handle(SaldoRequest request, CancellationToken cancellationToken)
		{
			var contaCorrente = await _contaCorrenteQueryService.ObterPorId(request.IdContaCorrente);
			if (contaCorrente == null)
			{
				throw new BusinessException("INVALID_ACCOUNT", "Apenas contas correntes cadastradas podem receber movimentação");
			}
			else if (!contaCorrente.Ativo)
			{
				throw new BusinessException("INVALID_ACCOUNT", "Apenas contas correntes ativas podem receber movimentação");
			}

			decimal saldo = await _movimentoQueryService.ObterSaldoPorId(request.IdContaCorrente);

			return new SaldoResponse
			{
				IdContaCorrente = contaCorrente.IdContaCorrente,
				NomeTitular = contaCorrente.Nome,
				NumeroContaCorrente = contaCorrente.Numero.ToString(),
				SaldoAtual = saldo,
				DataHoraResposta = DateTime.UtcNow
			};
		}
	}
}