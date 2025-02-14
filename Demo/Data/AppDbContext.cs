using Demo.Model;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employee { get; set; }

       // public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
