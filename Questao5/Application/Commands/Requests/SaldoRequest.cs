using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
	public class SaldoRequest : IRequest<SaldoResponse>
	{
		public string IdContaCorrente { get; set; }
	}
}