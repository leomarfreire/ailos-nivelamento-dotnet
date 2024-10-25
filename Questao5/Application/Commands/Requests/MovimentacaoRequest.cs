using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Questao5.Application.Commands.Requests
{
	public class MovimentacaoRequest : IRequest<string>
	{
		[Required(ErrorMessage = "O campo IdentificacaoRequisicao é obrigatório.")]
		public string IdentificacaoRequisicao { get; set; }

		[Required(ErrorMessage = "O campo IdentificacaoContaCorrente é obrigatório.")]
		public string IdentificacaoContaCorrente { get; set; }

		[Range(0.01, double.MaxValue, ErrorMessage = "O valor da movimentação deve ser positivo.")]
		public decimal Valor { get; set; }

		[Required(ErrorMessage = "O campo TipoMovimento é obrigatório.")]
		[RegularExpression("^[cCdD]$", ErrorMessage = "Tipo de movimento deve ser 'C' para crédito ou 'D' para débito.")]
		public string TipoMovimento { get; set; }
	}
}