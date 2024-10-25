namespace Questao5.Domain.Entities
{
	public class BusinessException : Exception
	{
		public string Type { get; }
		public string Mensagem { get; }

		public BusinessException(string tyoe, string mensagem) : base(mensagem)
		{
			Type = tyoe;
			Mensagem = mensagem;
		}
	}
}