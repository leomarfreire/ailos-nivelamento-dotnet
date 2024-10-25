using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
	public class ContaCorrente
	{
		[Key]
		[Column("idcontacorrente")]
		public string IdContaCorrente { get; set; }

		[Column("numero")]
		public int Numero { get; set; }

		[Column("nome")]
		public string Nome { get; set; }

		[Column("ativo")]
		public bool Ativo { get; set; }
	}
}