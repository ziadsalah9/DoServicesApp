using DoServicesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DoServicesApp.Data
{
    public class OrderDbContext :DbContext
    {

        public OrderDbContext(DbContextOptions<OrderDbContext>options):base(options)
        {
                
        }

        public DbSet<Order> Orders { get; set; }
    }
}
