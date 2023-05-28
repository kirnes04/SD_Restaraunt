using Microsoft.EntityFrameworkCore;
using SD_Restaurant.Models;

namespace SD_Restaurant.context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
    public DbSet<SD_Restaurant.Models.User>? User { get; set; }
    public DbSet<SD_Restaurant.Models.Session>? Session { get; set; }
    public DbSet<SD_Restaurant.Models.Order>? Order { get; set; }
    public DbSet<SD_Restaurant.Models.Dish>? Dish { get; set; }
    public DbSet<SD_Restaurant.Models.OrderDish>? OrderDish { get; set; }
}