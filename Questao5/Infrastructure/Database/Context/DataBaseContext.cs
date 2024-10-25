using Microsoft.EntityFrameworkCore;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Context
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
		public DbSet<ContaCorrente> ContasCorrentes { get; set; }
		public DbSet<Idempotencia> Idempotencias { get; set; }
		public DbSet<Movimento> Movimento { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ContaCorrente>()
				.HasKey(c => c.IdContaCorrente);
			modelBuilder.Entity<Idempotencia>()
				.HasKey(i => i.ChaveIdempotencia);
			modelBuilder.Entity<Movimento>()
				.HasKey(i => i.IdMovimento);
		}
	}
}
