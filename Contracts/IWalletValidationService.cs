using Hubtel.Api.Entities;

namespace Hubtel.Api.Contracts;

public interface IWalletValidationService
{
    Task<bool> IsAccountNumberUniqueAsync(string accountNumber);
    Task<bool> CanAddMoreWalletsAsync(string phoneNumber);
    void ValidateWallet(Wallet wallet);
}