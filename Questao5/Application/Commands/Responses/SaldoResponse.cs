namespace Questao5.Application.Commands.Responses
{
	public class SaldoResponse
	{
		public string IdContaCorrente { get; set; }
		public string NomeTitular { get; set; }
		public string NumeroContaCorrente { get; set; }
		public DateTime DataHoraResposta { get; set; }
		public decimal SaldoAtual { get; set; }
	}
}