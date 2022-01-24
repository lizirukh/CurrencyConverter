using Microsoft.EntityFrameworkCore;

namespace App2.Models
{
    public class ExchangeDetailContext : DbContext
    {
        public ExchangeDetailContext(DbContextOptions<ExchangeDetailContext> options) : base(options)
        {

        }

        public DbSet<ExchangeDetail> ExchangeDetails { get; set; }
    }
}
