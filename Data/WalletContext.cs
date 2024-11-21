using Hubtel.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Api.Data;

public class WalletContext : DbContext
{
    public WalletContext(DbContextOptions<WalletContext> options) : base(options)
    {
        
    }
    public DbSet<Wallet> Wallets { get; set; }
}