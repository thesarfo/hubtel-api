using Hubtel.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Api.Data;

public class WalletContext : DbContext
{
    public WalletContext(DbContextOptions<WalletContext> options) : base(options)
    {
        
    }
    public DbSet<Wallet> Wallets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wallet>()
            .Property(w => w.Id)
            .HasColumnName("wallet_id");

        modelBuilder.Entity<Wallet>()
            .Property(w => w.Name)
            .HasColumnName("wallet_name");

        modelBuilder.Entity<Wallet>()
            .Property(w => w.Type)
            .HasColumnName("wallet_type");

        modelBuilder.Entity<Wallet>()
            .Property(w => w.AccountNumber)
            .HasColumnName("account_number");

        modelBuilder.Entity<Wallet>()
            .Property(w => w.AccountScheme)
            .HasColumnName("account_scheme");

        modelBuilder.Entity<Wallet>()
            .Property(w => w.CreatedAt)
            .HasColumnName("created_at");

        modelBuilder.Entity<Wallet>()
            .Property(w => w.Owner)
            .HasColumnName("owner_phone");
    }
}