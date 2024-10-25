using System;
using System.Globalization;

namespace Questao1
{
	class ContaBancaria
	{
		public int Numero { get; private set; }
		public string Titular { get; set; }
		public double Saldo { get; private set; }
		private const double TaxaSaque = 3.50;

		public ContaBancaria(int numero, string titular)
		{
			this.Numero = numero;
			this.Titular = titular;
			this.Saldo = 0;
		}

		public ContaBancaria(int numero, string titular, double depositoInicial) : this(numero, titular)
		{
			this.Deposito(depositoInicial);
		}

		public override string ToString()
		{
			return $"Dados da conta: Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("0.00", CultureInfo.InvariantCulture)}";
		}

		public void Deposito(double valor)
		{
			if (valor > 0)
			{
				Saldo += valor;
			}
			else
			{
				Console.WriteLine("O valor do depósito deve ser positivo.");
			}
		}

		public void Saque(double valor)
		{
			double valorTotalSaque = valor + TaxaSaque;

			if (valor > 0)
			{
				Saldo -= valorTotalSaque;
			}
			else
			{
				Console.WriteLine("O valor do saque deve ser positivo.");
			}
		}
	}
}