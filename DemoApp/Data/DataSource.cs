using DemoApp.Models;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Data
{
    public class DataSource
    {
    }

    public class WindFarmContext : DbContext
    {
        public WindFarmContext(DbContextOptions<WindFarmContext> options) : base(options)
        {
            
        }

        public DbSet<Windmill> Windmills { get; set; }
        public DbSet<Blade> Blades { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Turbine> Turbines { get; set; }
        public DbSet<TurbineTelemetrySample> TurbineTelemetrySamples { get; set; }
        public DbSet<DemoApp.Models.TurbineTelemetrySample> TurbineTelemetrySample { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Windmill>().ToTable("Windmill");
            modelBuilder.Entity<Blade>().ToTable("Blade");
            modelBuilder.Entity<Platform>().ToTable("Platform");
            modelBuilder.Entity<Turbine>().ToTable("Turbine");
            modelBuilder.Entity<TurbineTelemetrySample>().ToTable("TurbineTelemetrySample");
        }   
    }

    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }
        
        public DbSet<Book> Books{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
        }
    }

    /* DEMO_CUSTOMIZATION: Add additional DbContext classes here for additional demo DbSet classes as needed
    
    Copy/paste one of the above DbContext classes such as 'BookStoreContext'.
    Rename the class to whatever you want.
    Example: 
    public class BookStoreContext : DbContext --> public class MiningOperationsContext : DbContext

    Change the constructor method to match
    Example:
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) --> public MiningOperationsContext(DbContextOptions<MiningOperationsContext> options) : base(options)

    Change the DbSet to create a new set of data
    Example:
    public DbSet<MiningCart> MiningCarts{ get; set; }

    Change the modelBuilder entity type and table name
    Example:
    modelBuilder.Entity<Book>().ToTable("Book"); --> modelBuilder.Entity<MiningCart>().ToTable("MiningCart");

    Complete example:
    
        public class MiningOperationsContext : DbContext
        {
            public BookStoreContext(DbContextOptions<MiningOperationsContext> options) : base(options)
            {

            }
        
            public DbSet<MiningCart> MiningCarts{ get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<MiningCart>().ToTable("MiningCart");
            }
        }
     */
}