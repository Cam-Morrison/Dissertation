using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace backend.entity
{
    public partial class dbContext : DbContext
    {
        private IConfiguration _config;
        public dbContext(IConfiguration config)
        {
            _config = config;
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
            Console.WriteLine(options.ContextType.Assembly);
        }

        public virtual DbSet<Action> Actions { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Watchlist> Watchlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_config.GetValue<string>("ClientConfiguration:dBContextSecret"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));
            }
        }
    }
}
