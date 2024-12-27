using Microsoft.EntityFrameworkCore;

namespace AppTest.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{
		}
		public DbSet<Model.PaymentDetail> PaymentDetails { get; set; }
	}
}
