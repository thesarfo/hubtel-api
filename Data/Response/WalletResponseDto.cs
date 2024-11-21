using Hubtel.Api.Entities;

namespace Hubtel.Api.Data.Response;

public record WalletResponseDto(Guid Id, string Name, string AccountNumber, string AccountScheme, string Type, string Owner, DateTime CreatedAt)
{
    public static WalletResponseDto ToWalletDto(Wallet wallet)
    {
        ArgumentNullException.ThrowIfNull(wallet);

        return new WalletResponseDto(wallet.Id,
            wallet.Name,
            wallet.AccountNumber,
            wallet.AccountScheme.ToString(),
            wallet.Type.ToString(),
            wallet.Owner,
            wallet.CreatedAt);
    }
}



