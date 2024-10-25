using System;
using System.Globalization;

namespace Questao1
{
	class Program
	{
		static void Main(string[] args)
		{
			ContaBancaria conta;

			Console.Write("Entre o número da conta: ");
			int numero = int.Parse(Console.ReadLine());

			Console.Write("Entre o titular da conta: ");
			string titular = Console.ReadLine();

			char resp;

			do
			{
				Console.Write("Haverá depósito inicial (s/n)? ");
				resp = char.Parse(Console.ReadLine());

				if (resp != 's' && resp != 'S' && resp != 'n' && resp != 'N')
				{
					Console.WriteLine("Entrada inválida. Digite 's' para sim ou 'n' para não.");
				}

			} while (resp != 's' && resp != 'S' && resp != 'n' && resp != 'N');

			if (resp == 's' || resp == 'S') {
			    Console.Write("Entre o valor de depósito inicial: ");
			    double depositoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
			    conta = new ContaBancaria(numero, titular, depositoInicial);
			}
			else {
			    conta = new ContaBancaria(numero, titular);
			}

			Console.WriteLine();
			Console.WriteLine("Dados da conta:");
			Console.WriteLine(conta);

			Console.WriteLine();
			Console.Write("Entre um valor para depósito: ");
			double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
			conta.Deposito(quantia);
			Console.WriteLine("Dados da conta atualizados:");
			Console.WriteLine(conta);

			Console.WriteLine();
			Console.Write("Entre um valor para saque: ");
			quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
			conta.Saque(quantia);
			Console.WriteLine("Dados da conta atualizados:");
			Console.WriteLine(conta);
		}
	}
}
